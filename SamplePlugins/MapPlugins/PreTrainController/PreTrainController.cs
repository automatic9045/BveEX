using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;

using AtsEx.Extensions.PreTrainPatch;

namespace AtsEx.Samples.MapPlugins.PreTrainController
{
    [Plugin(PluginType.MapPlugin)]
    public class PreTrainController : AssemblyPluginBase
    {
        private Train Train;
        private PreTrainPatch PreTrainPatch;

        public PreTrainController(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            PreTrainPatch?.Dispose();
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            if (!e.Scenario.Trains.ContainsKey("test"))
            {
                throw new BveFileLoadException("キーが 'test' の他列車が見つかりませんでした。", "PreTrainController");
            }

            Train = e.Scenario.Trains["test"];
            Train.TrainInfo.TrackKey = "0";
            Train.Location = 35;

            SectionManager sectionManager = e.Scenario.SectionManager;
            PreTrainPatch = Extensions.GetExtension<IPreTrainPatchFactory>().Patch(nameof(PreTrainPatch), sectionManager, new PreTrainLocationConverter(Train, sectionManager));
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            if (Train.Location < 15.1)
            {
                Train.Location = 15.1;
            }
            else if (1520.1 < Train.Location)
            {
                Train.Location = 1520.1;
            }

            return new MapPluginTickResult();
        }


        private class PreTrainLocationConverter : IPreTrainLocationConverter
        {
            private readonly Train SourceTrain;
            private readonly SectionManager SectionManager;

            public PreTrainLocationConverter(Train sourceTrain, SectionManager sectionManager)
            {
                SourceTrain = sourceTrain;
                SectionManager = sectionManager;
            }

            public PreTrainLocation Convert(PreTrainLocation source)
                => SourceTrain.TrainInfo.TrackKey == "0" ? PreTrainLocation.FromLocation(SourceTrain.Location, SectionManager) : source;
        }
    }
}
