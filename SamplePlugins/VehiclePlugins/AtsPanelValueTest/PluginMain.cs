using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Extensions.Native;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.VehiclePlugins.AtsPanelValueTest
{
    [Plugin(PluginType.VehiclePlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private readonly INative Native;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            Native = Extensions.GetExtension<INative>();
            Native.Started += OnStarted;
        }

        public override void Dispose()
        {
            Native.Started -= OnStarted;
        }

        private void OnStarted(object sender, StartedEventArgs e)
        {
            Native.AtsPanelArray[1] = 50; // 初期値を持たせる例。シナリオ開始直後及び駅ジャンプ時に実行
        }

        public override void Tick(TimeSpan elapsed)
        {
            int timeMilliseconds = BveHacker.Scenario.TimeManager.TimeMilliseconds;

            Native.AtsPanelArray[0] = timeMilliseconds / 500 % 2 == 0 ? 1 : 0; // 0.5 秒おきに点滅
            Native.AtsPanelArray[1] += 1; // 1 ずつ増やす
            Native.AtsPanelArray[2] = (int)(Math.Sin(timeMilliseconds / 400d) * 100); // 波形に変動させる

            if (Native.AtsPanelArray[1] == 200) Native.AtsPanelArray[1] = 0;
        }
    }
}
