using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Plugins;

namespace AtsEx.Samples.VehiclePlugins.CooperatingWithMapPlugin
{
    [Plugin(PluginType.VehiclePlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        public int SharedValue { get; } = 123;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed) => new VehiclePluginTickResult();
    }
}
