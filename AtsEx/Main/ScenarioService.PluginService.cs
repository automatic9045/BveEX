using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Handles;
using AtsEx.Plugins;
using AtsEx.PluginHost.Handles;
using AtsEx.PluginHost.Plugins;

namespace AtsEx
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

            public TickCommandBuilder Tick(TimeSpan elapsed)
            {
                TickCommandBuilder commandBuilder = new TickCommandBuilder(Handles);

                foreach (PluginBase plugin in Plugins[PluginType.VehiclePlugin].Values)
                {
                    TickResult tickResult = plugin.Tick(elapsed);
                    if (!(tickResult is VehiclePluginTickResult vehiclePluginTickResult))
                    {
                        throw new InvalidOperationException(string.Format(Resources.Value.VehiclePluginTickResultTypeInvalid.Value,
                           $"{nameof(PluginBase)}.{nameof(PluginBase.Tick)}", nameof(VehiclePluginTickResult)));
                    }

                    commandBuilder.Override(vehiclePluginTickResult);
                }

                foreach (PluginBase plugin in Plugins[PluginType.MapPlugin].Values)
                {
                    TickResult tickResult = plugin.Tick(elapsed);
                    if (!(tickResult is MapPluginTickResult mapPluginTickResult))
                    {
                        throw new InvalidOperationException(string.Format(Resources.Value.MapPluginTickResultTypeInvalid.Value,
                           $"{nameof(PluginBase)}.{nameof(PluginBase.Tick)}", nameof(MapPluginTickResult)));
                    }

                    commandBuilder.Override(mapPluginTickResult);
                }

                return commandBuilder;
            }
        }
    }
}
