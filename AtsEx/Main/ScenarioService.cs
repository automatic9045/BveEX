using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using AtsEx.Handles;
using AtsEx.Input;
using AtsEx.MapStatements;
using AtsEx.Panels;
using AtsEx.Plugins;
using AtsEx.Sound;
using AtsEx.PluginHost.Input.Native;
using AtsEx.PluginHost.Native;

namespace AtsEx
{
    internal abstract partial class ScenarioService : IDisposable
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
        private readonly BveHacker BveHacker;

        private readonly PluginService _PluginService;

        protected ScenarioService(AtsEx atsEx, PluginSourceSet vehiclePluginUsing, VehicleConfig vehicleConfig, VehicleSpec vehicleSpec)
        {
            AtsEx = atsEx;
            BveHacker = AtsEx.BveHacker;

            Native = new NativeImpl(vehicleSpec, vehicleConfig);

            PluginLoader pluginLoader = new PluginLoader(Native, BveHacker, AtsEx.Extensions);
            PluginSet plugins = pluginLoader.Load(vehiclePluginUsing);
            _PluginService = new PluginService(plugins, Native.Handles);

            HeaderErrorDynamicResolver headerErrorResolver = new HeaderErrorDynamicResolver(BveHacker.LoadErrorManager, BveHacker.MapHeaders);
            headerErrorResolver.Resolve();

            AtsEx.VersionFormProvider.SetScenario(plugins.Select(item => item.Value));
        }

        public virtual void Dispose()
        {
            AtsEx.VersionFormProvider.UnsetScenario();
            _PluginService.Dispose();
        }

        public void Started(BrakePosition defaultBrakePosition)
        {
            Native.InvokeStarted(defaultBrakePosition);
        }

        public TickCommandBuilder Tick(TimeSpan elapsed, VehicleState vehicleState, IList<int> panel, IList<int> sound)
        {
            Native.VehicleState = vehicleState;

            (Native.AtsPanelValues as AtsPanelValueSet).PreTick(panel);

            BveHacker.Tick(elapsed);
            TickCommandBuilder tickResult = _PluginService.Tick(elapsed);

            (Native.AtsPanelValues as AtsPanelValueSet).Tick(panel);
            (Native.AtsSounds as AtsSoundSet).Tick(sound);

            return tickResult;
        }

        public void SetPower(int notch)
        {
            (Native.Handles.Power as PowerHandle).Notch = notch;
        }

        public void SetBrake(int notch)
        {
            (Native.Handles.Brake as BrakeHandle).Notch = notch;
        }

        public void SetReverser(ReverserPosition position)
        {
            (Native.Handles.Reverser as Reverser).Position = position;
        }

        public void KeyDown(NativeAtsKeyName key)
        {
            ((NativeKeySet)Native.NativeKeys).NotifyPressed(key);
        }

        public void KeyUp(NativeAtsKeyName key)
        {
            ((NativeKeySet)Native.NativeKeys).NotifyReleased(key);
        }

        public void DoorOpened(DoorEventArgs args)
        {
            Native.InvokeDoorOpened(args);
        }

        public void DoorClosed(DoorEventArgs args)
        {
            Native.InvokeDoorClosed(args);
        }

        public void BeaconPassed(BeaconPassedEventArgs args)
        {
            Native.InvokeBeaconPassed(args);
        }
    }
}
