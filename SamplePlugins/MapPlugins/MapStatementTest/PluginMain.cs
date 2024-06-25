using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AtsEx.Extensions.MapStatements;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Samples.MapPlugins.MapStatementTest
{
    [Plugin(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        public PluginMain(PluginBuilder builder) : base(builder)
        {
            Headers.Load(BveHacker.MapHeaders);
            Statements.Load(Extensions.GetExtension<IStatementSet>());
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed) => new MapPluginTickResult();
    }
}
