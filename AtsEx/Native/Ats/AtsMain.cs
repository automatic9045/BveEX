using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;
using BveTypes.ClassWrappers;
using UnembeddedResources;

using AtsEx.Handles;
using AtsEx.Plugins;
using AtsEx.Troubleshooting;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Input.Native;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Native.Ats
{
    /// <summary>メインの機能をここに実装する。</summary>
    public static class AtsMain
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(AtsMain), "Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> BveVersionNotSupported { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CallerVersionNotSupported { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static AtsMain()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private static bool IsLoadedAsInputDevice = false;
        private static CallerInfo CallerInfo;
        private static TroubleshooterSet Troubleshooters;

        private static AtsEx.AsAtsPlugin AtsEx;
        private static ScenarioService.AsAtsPlugin ScenarioService;
        private static FrameSpan FrameSpan;

        private static int Power;
        private static int Brake;
        private static int Reverser;

        internal static event EventHandler<VehiclePluginUsingLoadedEventArgs> VehiclePluginUsingLoaded;

        internal static void LoadedAsInputDevice()
        {
            IsLoadedAsInputDevice = true;
        }

        public static void Load(CallerInfo callerInfo)
        {
            CallerInfo = callerInfo;

            Version callerVersion = CallerInfo.AtsExCallerAssembly.GetName().Version;
            if (callerVersion < new Version(0, 16))
            {
                string errorMessage = string.Format(Resources.Value.CallerVersionNotSupported.Value, "AtsEX", callerVersion);
                ErrorDialog.Show(2, errorMessage);
                throw new NotSupportedException(errorMessage.Replace("\n", ""));
            }

            if (IsLoadedAsInputDevice) return;

            AppInitializer.Initialize(CallerInfo, LaunchMode.Ats);

            Troubleshooters = TroubleshooterSet.Load();

            BveTypeSetLoader bveTypesLoader = new BveTypeSetLoader();
            BveTypeSetLoader.ProfileForDifferentVersionBveLoadedEventArgs args = null;
            bveTypesLoader.ProfileForDifferentVersionBveLoaded += (sender, e) => args = e;

            BveTypeSet bveTypes = bveTypesLoader.Load();

            AtsEx = new AtsEx.AsAtsPlugin(bveTypes);

            if (!(args is null))
            {
                string versionWarningText = string.Format(Resources.Value.BveVersionNotSupported.Value, args.BveVersion, args.ProfileVersion, App.Instance.ProductShortName);
                AtsEx.BveHacker.LoadErrorManager.Throw(versionWarningText);
            }
        }

        public static void Dispose()
        {
            if (IsLoadedAsInputDevice) return;

            FrameSpan = null;

            ScenarioService?.Dispose();
            ScenarioService = null;

            AtsEx?.Dispose();
            AtsEx = null;

            Troubleshooters?.Dispose();
            Troubleshooters = null;
        }

        public static void SetVehicleSpec(VehicleSpec vehicleSpec)
        {
            string callerAssemblyLocation = CallerInfo.AtsExCallerAssembly.Location;

            string vehicleConfigPath = Path.Combine(Path.GetDirectoryName(callerAssemblyLocation), Path.GetFileNameWithoutExtension(callerAssemblyLocation) + ".VehicleConfig.xml");
            VehicleConfig vehicleConfig = File.Exists(vehicleConfigPath) ? VehicleConfig.LoadFrom(vehicleConfigPath)
                : IsLoadedAsInputDevice ? VehicleConfig.Default
                : VehicleConfig.Resolve(AtsEx.BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path);

            string vehiclePluginUsingPath = Path.Combine(Path.GetDirectoryName(callerAssemblyLocation), Path.GetFileNameWithoutExtension(callerAssemblyLocation) + ".VehiclePluginUsing.xml");
            PluginSourceSet vehiclePluginUsing = File.Exists(vehiclePluginUsingPath) ? PluginSourceSet.FromPluginUsing(PluginType.VehiclePlugin, false, vehiclePluginUsingPath)
                : !(vehicleConfig.PluginUsingPath is null) ? PluginSourceSet.FromPluginUsing(PluginType.VehiclePlugin, true, vehicleConfig.PluginUsingPath)
                : IsLoadedAsInputDevice ? PluginSourceSet.Empty(PluginType.VehiclePlugin)
                : PluginSourceSet.ResolvePluginUsingToLoad(PluginType.VehiclePlugin, true, AtsEx.BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path);

            VehiclePluginUsingLoaded?.Invoke(null, new VehiclePluginUsingLoadedEventArgs(vehiclePluginUsing, vehicleConfig));

            if (IsLoadedAsInputDevice) return;

            PluginHost.Native.VehicleSpec exVehicleSpec = new PluginHost.Native.VehicleSpec(
                vehicleSpec.BrakeNotches, vehicleSpec.PowerNotches, vehicleSpec.AtsNotch, vehicleSpec.B67Notch, vehicleSpec.Cars);

            ScenarioService = new ScenarioService.AsAtsPlugin(AtsEx, vehiclePluginUsing, vehicleConfig, exVehicleSpec);
            FrameSpan = new FrameSpan();
        }

        public static void Initialize(DefaultBrakePosition defaultBrakePosition)
        {
            if (IsLoadedAsInputDevice) return;

            ScenarioService?.Started((BrakePosition)defaultBrakePosition);
            FrameSpan.Initialize();
        }

        public static AtsHandles Elapse(VehicleState vehicleState, IntPtr panel, IntPtr sound)
        {
            if (IsLoadedAsInputDevice)
            {
                return new AtsHandles()
                {
                    Brake = Brake,
                    Power = Power,
                    Reverser = Reverser,
                    ConstantSpeed = 0,
                };
            }

            AtsIoArray panelArray = new AtsIoArray(panel);
            AtsIoArray soundArray = new AtsIoArray(sound);

            TimeSpan now = TimeSpan.FromMilliseconds(vehicleState.Time);
            TimeSpan elapsed = FrameSpan.Tick(now);

            PluginHost.Native.VehicleState exVehicleState = new PluginHost.Native.VehicleState(
                vehicleState.Location, vehicleState.Speed, now,
                vehicleState.BcPressure, vehicleState.MrPressure, vehicleState.ErPressure, vehicleState.BpPressure, vehicleState.SapPressure, vehicleState.Current);

            AtsEx.Tick(elapsed);
            TickCommandBuilder tickResult = ScenarioService?.Tick(elapsed, exVehicleState, panelArray, soundArray);

            HandlePositionSet handlePositionSet = tickResult.Compile();

            return new AtsHandles()
            {
                Brake = handlePositionSet.Brake,
                Power = handlePositionSet.Power,
                Reverser = (int)handlePositionSet.ReverserPosition,
                ConstantSpeed = (int)handlePositionSet.ConstantSpeed,
            };
        }

        public static void SetPower(int notch)
        {
            Power = notch;

            if (IsLoadedAsInputDevice) return;

            ScenarioService?.SetPower(notch);
        }

        public static void SetBrake(int notch)
        {
            Brake = notch;

            if (IsLoadedAsInputDevice) return;

            ScenarioService?.SetBrake(notch);
        }

        public static void SetReverser(int position)
        {
            Reverser = position;

            if (IsLoadedAsInputDevice) return;

            ScenarioService?.SetReverser((ReverserPosition)position);
        }

        public static void KeyDown(ATSKeys atsKeyCode)
        {
            if (IsLoadedAsInputDevice) return;

            ScenarioService?.KeyDown((NativeAtsKeyName)atsKeyCode);
        }

        public static void KeyUp(ATSKeys atsKeyCode)
        {
            if (IsLoadedAsInputDevice) return;

            ScenarioService?.KeyUp((NativeAtsKeyName)atsKeyCode);
        }

        public static void DoorOpen()
        {
            if (IsLoadedAsInputDevice) return;

            ScenarioService?.DoorOpened(new DoorEventArgs());
        }
        public static void DoorClose()
        {
            if (IsLoadedAsInputDevice) return;

            ScenarioService?.DoorClosed(new DoorEventArgs());
        }
        public static void HornBlow(HornType hornType)
        {

        }
        public static void SetSignal(int signal)
        {

        }
        public static void SetBeaconData(BeaconData beaconData)
        {
            if (IsLoadedAsInputDevice) return;

            BeaconPassedEventArgs args = new BeaconPassedEventArgs(beaconData.Num, beaconData.Sig, beaconData.Z, beaconData.Data);
            ScenarioService?.BeaconPassed(args);
        }


        internal class VehiclePluginUsingLoadedEventArgs : EventArgs
        {
            public PluginSourceSet VehiclePluginUsing { get; }
            public VehicleConfig VehicleConfig { get; }

            public VehiclePluginUsingLoadedEventArgs(PluginSourceSet vehiclePluginUsing, VehicleConfig vehicleConfig)
            {
                VehiclePluginUsing = vehiclePluginUsing;
                VehicleConfig = vehicleConfig;
            }
        }
    }
}
