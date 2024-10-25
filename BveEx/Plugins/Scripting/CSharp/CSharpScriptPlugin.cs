using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Plugins;
using AtsEx.Scripting;
using AtsEx.Scripting.CSharp;

namespace AtsEx.Plugins.Scripting.CSharp
{
    internal sealed class CSharpScriptPlugin : ScriptPluginBase
    {
        private CSharpScriptPlugin(ScriptPluginBuilder builder, PluginType pluginType) : base(builder, pluginType)
        {
        }

        public static CSharpScriptPlugin FromPackage(PluginBuilder builder, PluginType pluginType, ScriptPluginPackage package)
        {
            ScriptPluginBuilder newBuilder = new ScriptPluginBuilder(builder)
            {
                Location = package.Location,
                Title = package.Title,
                Version = package.Version,
                Description = package.Description,
                Copyright = package.Copyright,

                ConstructorScript = package.ConstructorScriptPath is null ?             null : PluginScript<Globals>.LoadFrom(package.ConstructorScriptPath),
                DisposeScript = package.DisposeScriptPath is null ?                     null : PluginScript<Globals>.LoadFrom(package.DisposeScriptPath),
                OnScenarioCreatedScript = package.OnScenarioCreatedScriptPath is null ? null : PluginScript<ScenarioCreatedGlobals>.LoadFrom(package.OnScenarioCreatedScriptPath),
                OnStartedScript = package.OnStartedScriptPath is null ?                 null : PluginScript<StartedGlobals>.LoadFrom(package.OnStartedScriptPath),
                TickScript = package.TickScriptPath is null ?                           null : PluginScript<TickResult, TickGlobals>.LoadFrom(package.TickScriptPath),
            };

            return new CSharpScriptPlugin(newBuilder, pluginType);
        }
    }
}
