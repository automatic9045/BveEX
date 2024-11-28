using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Input.Native;
using BveEx.PluginHost.Native;
using BveEx.PluginHost.Plugins;

namespace BveEx.Plugins.Native
{
    [Plugin(PluginType.VehiclePlugin)]
    internal sealed class NativePlugin : PluginBase
    {
        private readonly Library Library;

        private bool IgnoreHandleUpdates = false;

        public override string Location { get; }
        public override string Name { get; }
        public override string Title { get; } = "(Native)";
        public override string Version { get; }
        public override string Description { get; }
        public override string Copyright { get; }

        public NativePlugin(NativePluginBuilder builder) : base(builder)
        {
            Library = new Library(builder.LibraryPath);

            Location = builder.LibraryPath;
            Name = Path.GetFileName(Location);

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(Location);
            Version = fileVersionInfo.FileVersion;
            Description = fileVersionInfo.FileDescription;
            Copyright = fileVersionInfo.LegalCopyright;

            Library.Load?.Invoke();
            Library.SetVehicleSpec?.Invoke(new Imports.VehicleSpec()
            {
                BrakeNotches = Native.VehicleSpec.BrakeNotches,
                PowerNotches = Native.VehicleSpec.PowerNotches,
                AtsNotch = Native.VehicleSpec.AtsNotch,
                B67Notch = Native.VehicleSpec.B67Notch,
                Cars = Native.VehicleSpec.Cars,
            });

            Native.PostTick += PostTick;

            Native.Started += OnStarted;
            Native.Handles.Power.NotchChanged += OnPowerChanged;
            Native.Handles.Brake.NotchChanged += OnBrakeChanged;
            Native.Handles.Reverser.PositionChanged += OnReverserChanged;
            Native.NativeKeys.AnyKeyPressed += OnKeyDown;
            Native.NativeKeys.AnyKeyReleased += OnKeyUp;
            Native.HornBlown += OnHornBlown;
            Native.DoorOpened += OnDoorOpened;
            Native.DoorClosed += OnDoorClosed;
            Native.SignalUpdated += OnSignalUpdated;
            Native.BeaconPassed += OnBeaconPassed;
        }

        public override void Dispose()
        {
            Native.PostTick -= PostTick;

            Native.Started -= OnStarted;
            Native.BeaconPassed -= OnBeaconPassed;
            Native.DoorOpened -= OnDoorOpened;
            Native.DoorClosed -= OnDoorClosed;
            Native.NativeKeys.AnyKeyPressed -= OnKeyDown;
            Native.NativeKeys.AnyKeyReleased -= OnKeyUp;

            Library.DisposeProc?.Invoke();
            Library.Dispose();
        }

        private void PostTick(object sender, EventArgs e)
        {
            IgnoreHandleUpdates = false;
        }

        private void OnStarted(StartedEventArgs e) => Library.Initialize?.Invoke((int)e.DefaultBrakePosition);
        private void OnPowerChanged(object sender, EventArgs e)
        {
            if (!IgnoreHandleUpdates) Library.SetPower?.Invoke(Native.Handles.Power.Notch);
        }
        private void OnBrakeChanged(object sender, EventArgs e)
        {
            if (!IgnoreHandleUpdates) Library.SetBrake?.Invoke(Native.Handles.Brake.Notch);
        }
        private void OnReverserChanged(object sender, EventArgs e)
        {
            if (!IgnoreHandleUpdates) Library.SetReverser?.Invoke((int)Native.Handles.Reverser.Position);
        }
        private void OnKeyDown(object sender, NativeKeyEventArgs e) => Library.KeyDown?.Invoke((int)e.KeyName);
        private void OnKeyUp(object sender, NativeKeyEventArgs e) => Library.KeyUp?.Invoke((int)e.KeyName);
        private void OnHornBlown(HornBlownEventArgs e) => Library.HornBlow?.Invoke((int)e.HornType);
        private void OnDoorOpened(DoorEventArgs e) => Library.DoorOpen?.Invoke();
        private void OnDoorClosed(DoorEventArgs e) => Library.DoorClose?.Invoke();
        private void OnSignalUpdated(SignalUpdatedEventArgs e) => Library.SetSignal?.Invoke(e.SignalIndex);
        private void OnBeaconPassed(BeaconPassedEventArgs e)
        {
            Library.SetBeaconData?.Invoke(new Imports.BeaconData()
            {
                Type = e.Type,
                Signal = e.SignalIndex,
                Distance = e.Distance,
                Optional = e.Optional,
            });
        }

        public override void Tick(TimeSpan elapsed)
        {
            IgnoreHandleUpdates = true;

            if (!(Library.Elapse is null))
            {
                Imports.VehicleState vehicleState = new Imports.VehicleState()
                {
                    Location = Native.VehicleState.Location,
                    Speed = Native.VehicleState.Speed,
                    Time = (int)Native.VehicleState.Time.TotalMilliseconds,
                    BcPressure = Native.VehicleState.BcPressure,
                    MrPressure = Native.VehicleState.MrPressure,
                    ErPressure = Native.VehicleState.ErPressure,
                    BpPressure = Native.VehicleState.BpPressure,
                    SapPressure = Native.VehicleState.SapPressure,
                    Current = Native.VehicleState.Current,
                };
                AtsPlugin atsPlugin = BveHacker.Scenario.Vehicle.Instruments.AtsPlugin;

                Imports.Handles handles = Library.Elapse.Invoke(vehicleState, atsPlugin.PanelArray, atsPlugin.SoundArray);

                HandleSet atsHandles = BveHacker.Scenario.Vehicle.Instruments.AtsPlugin.AtsHandles;
                atsHandles.PowerNotch = handles.Power;
                atsHandles.BrakeNotch = handles.Brake;
                atsHandles.ReverserPosition = (ReverserPosition)handles.Reverser;
                atsHandles.ConstantSpeedMode = (ConstantSpeedMode)handles.ConstantSpeed;
            }
        }
    }
}
