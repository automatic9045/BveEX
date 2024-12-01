using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.PluginHost;

namespace BveEx
{
    internal class AtsPluginOverrider : IDisposable
    {
        private static readonly string DummyAtsPath;

        static AtsPluginOverrider()
        {
            string baseDirectory = Path.GetDirectoryName(App.Instance.BveExAssembly.Location);
            DummyAtsPath = $"{baseDirectory}\\BveEx.DummyAts.{(Environment.Is64BitProcess ? "x64" : "x86")}.dll";
        }


        private readonly HarmonyPatch LoadVehiclePatch;
        private readonly HarmonyPatch DisposeVehiclePatch;
        private readonly HarmonyPatch DisposeAtsPatch;

        private bool HasAtsLoadingFailed = false;
        private Vehicle LastVehicle = null;

        public AtsPluginOverrider(LoadingProgressForm loadingProgressForm, ClassMemberSet vehicleMembers, ClassMemberSet atsPluginMembers)
        {
            LoadVehiclePatch = HarmonyPatch.Patch(nameof(AtsPluginOverrider), vehicleMembers.GetSourceMethodOf(nameof(Vehicle.Load)).Source, PatchType.Prefix);
            LoadVehiclePatch.Invoked += (sender, e) =>
            {
                Vehicle instance = Vehicle.FromSource(e.Instance);
                VehicleFile vehicleFile = VehicleFile.FromSource(e.Args[1]);

                HasAtsLoadingFailed = false;

                if (vehicleFile.AtsPluginPath is null)
                {
                    vehicleFile.AtsPluginPath = DummyAtsPath;
                    HasAtsLoadingFailed = true;
                }

                LastVehicle = instance;
                return PatchInvokationResult.DoNothing(e);
            };

            DisposeVehiclePatch = HarmonyPatch.Patch(nameof(AtsPluginOverrider), vehicleMembers.GetSourceMethodOf(nameof(Vehicle.Dispose)).Source, PatchType.Prefix);
            DisposeVehiclePatch.Invoked += (sender, e) =>
            {
                LastVehicle = null;
                return PatchInvokationResult.DoNothing(e);
            };

            DisposeAtsPatch = HarmonyPatch.Patch(nameof(AtsPluginOverrider), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.Dispose)).Source, PatchType.Prefix);
            DisposeAtsPatch.Invoked += (sender, e) =>
            {
                AtsPlugin instance = AtsPlugin.FromSource(e.Instance);

                if (instance.IsPluginLoaded || HasAtsLoadingFailed)
                {
                    return PatchInvokationResult.DoNothing(e);
                }
                else // BVE が ATS プラグインの読込に失敗した場合
                {
                    HasAtsLoadingFailed = true;

                    try
                    {
                        instance.LoadLibrary(DummyAtsPath);

                        if (!(LastVehicle is null))
                        {
                            NotchInfo notchInfo = LastVehicle.Instruments.Cab.Handles.NotchInfo;
                            int carCount = (int)(LastVehicle.Dynamics.MotorCar.Count + LastVehicle.Dynamics.TrailerCar.Count);

                            instance.OnSetVehicleSpec(notchInfo, carCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        loadingProgressForm.ThrowError(ex.Message, DummyAtsPath, 0, 0);
                        instance.Dispose();
                    }

                    return new PatchInvokationResult(SkipModes.SkipPatches | SkipModes.SkipOriginal);
                }
            };
        }

        public void Dispose()
        {
            LoadVehiclePatch.Dispose();
            DisposeVehiclePatch.Dispose();
            DisposeAtsPatch.Dispose();
        }
    }
}
