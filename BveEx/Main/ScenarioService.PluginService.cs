using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Handles;
using BveEx.Plugins;
using BveEx.PluginHost.Handles;
using BveEx.PluginHost.Plugins;

namespace BveEx
{
    internal partial class ScenarioService
    {
        private class PluginService : IDisposable
        {
            private readonly PluginSet Plugins;
            private readonly HandleSet Handles;

            public PluginService(PluginSet plugins, HandleSet handles)
            {
                Plugins = plugins;
                Handles = handles;
            }

            public void Dispose()
            {
                foreach (KeyValuePair<string, PluginBase> plugin in Plugins)
                {
                    plugin.Value.Dispose();
                }
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
}
