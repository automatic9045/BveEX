using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using AtsEx.Handles;
using AtsEx.Input;
using AtsEx.Panels;
using AtsEx.Plugins;
using AtsEx.Sound;
using AtsEx.PluginHost.Input.Native;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost;

namespace AtsEx
{
    internal partial class ScenarioService : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ScenarioService>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> VehiclePluginTickResultTypeInvalid { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> MapPluginTickResultTypeInvalid { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static ScenarioService()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


        private readonly AtsEx AtsEx;
        private readonly NativeImpl Native;

        private readonly PluginService _PluginService;

        public Scenario Target { get; private set; } = null;

        public ScenarioService(AtsEx atsEx, PluginSourceSet vehiclePluginUsing, VehicleConfig vehicleConfig, VehicleSpec vehicleSpec)
        {
            AtsEx = atsEx;
            AtsEx.BveHacker.ScenarioCreated += OnScenarioCreated;

            Native = new NativeImpl(vehicleSpec, vehicleConfig);

            PluginLoader pluginLoader = new PluginLoader(Native, AtsEx.BveHacker, AtsEx.Extensions);
            PluginSet plugins = pluginLoader.Load(vehiclePluginUsing);
            _PluginService = new PluginService(plugins, Native.Handles);

            AtsEx.VersionFormProvider.SetScenario(plugins[PluginType.VehiclePlugin].Values, plugins[PluginType.MapPlugin].Values);
        }

        public virtual void Dispose()
        {
            AtsEx.BveHacker.ScenarioCreated -= OnScenarioCreated;

            AtsEx.VersionFormProvider.UnsetScenario();
            _PluginService.Dispose();
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            Target = e.Scenario;
        }

        public void Started(BrakePosition defaultBrakePosition)
        {
            Native.InvokeStarted(defaultBrakePosition);
        }

        public void PreviewTick()
        {
            Native.InvokePreviewTick();
        }

        public void PostTick()
        {
            Native.InvokePostTick();
        }

        public HandlePositionSet Tick(TimeSpan elapsed, VehicleState vehicleState, IList<int> panel, IList<int> sound)
        {
            HandleSet atsHandles = AtsEx.BveHacker.Scenario.Vehicle.Instruments.AtsPlugin.AtsHandles;
            NotifyHandleUpdated();

            Native.VehicleState = vehicleState;
            (Native.AtsPanelValues as AtsPanelValueSet).PreTick(panel);

            AtsEx.BveHacker.Tick(elapsed);
            HandlePositionSet lastHandlePositionSet = _PluginService.Tick(elapsed, handlePositionSet =>
            {
                atsHandles.PowerNotch = handlePositionSet.Power;
                atsHandles.BrakeNotch = handlePositionSet.Brake;
                atsHandles.ReverserPosition = handlePositionSet.ReverserPosition;
                atsHandles.ConstantSpeedMode = handlePositionSet.ConstantSpeed;

                NotifyHandleUpdated();
            });

            (Native.AtsPanelValues as AtsPanelValueSet).Tick(panel);
            (Native.AtsSounds as AtsSoundSet).Tick(sound);

            return lastHandlePositionSet;


            void NotifyHandleUpdated()
            {
                SetPower(atsHandles.PowerNotch, false);
                SetBrake(atsHandles.BrakeNotch, false);
                SetReverser(atsHandles.ReverserPosition, false);
            }
        }

        public void SetPower(int notch, bool forceInvokeEvent)
        {
            PowerHandle powerHandle = (PowerHandle)Native.Handles.Power;
            int oldNotch = powerHandle.Notch;
            powerHandle.Notch = notch;
            if (forceInvokeEvent && notch == oldNotch) powerHandle.InvokeNotchChanged();
        }

        public void SetBrake(int notch, bool forceInvokeEvent)
        {
            BrakeHandle brakeHandle = (BrakeHandle)Native.Handles.Brake;
            int oldNotch = brakeHandle.Notch;
            brakeHandle.Notch = notch;
            if (forceInvokeEvent && notch == oldNotch) brakeHandle.InvokeNotchChanged();
        }

        public void SetReverser(ReverserPosition position, bool forceInvokeEvent)
        {
            Reverser reverser = (Reverser)Native.Handles.Reverser;
            ReverserPosition oldPosition = reverser.Position;
            reverser.Position = position;
            if (forceInvokeEvent && position == oldPosition) reverser.InvokePositionChanged();
        }

        public void KeyDown(NativeAtsKeyName key)
        {
            ((NativeKeySet)Native.NativeKeys).NotifyPressed(key);
        }

        public void KeyUp(NativeAtsKeyName key)
        {
            ((NativeKeySet)Native.NativeKeys).NotifyReleased(key);
        }

        public void HornBlow(HornType hornType)
        {
            Native.InvokeHornBlown(hornType);
        }

        public void DoorOpened()
        {
            Native.InvokeDoorOpened();
        }

        public void DoorClosed()
        {
            Native.InvokeDoorClosed();
        }

        public void BeaconPassed(BeaconPassedEventArgs args)
        {
            Native.InvokeBeaconPassed(args);
        }

        public void SetSignal(int signalIndex)
        {
            Native.InvokeSignalUpdated(signalIndex);
        }
    }
}
