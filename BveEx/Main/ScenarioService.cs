using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using BveEx.Plugins;
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

        private readonly PluginSet Plugins;

        public Scenario Target { get; private set; } = null;

        public ScenarioService(BveEx bveEx, PluginSourceSet vehiclePluginUsing, VehicleConfig vehicleConfig)
        {
            BveEx = bveEx;
            BveEx.BveHacker.ScenarioCreated += OnScenarioCreated;

            PluginLoader pluginLoader = new PluginLoader(BveEx.BveHacker, BveEx.Extensions);
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

        public void PreviewTick()
        {
            BveEx.BveHacker.InvokePreviewTick();
        }

        public void PostTick()
        {
            BveEx.BveHacker.InvokePostTick();
        }

        public void Tick(TimeSpan elapsed)
        {
            foreach (PluginBase plugin in Plugins[PluginType.VehiclePlugin].Values)
            {
                plugin.Tick(elapsed);
            }

            foreach (PluginBase plugin in Plugins[PluginType.MapPlugin].Values)
            {
                plugin.Tick(elapsed);
            }
        }
    }
}
