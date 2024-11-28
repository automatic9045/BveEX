using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using BveEx.Input;
using BveEx.Panels;
using BveEx.Plugins;
using BveEx.Sound;
using BveEx.PluginHost.Input.Native;
using BveEx.PluginHost.Native;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost;

namespace BveEx
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


        private readonly BveEx BveEx;
        private readonly NativeImpl Native;

        private readonly PluginSet Plugins;

        public Scenario Target { get; private set; } = null;

        public ScenarioService(BveEx bveEx, PluginSourceSet vehiclePluginUsing, VehicleConfig vehicleConfig, VehicleSpec vehicleSpec)
        {
            BveEx = bveEx;
            BveEx.BveHacker.ScenarioCreated += OnScenarioCreated;

            Native = new NativeImpl(vehicleSpec, vehicleConfig);

            PluginLoader pluginLoader = new PluginLoader(Native, BveEx.BveHacker, BveEx.Extensions);
            Plugins = pluginLoader.Load(vehiclePluginUsing);

            BveEx.VersionFormProvider.SetScenario(Plugins[PluginType.VehiclePlugin].Values, Plugins[PluginType.MapPlugin].Values);
        }

        public virtual void Dispose()
        {
            BveEx.BveHacker.ScenarioCreated -= OnScenarioCreated;

            BveEx.VersionFormProvider.UnsetScenario();
            foreach (KeyValuePair<string, PluginBase> plugin in Plugins)
            {
                plugin.Value.Dispose();
            }
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
            BveEx.BveHacker.InvokePreviewTick();
        }

        public void PostTick()
        {
            BveEx.BveHacker.InvokePostTick();
        }

        public void Tick(TimeSpan elapsed, VehicleState vehicleState, IList<int> panel, IList<int> sound)
        {
            Native.VehicleState = vehicleState;
            (Native.AtsPanelValues as AtsPanelValueSet).PreTick(panel);

            foreach (PluginBase plugin in Plugins[PluginType.VehiclePlugin].Values)
            {
                plugin.Tick(elapsed);
            }

            foreach (PluginBase plugin in Plugins[PluginType.MapPlugin].Values)
            {
                plugin.Tick(elapsed);
            }

            (Native.AtsPanelValues as AtsPanelValueSet).Tick(panel);
            (Native.AtsSounds as AtsSoundSet).Tick(sound);
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
