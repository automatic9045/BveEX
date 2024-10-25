using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Panels.Native;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.VehiclePlugins.AtsPanelValueTest
{
    [Plugin(PluginType.VehiclePlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private readonly IAtsPanelValue<bool> Ats0;
        private readonly IAtsPanelValue<int> Ats1;
        private readonly IAtsPanelValue<double> Ats2;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            Ats0 = Native.AtsPanelValues.RegisterBoolean(0); // bool 型の例
            Ats1 = Native.AtsPanelValues.RegisterInt32(1, 50); // int 型・初期値を持たせる例
            Ats2 = Native.AtsPanelValues.Register<double>(2, x => (int)x); // bool 型、int 型以外の状態量を手動で定義する例
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            int timeMilliseconds = (int)Native.VehicleState.Time.TotalMilliseconds;

            Ats0.Value = timeMilliseconds / 500 % 2 == 0; // 0.5 秒おきに点滅
            Ats1.Value += 1; // 1 ずつ増やす
            Ats2.Value = Math.Sin(timeMilliseconds / 400d) * 100; // 波形に変動させる

            if (Ats1.Value == 200) Ats1.Value = 0;

            return new VehiclePluginTickResult();
        }
    }
}
