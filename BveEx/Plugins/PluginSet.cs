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
        public IReadOnlyDictionary<string, PluginBase> MapPlugins { get; private set; } = null;
        public IReadOnlyDictionary<string, PluginBase> VehiclePlugins { get; private set; } = null;

        public event EventHandler AllPluginsLoaded;

        public PluginSet()
        {
        }

        public void SetPlugins(IReadOnlyDictionary<string, PluginBase> vehiclePlugins, IReadOnlyDictionary<string, PluginBase> mapPlugins)
        {
            if (!(VehiclePlugins is null)) throw new InvalidOperationException();
            if (!(MapPlugins is null)) throw new InvalidOperationException();

            VehiclePlugins = vehiclePlugins;
            MapPlugins = mapPlugins;

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
