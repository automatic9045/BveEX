using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;

using BveEx.Native;

namespace BveEx
{
    internal partial class BveEx
    {
        private void ListenPatchEvents()
        {
            Patches.LoadScenarioPatch.Invoked += (sender, e) =>
            {
                ScenarioInfo scenarioInfo = ScenarioInfo.FromSource(e.Args[0]);

                ScenarioOpened?.Invoke(this, new ValueEventArgs<ScenarioInfo>(scenarioInfo));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.DisposeScenarioPatch.Invoked += (sender, e) =>
            {
                Scenario scenario = Scenario.FromSource(e.Instance);

                ScenarioClosed?.Invoke(this, new ValueEventArgs<Scenario>(scenario));
                return new PatchInvokationResult(SkipModes.Continue);
            };


            Patches.OnSetBeaconDataPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);
                ObjectPassedEventArgs args = ObjectPassedEventArgs.FromSource(e.Args[1]);

                Beacon beacon = (Beacon)args.MapObject;
                Section section = beacon.TargetSectionIndex == int.MaxValue
                    ? atsPlugin.SectionManager.LastSection
                    : beacon.TargetSectionIndex >= 0 && atsPlugin.SectionManager.Sections.Count > beacon.TargetSectionIndex ? (Section)atsPlugin.SectionManager.Sections[beacon.TargetSectionIndex] : null;

                BeaconData beaconData = new BeaconData()
                {
                    Data = beacon.SendData,
                    Z = section is null ? 0f : (float)(section.Location - atsPlugin.LocationManager.Location),
                    Sig = section is null ? 0 : section.CurrentSignalIndex,
                    Num = beacon.Type,
                };

                OnSetBeaconData?.Invoke(this, new ValueEventArgs<BeaconData>(beaconData));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnKeyDownPatch.Invoked += (sender, e) =>
            {
                InputEventArgs args = (InputEventArgs)e.Args[1];

                int axis = args.Axis;
                int value = args.Value & 65535;

                switch (axis)
                {
                    case -1:
                        switch (value)
                        {
                            case 0:
                                OnHornBlow?.Invoke(this, new ValueEventArgs<HornType>(HornType.Primary));
                                break;

                            case 1:
                                OnHornBlow?.Invoke(this, new ValueEventArgs<HornType>(HornType.Secondary));
                                break;

                            case 3:
                                OnHornBlow?.Invoke(this, new ValueEventArgs<HornType>(HornType.Music));
                                break;
                        }
                        break;

                    case -2:
                        OnKeyDown?.Invoke(this, new ValueEventArgs<ATSKeys>((ATSKeys)value));
                        break;
                }

                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnKeyUpPatch.Invoked += (sender, e) =>
            {
                InputEventArgs args = (InputEventArgs)e.Args[1];

                int axis = args.Axis;
                int value = args.Value & 65535;

                if (axis == -2)
                {
                    OnKeyUp?.Invoke(this, new ValueEventArgs<ATSKeys>((ATSKeys)value));
                }
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnDoorStateChangedPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);

                if (atsPlugin.Doors.AreAllClosed)
                {
                    OnDoorClose?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    OnDoorOpen?.Invoke(this, EventArgs.Empty);
                }
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnSetSignalPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);
                MapFunctionList sections = atsPlugin.SectionManager.Sections;

                int currentSectionSignalIndex = sections.CurrentIndex < 0 ? int.MaxValue : ((Section)sections[sections.CurrentIndex]).CurrentSignalIndex;

                OnSetSignal?.Invoke(this, new ValueEventArgs<int>(currentSectionSignalIndex));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnSetReverserPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);

                OnSetReverser?.Invoke(this, new ValueEventArgs<int>((int)atsPlugin.Handles.ReverserPosition));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnSetBrakePatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);

                OnSetBrake?.Invoke(this, new ValueEventArgs<int>(atsPlugin.Handles.BrakeNotch));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnSetPowerPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);

                OnSetPower?.Invoke(this, new ValueEventArgs<int>(atsPlugin.Handles.PowerNotch));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnSetVehicleSpecPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);
                if (!atsPlugin.IsPluginLoaded) return new PatchInvokationResult(SkipModes.Continue);

                NotchInfo notchInfo = NotchInfo.FromSource(e.Args[0]);
                int carCount = (int)e.Args[1];

                VehicleSpec vehicleSpec = new VehicleSpec()
                {
                    PowerNotches = notchInfo.PowerNotchCount,
                    BrakeNotches = notchInfo.BrakeNotchCount,
                    B67Notch = notchInfo.B67Notch,
                    AtsNotch = notchInfo.AtsCancelNotch,
                    Cars = carCount,
                };

                OnSetVehicleSpec?.Invoke(this, new ValueEventArgs<VehicleSpec>(vehicleSpec));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.OnInitializePatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);
                if (!atsPlugin.IsPluginLoaded) return new PatchInvokationResult(SkipModes.Continue);

                BrakePosition brakePosition = (BrakePosition)e.Args[0];

                OnInitialize?.Invoke(this, new ValueEventArgs<DefaultBrakePosition>((DefaultBrakePosition)brakePosition));
                return new PatchInvokationResult(SkipModes.Continue);
            };

            Patches.PostElapsePatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);
                if (!atsPlugin.IsPluginLoaded) return new PatchInvokationResult(SkipModes.Continue);

                VehicleState vehicleState = new VehicleState()
                {
                    Location = atsPlugin.LocationManager.Location,
                    Speed = (float)atsPlugin.StateStore.Speed[0],
                    Time = (int)e.Args[0],
                    BcPressure = (float)atsPlugin.StateStore.BcPressure[0],
                    MrPressure = (float)atsPlugin.StateStore.MrPressure[0],
                    ErPressure = (float)atsPlugin.StateStore.ErPressure[0],
                    BpPressure = (float)atsPlugin.StateStore.BpPressure[0],
                    SapPressure = (float)atsPlugin.StateStore.SapPressure[0],
                    Current = (float)atsPlugin.StateStore.Current[0],
                };

                PostElapse?.Invoke(this, new OnElapseEventArgs(vehicleState, atsPlugin.PanelArray, atsPlugin.SoundArray));
                return new PatchInvokationResult(SkipModes.Continue);
            };
        }
    }
}
