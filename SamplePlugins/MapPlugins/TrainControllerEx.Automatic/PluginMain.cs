using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.Direct3D9;

using BveTypes.ClassWrappers;

using BveEx.Extensions.TrainDrawPatch;
using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.MapPlugins.TrainControllerEx.Automatic
{
    [Plugin(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private Train Train;
        private TrainDrawPatch Patch;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            Patch.Dispose();
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            if (!e.Scenario.Trains.ContainsKey("test_ex_2"))
            {
                throw new BveFileLoadException("キーが 'test_ex_2' の他列車が見つかりませんでした。", "TrainControllerEx.Automatic");
            }

            Train = e.Scenario.Trains["test_ex_2"];
            Patch = Extensions.GetExtension<ITrainDrawPatchFactory>().Patch(nameof(Automatic), Train, DrawTrain);
        }

        public override void Tick(TimeSpan elapsed)
        {
        }

        public void DrawTrain(Direct3DProvider direct3DProvider, Matrix viewMatrix)
        {
            foreach (Structure structure in Train.TrainInfo.Structures)
            {
                if (structure.Model is null) return;

                double angle = Native.VehicleState.Time.TotalSeconds;
                Matrix location = Matrix.RotationY((float)angle - (float)Math.PI / 2) * Matrix.Translation(15 + 8 * (float)Math.Sin(angle), -1, 162 + 8 * (float)Math.Cos(angle));
                Matrix originLocation = BveHacker.Scenario.Map.MyTrack.GetTransform(0, BveHacker.Scenario.VehicleLocation.BlockIndex * 25);
                
                direct3DProvider.Device.SetTransform(TransformState.World, location * originLocation * viewMatrix);
                structure.Model.Draw(direct3DProvider, false);
                structure.Model.Draw(direct3DProvider, true);
            }
        }
    }
}
