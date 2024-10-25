using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using BveTypes.ClassWrappers.Extensions;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.StructureModelFinder
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IStructureModelFinder))]
    internal class StructureModelFinder : AssemblyPluginBase, IStructureModelFinder
    {
        private WrappedSortedList<string, Model> KeyToModel;
        private Dictionary<Model, string> ModelToKey;

        public override string Title { get; } = nameof(StructureModelFinder);
        public override string Description { get; } = "ストラクチャーの 3D モデルを簡単に検索するための機能を提供します。";

        public StructureModelFinder(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            WrappedSortedList<string, Model> loadedModels = e.Scenario.Route.StructureModels;

            KeyToModel = loadedModels;
            ModelToKey = loadedModels.Where(item => !(item.Value is null)).ToDictionary(item => item.Value, item => item.Key);
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        public override TickResult Tick(TimeSpan elapsed) => new ExtensionTickResult();

        public Model GetModel(string structureKey) => KeyToModel[structureKey];
        public string GetStructureKey(Model model) => ModelToKey[model];
    }
}
