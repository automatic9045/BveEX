using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Extensions.MapStatements;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.MapPlugins.MapStatementTest
{
    [Plugin(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        public PluginMain(PluginBuilder builder) : base(builder)
        {
            Statements.Load(Extensions.GetExtension<IStatementSet>());
        }

        public override void Dispose()
        {
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
