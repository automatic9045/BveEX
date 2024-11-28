using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;
using BveEx.Scripting;

namespace BveEx.Plugins.Scripting
{
    internal sealed class ScriptPluginBuilder : PluginBuilder
    {
        public string Location { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }

        public IPluginScript<Globals> ConstructorScript { get; set; }
        public IPluginScript<Globals> DisposeScript { get; set; }
        public IPluginScript<ScenarioCreatedGlobals> OnScenarioCreatedScript { get; set; }
        public IPluginScript<StartedGlobals> OnStartedScript { get; set; }
        public IPluginScript<TickGlobals> TickScript { get; set; }

        public ScriptPluginBuilder(PluginBuilder source) : base(source)
        {
        }
    }
}
