using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using BveEx.Extensions.Native;
using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.Scripting;

namespace BveEx.Plugins.Scripting
{
    internal abstract class ScriptPluginBase : PluginBase, IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ScriptPluginBase>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Name { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> NoReturnValue { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();
        private static readonly string NameText;

        static ScriptPluginBase()
        {
#if DEBUG
            _ = Resources.Value;
#endif

            NameText = Resources.Value.Name.Value;
        }

        public override string Name { get; } = NameText;
        public override string Title { get; } = "";
        public override string Version { get; } = "";
        public override string Description { get; } = "";
        public override string Copyright { get; } = "";

        protected readonly Globals Globals;

        private readonly IPluginScript<Globals> DisposeScript;
        private readonly IPluginScript<ScenarioCreatedGlobals> OnScenarioCreatedScript;
        private readonly IPluginScript<StartedGlobals> OnStartedScript;
        private readonly IPluginScript<TickGlobals> TickScript;

        protected ScriptPluginBase(ScriptPluginBuilder builder, PluginType pluginType) : base(builder, new PluginAttribute(pluginType))
        {
            Title = builder.Title;
            Version = builder.Version;
            Description = builder.Description;
            Copyright = builder.Copyright;

            Globals = new Globals(BveHacker);

            DisposeScript = builder.DisposeScript?.GetWithCheckErrors();
            OnScenarioCreatedScript = builder.OnScenarioCreatedScript?.GetWithCheckErrors();
            OnStartedScript = builder.OnStartedScript?.GetWithCheckErrors();
            TickScript = builder.TickScript?.GetWithCheckErrors();

            IPluginScript<Globals> constructorScript = builder.ConstructorScript?.GetWithCheckErrors();
            constructorScript?.Run(Globals);

            BveHacker.ScenarioCreated += OnScenarioCreated;
            Extensions.GetExtension<INative>().Started += OnStarted;
        }

        public override void Dispose() => DisposeScript?.Run(Globals);

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            ScenarioCreatedGlobals globals = new ScenarioCreatedGlobals(Globals, e);
            OnScenarioCreatedScript?.Run(globals);
        }

        private void OnStarted(object sender, StartedEventArgs e)
        {
            StartedGlobals globals = new StartedGlobals(Globals, e);
            OnStartedScript?.Run(globals);
        }

        public override void Tick(TimeSpan elapsed)
        {
            if (TickScript is null) return;

            TickGlobals globals = new TickGlobals(Globals, elapsed);
            TickScript.Run(globals);
        }
    }
}
