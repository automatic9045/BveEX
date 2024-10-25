using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Plugins.Scripting
{
    internal partial class ScriptPluginPackage
    {
        internal class ScriptsBuilder
        {
            public string ConstructorScriptPath { get; set; }
            public string DisposeScriptPath { get; set; }
            public string OnScenarioCreatedScriptPath { get; set; }
            public string OnStartedScriptPath { get; set; }
            public string TickScriptPath { get; set; }
        }
    }
}
