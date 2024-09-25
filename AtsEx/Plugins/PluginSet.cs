using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Plugins
{
    internal class PluginSet : IPluginSet
    {
        private Dictionary<PluginType, ReadOnlyDictionary<string, PluginBase>> Items = null;
        public ReadOnlyDictionary<string, PluginBase> this[PluginType pluginType] => Items is null ? throw new MemberNotInitializedException() : Items[pluginType];

        public event EventHandler AllPluginsLoaded;

        public PluginSet()
        {
        }

        public void SetPlugins(IDictionary<string, PluginBase> vehiclePlugins, IDictionary<string, PluginBase> mapPlugins)
        {
            if (!(Items is null)) throw new InvalidOperationException();

            Items = new Dictionary<PluginType, ReadOnlyDictionary<string, PluginBase>>()
            {
                [PluginType.VehiclePlugin] = new ReadOnlyDictionary<string, PluginBase>(vehiclePlugins),
                [PluginType.MapPlugin] = new ReadOnlyDictionary<string, PluginBase>(mapPlugins),
            };

            AllPluginsLoaded?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<KeyValuePair<string, PluginBase>> GetEnumerator()
            => Items is null ? throw new MemberNotInitializedException() : Items.Values.SelectMany(x => x).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
