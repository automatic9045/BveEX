using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.DiagramUpdater
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IDiagramUpdater))]
    internal sealed class DiagramUpdater : AssemblyPluginBase, IDiagramUpdater
    {
        private readonly TimePosForm TimePosForm;

        public override string Title { get; } = nameof(DiagramUpdater);
        public override string Description { get; } = "時刻表、ダイヤグラムなどの行路に関わるオブジェクトの更新機能を提供します。";

        public DiagramUpdater(PluginBuilder builder) : base(builder)
        {
            TimePosForm = BveHacker.TimePosForm;
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed) => new ExtensionTickResult();

        public void UpdateDiagram(Scenario scenario)
        {
            if (scenario is null) throw new ArgumentNullException(nameof(scenario));

            StationList stations = scenario.Route.Stations;
            TimeTable timeTable = scenario.TimeTable;

            timeTable.NameTexts = new string[stations.Count + 1];
            timeTable.NameTextWidths = new int[stations.Count + 1];
            timeTable.ArrivalTimeTexts = new string[stations.Count + 1];
            timeTable.ArrivalTimeTextWidths = new int[stations.Count + 1];
            timeTable.DepartureTimeTexts = new string[stations.Count + 1];
            timeTable.DepartureTimeTextWidths = new int[stations.Count + 1];
            timeTable.Update();

            TimePosForm.SetScenario(scenario);
        }

        public void UpdateDiagram() => UpdateDiagram(BveHacker.Scenario);
    }
}
