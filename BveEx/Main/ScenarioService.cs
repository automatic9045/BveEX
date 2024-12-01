using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using BveEx.Extensions.MapStatements;
using BveEx.Plugins;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost;

namespace BveEx
{
    internal class ScenarioService : IDisposable
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
        private readonly PluginSet Plugins = new PluginSet();
        private readonly MapPluginFactory MapPluginFactory;

        private MapListener MapListener;

        public Scenario Target { get; private set; } = null;

        public ScenarioService(BveEx bveEx)
        {
            BveEx = bveEx;
            BveEx.BveHacker.ScenarioCreated += OnScenarioCreated;

            MapPluginFactory = new MapPluginFactory(BveEx.BveHacker, BveEx.Extensions, Plugins);

            IStatementSet statements = BveEx.Extensions.GetExtension<IStatementSet>();
            MapListener = MapListener.Listen(statements);

            MapListener.LoadRequested += (sender, e) =>
            {
                Dictionary<string, PluginBase> mapPlugins = MapPluginFactory.LoadPluginUsing(e.PluginUsingPath);

                Plugins.AddMapPlugins(mapPlugins);
                BveEx.VersionFormProvider.SetScenario(((IPluginSet)Plugins).VehiclePlugins.Values, ((IPluginSet)Plugins).MapPlugins.Values);
            };

            MapListener.LoadAssemblyRequested += (sender, e) =>
            {
                Dictionary<string, PluginBase> mapPlugins = MapPluginFactory.LoadAssembly(e.AssemblyPath, e.Identifier);

                Plugins.AddMapPlugins(mapPlugins);
                BveEx.VersionFormProvider.SetScenario(((IPluginSet)Plugins).VehiclePlugins.Values, ((IPluginSet)Plugins).MapPlugins.Values);
            };

            statements.LoadingCompleted += OnStatementLoadingCompleted;
        }

        public void Dispose()
        {
            BveEx.BveHacker.ScenarioCreated -= OnScenarioCreated;

            IStatementSet statements = BveEx.Extensions.GetExtension<IStatementSet>();
            statements.LoadingCompleted -= OnStatementLoadingCompleted;

            MapListener?.Dispose();

            BveEx.VersionFormProvider.UnsetScenario();
            foreach (KeyValuePair<string, PluginBase> plugin in Plugins)
            {
                plugin.Value.Dispose();
            }
        }

        private void OnStatementLoadingCompleted(object sender, EventArgs e)
        {
            Plugins.CompleteLoadingMapPlugins();

            MapListener.Dispose();
            MapListener = null;
        }

        public void LoadVehiclePlugins()
        {
            VehiclePluginFactory vehiclePluginFactory = new VehiclePluginFactory(BveEx.BveHacker, BveEx.Extensions, Plugins);
            string vehiclePath = BveEx.BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path;
            Dictionary<string, PluginBase> vehiclePlugins = vehiclePluginFactory.Load(vehiclePath);

            Plugins.AddVehiclePlugins(vehiclePlugins);
            BveEx.VersionFormProvider.SetScenario(((IPluginSet)Plugins).VehiclePlugins.Values, ((IPluginSet)Plugins).MapPlugins.Values);
            Plugins.CompleteLoadingVehiclePlugins();
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
            foreach (PluginBase plugin in ((IPluginSet)Plugins).VehiclePlugins.Values)
            {
                plugin.Tick(elapsed);
            }

            foreach (PluginBase plugin in ((IPluginSet)Plugins).MapPlugins.Values)
            {
                plugin.Tick(elapsed);
            }
        }
    }
}
