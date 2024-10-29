using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Handles;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Sound;
using BveEx.PluginHost.Sound.Native;

namespace BveEx.Samples.VehiclePlugins.SimpleAts
{
    [Plugin(PluginType.VehiclePlugin)]
    public class SimpleAts : AssemblyPluginBase
    {
        private readonly IAtsSound AtsSound;

        public SimpleAts(PluginBuilder builder) : base(builder)
        {
            AtsSound = Native.AtsSounds.Register(0);
        }

        public override void Dispose()
        {
        }

        public override IPluginTickResult Tick(TimeSpan elapsed)
        {
            VehicleLocation location = BveHacker.Scenario.VehicleLocation;
            PluginHost.Handles.HandleSet handleSet = Native.Handles;

            double speedMps = location.Speed;

            VehiclePluginTickResult tickResult = new VehiclePluginTickResult();
            if (speedMps > 100d.KmphToMps()) // 100km/h以上出ていたら常用最大ブレーキ
            {
                if (AtsSound.PlayState == PlayState.Stop) AtsSound.PlayLoop();

                tickResult.HandleCommandSet = new HandleCommandSet()
                {
                    PowerNotch = 0,
                    BrakeNotch = Math.Max(handleSet.Brake.MaxServiceBrakeNotch, handleSet.Brake.Notch),
                    ConstantSpeedMode = ConstantSpeedMode.Disable,
                };
            }
            else
            {
                if (AtsSound.PlayState == PlayState.PlayingLoop) AtsSound.Stop();
            }

            return tickResult;
        }
    }
}
