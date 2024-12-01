using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Plugins;

using BveEx.Extensions.Native;

namespace BveEx.Samples.VehiclePlugins.SimpleAts
{
    [Plugin(PluginType.VehiclePlugin)]
    public class SimpleAts : AssemblyPluginBase
    {
        private readonly INative Native;

        public SimpleAts(PluginBuilder builder) : base(builder)
        {
            Native = Extensions.GetExtension<INative>();
        }

        public override void Dispose()
        {
        }

        public override void Tick(TimeSpan elapsed)
        {
            double speedMps = BveHacker.Scenario.VehicleLocation.Speed;
            SoundPlayMode soundPlayMode = SoundPlayCommands.GetMode(Native.AtsSoundArray[0]);

            if (speedMps > 100 / 3.6) // 100 km/h 以上出ていたら常用最大ブレーキ
            {
                if (soundPlayMode == SoundPlayMode.Stop) Native.AtsSoundArray[0] = SoundPlayMode.PlayLooping.ToCommand();

                AtsPlugin atsPlugin = BveHacker.Scenario.Vehicle.Instruments.AtsPlugin;
                atsPlugin.AtsHandles.PowerNotch = 0;
                atsPlugin.AtsHandles.BrakeNotch = Math.Max(atsPlugin.AtsHandles.NotchInfo.EmergencyBrakeNotch - 1, atsPlugin.Handles.BrakeNotch);
                atsPlugin.AtsHandles.ConstantSpeedMode = ConstantSpeedMode.Disable;
            }
            else
            {
                if (soundPlayMode == SoundPlayMode.PlayLooping) Native.AtsSoundArray[0] = SoundPlayMode.Stop.ToCommand();
            }
        }
    }
}
