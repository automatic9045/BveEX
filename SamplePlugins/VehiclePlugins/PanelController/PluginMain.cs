using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.VehiclePlugins.PanelController
{
    [Plugin(PluginType.VehiclePlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private readonly double[] Element4Subject = new double[1];
        private readonly VehiclePanelElement Element6;

        private Matrix Element5Location;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            string panelDirectory = Path.Combine(Path.GetDirectoryName(BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path), "Panel");
            Element6 = new VehiclePanelElement()
            {
                DaytimeImagePath = Path.Combine(panelDirectory, "control2.png"),
            };
            Element6.CreateModels();

            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            VehiclePanel panel = e.Scenario.Vehicle.Panel;
            panel.Elements[4].Subject = Element4Subject;
            Element5Location = panel.Elements[5].Matrix;
            panel.Elements.Add(Element6);
        }

        public override IPluginTickResult Tick(TimeSpan elapsed)
        {
            float amount = (float)Math.Sin(Native.VehicleState.Time.TotalSeconds * 2);

            VehiclePanel panel = BveHacker.Scenario.Vehicle.Panel;
            Element4Subject[0] = Native.VehicleState.Time.TotalMilliseconds / 10 % 200;
            panel.Elements[5].Matrix = Matrix.Scaling((amount + 2) / 3, (amount + 1.5f) / 2.5f, 0) * Element5Location * Matrix.Translation(amount * 100, 0, 0);
            Element6.Matrix = Matrix.Translation(200, 300 + amount * 50, 0);

            return new VehiclePluginTickResult();
        }
    }
}
