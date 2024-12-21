using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Extensions.Native;
using BveEx.PluginHost;
using BveEx.PluginHost.Input;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.MapPlugins.TrainController
{
    [Plugin(PluginType.MapPlugin)]
    public class TrainController : AssemblyPluginBase
    {
        private Train Train;
        private double Speed = 25 / 3.6; // 25[km/h] = (25 / 3.6)[m/s]

        public TrainController(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;

            INative native = Extensions.GetExtension<INative>();
            native.Opened += OnNativeOpened;
        }

        private void OnNativeOpened(object sender, EventArgs e)
        {
            INative native = Extensions.GetExtension<INative>();

            native.AtsKeys.GetKey(AtsKeyName.D).Pressed += (sender2, e2) =>
            {
                Train.TrainInfo.TrackKey = "1";
            };

            native.AtsKeys.GetKey(AtsKeyName.E).Pressed += (sender2, e2) =>
            {
                Train.TrainInfo.TrackKey = "0";
            };
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            if (!e.Scenario.Trains.ContainsKey("test"))
            {
                throw new BveFileLoadException("キーが 'test' の他列車が見つかりませんでした。", "TrainController");
            }

            Train = e.Scenario.Trains["test"];
        }

        public override void Tick(TimeSpan elapsed)
        {
            INative native = Extensions.GetExtension<INative>();
            if (native.AtsKeys.GetKey(AtsKeyName.F).IsPressed) Speed -= 10.0 * elapsed.Ticks / TimeSpan.TicksPerMillisecond / 1000;
            if (native.AtsKeys.GetKey(AtsKeyName.G).IsPressed) Speed += 10.0 * elapsed.Ticks / TimeSpan.TicksPerMillisecond / 1000;

            if (Speed > 0)
            {
                Speed -= 2.0 * elapsed.Ticks / TimeSpan.TicksPerMillisecond / 1000;
                if (Speed < 0) Speed = 0d;
            }
            else if (Speed < 0)
            {
                Speed += 2.0 * elapsed.Ticks / TimeSpan.TicksPerMillisecond / 1000;
                if (Speed > 0) Speed = 0d;
            }

            Train.Location += Speed * elapsed.Ticks / TimeSpan.TicksPerMillisecond / 1000;
            Train.Speed = Speed;
        }
    }
}
