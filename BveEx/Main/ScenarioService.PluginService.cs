using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Plugins;
using BveEx.PluginHost.Plugins;

namespace BveEx
{
    internal partial class ScenarioService
    {
        private class PluginService : IDisposable
        {
            private readonly PluginSet Plugins;

            public PluginService(PluginSet plugins)
            {
                Plugins = plugins;
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
