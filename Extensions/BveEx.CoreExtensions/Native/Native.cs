using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Mackoy.Bvets;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.PluginHost;
using BveEx.PluginHost.Input;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

using BveEx.Extensions.Native.Input;

namespace BveEx.Extensions.Native
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(INative))]
    internal class Native : AssemblyPluginBase, INative
    {
        private readonly PatchSet Patches;

        private ScenarioService ScenarioService = null;

        public bool IsAvailable => !(ScenarioService is null);

        public IList<int> AtsPanelArray => ScenarioService?.AtsPanelValues;
        public IList<int> AtsSoundArray => ScenarioService?.AtsSounds;
        public AtsKeySet AtsKeys => ScenarioService?.AtsKeys;
        public VehicleSpec VehicleSpec => ScenarioService?.VehicleSpec;
        public VehicleState VehicleState => ScenarioService?.VehicleState;

        public event EventHandler Opened;
        public event EventHandler Closed;

        public event EventHandler VehicleSpecLoaded;
        public event EventHandler<StartedEventArgs> Started;
        public event EventHandler<HornBlownEventArgs> HornBlown;
        public event EventHandler DoorOpened;
        public event EventHandler DoorClosed;
        public event EventHandler<SignalUpdatedEventArgs> SignalUpdated;
        public event EventHandler<BeaconPassedEventArgs> BeaconPassed;

        public Native(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet atsPluginMembers = BveHacker.BveTypes.GetClassInfoOf<AtsPlugin>();
            Patches = new PatchSet(atsPluginMembers);

            Patches.ConstructorPatch.Invoked += (sender, e) =>
            {
                AtsPlugin instance = AtsPlugin.FromSource(e.Instance);

                ScenarioService = new ScenarioService(instance);
                Opened?.Invoke(this, EventArgs.Empty);

                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnSetVehicleSpecPatch.Invoked += (sender, e) =>
            {
                NotchInfo notchInfo = NotchInfo.FromSource(e.Args[0]);
                int carCount = (int)e.Args[1];

                ScenarioService.VehicleSpec = new VehicleSpec(notchInfo.BrakeNotchCount, notchInfo.PowerNotchCount, notchInfo.AtsCancelNotch, notchInfo.B67Notch, carCount);
                VehicleSpecLoaded?.Invoke(this, EventArgs.Empty);

                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnInitializePatch.Invoked += (sender, e) =>
            {
                BrakePosition brakePosition = (BrakePosition)e.Args[0];
                Started?.Invoke(this, new StartedEventArgs(brakePosition));

                return PatchInvokationResult.DoNothing(e);
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
                                HornBlown?.Invoke(this, new HornBlownEventArgs(HornType.Primary));
                                break;

                            case 1:
                                HornBlown?.Invoke(this, new HornBlownEventArgs(HornType.Secondary));
                                break;

                            case 3:
                                HornBlown?.Invoke(this, new HornBlownEventArgs(HornType.Music));
                                break;
                        }
                        break;

                    case -2:
                        ScenarioService.AtsKeys.NotifyPressed((AtsKeyName)value);
                        break;
                }

                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnKeyUpPatch.Invoked += (sender, e) =>
            {
                InputEventArgs args = (InputEventArgs)e.Args[1];

                int axis = args.Axis;
                int value = args.Value & 65535;

                if (axis == -2)
                {
                    ScenarioService.AtsKeys.NotifyReleased((AtsKeyName)value);
                }

                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnDoorStateChangedPatch.Invoked += (sender, e) =>
            {
                AtsPlugin instance = AtsPlugin.FromSource(e.Instance);

                if (instance.Doors.AreAllClosed)
                {
                    DoorClosed?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    DoorOpened?.Invoke(this, EventArgs.Empty);
                }

                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnSetSignalPatch.Invoked += (sender, e) =>
            {
                AtsPlugin instance = AtsPlugin.FromSource(e.Instance);

                MapFunctionList sections = instance.SectionManager.Sections;
                int signalIndex = sections.CurrentIndex < 0 ? int.MaxValue : ((Section)sections[sections.CurrentIndex]).CurrentSignalIndex;

                SignalUpdated?.Invoke(this, new SignalUpdatedEventArgs(signalIndex));

                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnSetBeaconDataPatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);
                ObjectPassedEventArgs args = ObjectPassedEventArgs.FromSource(e.Args[1]);

                Beacon beacon = (Beacon)args.MapObject;
                Section section = beacon.TargetSectionIndex == int.MaxValue ? atsPlugin.SectionManager.LastSection
                    : 0 <= beacon.TargetSectionIndex && beacon.TargetSectionIndex < atsPlugin.SectionManager.Sections.Count ? (Section)atsPlugin.SectionManager.Sections[beacon.TargetSectionIndex]
                    : null;

                BeaconPassedEventArgs beaconPassedArgs = new BeaconPassedEventArgs(
                    beacon.Type,
                    section is null ? 0 : section.CurrentSignalIndex,
                    section is null ? 0f : (float)(section.Location - atsPlugin.Location.Location),
                    beacon.SendData);

                BeaconPassed?.Invoke(this, beaconPassedArgs);

                return PatchInvokationResult.DoNothing(e);
            };

            BveHacker.ScenarioClosed += OnScenarioClosed;
            BveHacker.PreviewTick += OnPreviewTick;
        }

        public override void Dispose()
        {
            Patches.Dispose();

            BveHacker.ScenarioClosed -= OnScenarioClosed;
            BveHacker.PreviewTick -= OnPreviewTick;
        }

        private void OnScenarioClosed(EventArgs e)
        {
            ScenarioService = null;

            VehicleSpecLoaded = null;
            Started = null;
            HornBlown = null;
            DoorOpened = null;
            DoorClosed = null;
            SignalUpdated = null;
            BeaconPassed = null;

            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void OnPreviewTick(object sender, EventArgs e)
        {
            AtsPlugin atsPlugin = BveHacker.Scenario.Vehicle.Instruments.AtsPlugin;

            ScenarioService.VehicleState = new VehicleState(
                atsPlugin.Location.Location,
                (float)atsPlugin.StateStore.Speed[0],
                BveHacker.Scenario.TimeManager.Time,
                (float)atsPlugin.StateStore.BcPressure[0],
                (float)atsPlugin.StateStore.MrPressure[0],
                (float)atsPlugin.StateStore.ErPressure[0],
                (float)atsPlugin.StateStore.BpPressure[0],
                (float)atsPlugin.StateStore.SapPressure[0],
                (float)atsPlugin.StateStore.Current[0]);
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
