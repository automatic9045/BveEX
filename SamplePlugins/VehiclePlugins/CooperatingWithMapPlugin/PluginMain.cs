using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.VehiclePlugins.CooperatingWithMapPlugin
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
