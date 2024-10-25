using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;
using BveEx.Scripting;

namespace BveEx.Plugins.Scripting
{
    public class ScenarioCreatedGlobals : Globals
    {
#pragma warning disable IDE1006 // 命名スタイル
        public readonly ScenarioCreatedEventArgs e;
#pragma warning restore IDE1006 // 命名スタイル

        public ScenarioCreatedGlobals(Globals source, ScenarioCreatedEventArgs e) : base(source)
        {
            this.e = e;
        }
    }
}
