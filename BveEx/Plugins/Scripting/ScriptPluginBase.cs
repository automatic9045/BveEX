using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using BveEx.PluginHost;
using BveEx.PluginHost.Handles;
using BveEx.PluginHost.Native;
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

        public override string Location { get; } = "";
        public override string Name { get; } = NameText;
        public override string Title { get; } = "";
        public override string Version { get; } = "";
        public override string Description { get; } = "";
        public override string Copyright { get; } = "";

        protected readonly Globals Globals;

        private readonly IPluginScript<Globals> DisposeScript;
        private readonly IPluginScript<ScenarioCreatedGlobals> OnScenarioCreatedScript;
        private readonly IPluginScript<StartedGlobals> OnStartedScript;
        private readonly IPluginScript<TickResult, TickGlobals> TickScript;

        protected ScriptPluginBase(ScriptPluginBuilder builder, PluginType pluginType) : base(builder, new PluginAttribute(pluginType))
        {
            Location = builder.Location;
            Title = builder.Title;
            Version = builder.Version;
            Description = builder.Description;
            Copyright = builder.Copyright;

            Globals = new Globals(Native, BveHacker);

            DisposeScript = builder.DisposeScript?.GetWithCheckErrors();
            OnScenarioCreatedScript = builder.OnScenarioCreatedScript?.GetWithCheckErrors();
            OnStartedScript = builder.OnStartedScript?.GetWithCheckErrors();
            TickScript = builder.TickScript?.GetWithCheckErrors();

            IPluginScript<Globals> constructorScript = builder.ConstructorScript?.GetWithCheckErrors();
            constructorScript?.Run(Globals);

            BveHacker.ScenarioCreated += OnScenarioCreated;
            Native.Started += OnStarted;
        }

        public override void Dispose() => DisposeScript?.Run(Globals);

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            ScenarioCreatedGlobals globals = new ScenarioCreatedGlobals(Globals, e);
            OnScenarioCreatedScript?.Run(globals);
        }

        private void OnStarted(StartedEventArgs e)
        {
            StartedGlobals globals = new StartedGlobals(Globals, e);
            OnStartedScript?.Run(globals);
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            if (TickScript is null)
            {
                switch (PluginType)
                {
                    case PluginType.VehiclePlugin:
                        return new VehiclePluginTickResult();

                    case PluginType.MapPlugin:
                        return new MapPluginTickResult();
                }
            }

            TickGlobals globals = new TickGlobals(Globals, elapsed);
            IScriptResult<TickResult> result = TickScript.Run(globals) ?? throw new InvalidOperationException(string.Format(Resources.Value.NoReturnValue.Value, Title, nameof(Tick)));

            return result.ReturnValue;
        }
    }
}
