using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Plugins
{
    internal class PluginSet : IPluginSet
    {
        private readonly Dictionary<string, PluginBase> MapPlugins = new Dictionary<string, PluginBase>();
        private readonly Dictionary<string, PluginBase> VehiclePlugins = new Dictionary<string, PluginBase>();

        IReadOnlyDictionary<string, PluginBase> IPluginSet.MapPlugins => MapPlugins;
        IReadOnlyDictionary<string, PluginBase> IPluginSet.VehiclePlugins => VehiclePlugins;

        public bool AreMapPluginsLoaded { get; private set; } = false;
        public bool AreVehiclePluginsLoaded { get; private set; } = false;

        public event EventHandler MapPluginsLoaded;
        public event EventHandler VehiclePluginsLoaded;
        public event EventHandler AllPluginsLoaded;

        public PluginSet()
        {
        }

        public void AddMapPlugins(IReadOnlyDictionary<string, PluginBase> plugins)
        {
            if (AreMapPluginsLoaded) throw new InvalidOperationException();
            if (AreVehiclePluginsLoaded) throw new InvalidOperationException();

            foreach (KeyValuePair<string, PluginBase> plugin in plugins)
            {
                MapPlugins.Add(plugin.Key, plugin.Value);
            }
        }

        public void CompleteLoadingMapPlugins()
        {
            AreMapPluginsLoaded = true;
            MapPluginsLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void AddVehiclePlugins(IReadOnlyDictionary<string, PluginBase> plugins)
        {
            if (!AreMapPluginsLoaded) throw new InvalidOperationException();
            if (AreVehiclePluginsLoaded) throw new InvalidOperationException();

            foreach (KeyValuePair<string, PluginBase> plugin in plugins)
            {
                VehiclePlugins.Add(plugin.Key, plugin.Value);
            }
        }

        public void CompleteLoadingVehiclePlugins()
        {
            AreVehiclePluginsLoaded = true;
            VehiclePluginsLoaded?.Invoke(this, EventArgs.Empty);
            AllPluginsLoaded?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<KeyValuePair<string, PluginBase>> GetEnumerator()
        {
            IEnumerable<KeyValuePair<string, PluginBase>> vehiclePlugins = VehiclePlugins ?? Enumerable.Empty<KeyValuePair<string, PluginBase>>();
            IEnumerable<KeyValuePair<string, PluginBase>> mapPlugins = MapPlugins ?? Enumerable.Empty<KeyValuePair<string, PluginBase>>();

            return vehiclePlugins.Concat(mapPlugins).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
