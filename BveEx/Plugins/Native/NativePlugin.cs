using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

using BveEx.Extensions.Native;
using BveEx.Extensions.Native.Input;

namespace BveEx.Plugins.Native
{
    [Plugin(PluginType.VehiclePlugin)]
    internal sealed class NativePlugin : PluginBase
    {
        private readonly INative Native;
        private readonly Library Library;

        public override string Location { get; }
        public override string Name { get; }
        public override string Title { get; } = "(Native)";
        public override string Version { get; }
        public override string Description { get; }
        public override string Copyright { get; }

        public NativePlugin(NativePluginBuilder builder) : base(builder)
        {
            Native = Extensions.GetExtension<INative>();
            Library = new Library(builder.LibraryPath);

            Location = builder.LibraryPath;
            Name = Path.GetFileName(Location);

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(Location);
            Version = fileVersionInfo.FileVersion;
            Description = fileVersionInfo.FileDescription;
            Copyright = fileVersionInfo.LegalCopyright;

            Native.VehicleSpecLoaded += OnLoaded;
            Native.Started += OnStarted;
            Native.AtsKeys.AnyKeyPressed += OnKeyDown;
            Native.AtsKeys.AnyKeyReleased += OnKeyUp;
            Native.HornBlown += OnHornBlown;
            Native.DoorOpened += OnDoorOpened;
            Native.DoorClosed += OnDoorClosed;
            Native.SignalUpdated += OnSignalUpdated;
            Native.BeaconPassed += OnBeaconPassed;

            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            Library.DisposeProc?.Invoke();
            Library.Dispose();

            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            AtsPlugin atsPlugin = e.Scenario.Vehicle.Instruments.AtsPlugin;

            atsPlugin.Handles.PowerChanged += OnPowerChanged;
            atsPlugin.Handles.BrakeChanged += OnBrakeChanged;
            atsPlugin.Handles.ReverserChanged += OnReverserChanged;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            INative native = Extensions.GetExtension<INative>();

            Library.Load?.Invoke();
            Library.SetVehicleSpec?.Invoke(new Imports.VehicleSpec()
            {
                BrakeNotches = native.VehicleSpec.BrakeNotches,
                PowerNotches = native.VehicleSpec.PowerNotches,
                AtsNotch = native.VehicleSpec.AtsNotch,
                B67Notch = native.VehicleSpec.B67Notch,
                Cars = native.VehicleSpec.Cars,
            });
        }

        private void OnStarted(object sender, StartedEventArgs e) => Library.Initialize?.Invoke((int)e.DefaultBrakePosition);
        private void OnPowerChanged(object sender, ValueEventArgs<int> e) => Library.SetPower?.Invoke(((HandleSet)sender).PowerNotch);
        private void OnBrakeChanged(object sender, ValueEventArgs<int> e) => Library.SetBrake?.Invoke(((HandleSet)sender).BrakeNotch);
        private void OnReverserChanged(object sender, ValueEventArgs<int> e) => Library.SetReverser?.Invoke((int)((HandleSet)sender).ReverserPosition);
        private void OnKeyDown(object sender, AtsKeyEventArgs e) => Library.KeyDown?.Invoke((int)e.KeyName);
        private void OnKeyUp(object sender, AtsKeyEventArgs e) => Library.KeyUp?.Invoke((int)e.KeyName);
        private void OnHornBlown(object sender, HornBlownEventArgs e) => Library.HornBlow?.Invoke((int)e.HornType);
        private void OnDoorOpened(object sender, EventArgs e) => Library.DoorOpen?.Invoke();
        private void OnDoorClosed(object sender, EventArgs e) => Library.DoorClose?.Invoke();
        private void OnSignalUpdated(object sender, SignalUpdatedEventArgs e) => Library.SetSignal?.Invoke(e.SignalIndex);
        private void OnBeaconPassed(object sender, BeaconPassedEventArgs e)
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
            if (Library.Elapse is null) return;

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
