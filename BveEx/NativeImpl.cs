using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Input;
using BveEx.Panels;
using BveEx.Plugins;
using BveEx.Sound;
using BveEx.PluginHost;
using BveEx.PluginHost.Input.Native;
using BveEx.PluginHost.Native;
using BveEx.PluginHost.Panels.Native;
using BveEx.PluginHost.Sound.Native;

namespace BveEx
{
    internal class NativeImpl : INative
    {
        public NativeImpl(VehicleSpec vehicleSpec, VehicleConfig vehicleConfigOptions)
        {
            VehicleSpec = vehicleSpec;

            AtsPanelValues = new AtsPanelValueSet(vehicleConfigOptions.DetectPanelValueIndexConflict);
            AtsSounds = new AtsSoundSet(vehicleConfigOptions.DetectSoundIndexConflict);
        }

        public void InvokeStarted(BrakePosition defaultBrakePosition) => Started?.Invoke(new StartedEventArgs(defaultBrakePosition));
        public void InvokeHornBlown(HornType hornType) => HornBlown?.Invoke(new HornBlownEventArgs(hornType));
        public void InvokeDoorOpened() => DoorOpened?.Invoke(new DoorEventArgs());
        public void InvokeDoorClosed() => DoorClosed?.Invoke(new DoorEventArgs());
        public void InvokeSignalUpdated(int signalIndex) => SignalUpdated?.Invoke(new SignalUpdatedEventArgs(signalIndex));
        public void InvokeBeaconPassed(BeaconPassedEventArgs args) => BeaconPassed?.Invoke(args);

        public IAtsPanelValueSet AtsPanelValues { get; }

        public INativeKeySet NativeKeys { get; } = new NativeKeySet();
        public IAtsSoundSet AtsSounds { get; }

        public VehicleSpec VehicleSpec { get; }

        public VehicleState VehicleState { get; set; } = null;

        public event StartedEventHandler Started;
        public event HornBlownEventHandler HornBlown;
        public event DoorEventHandler DoorOpened;
        public event DoorEventHandler DoorClosed;
        public event SignalUpdatedEventHandler SignalUpdated;
        public event BeaconPassedEventHandler BeaconPassed;
    }
}
