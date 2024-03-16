using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Handles;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Sound;
using AtsEx.PluginHost.Sound.Native;

namespace AtsEx.Samples.VehiclePlugins.SimpleAts
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

        public override TickResult Tick(TimeSpan elapsed)
        {
            UserVehicleLocationManager locationManager = BveHacker.Scenario.LocationManager;
            PluginHost.Handles.HandleSet handleSet = Native.Handles;

            double speedMps = locationManager.SpeedMeterPerSecond;

            VehiclePluginTickResult tickResult = new VehiclePluginTickResult();
            if (speedMps > 100d.KmphToMps()) // 100km/h以上出ていたら常用最大ブレーキ
            {
                if (AtsSound.PlayState == PlayState.Stop) AtsSound.PlayLoop();

                int atsPowerNotch = 0;
                int atsBrakeNotch = handleSet.Brake.MaxServiceBrakeNotch;

                NotchCommandBase powerCommand = handleSet.Power.GetCommandToSetNotchTo(atsPowerNotch);
                NotchCommandBase brakeCommand = handleSet.Brake.GetCommandToSetNotchTo(Math.Max(atsBrakeNotch, handleSet.Brake.Notch));
                ReverserPositionCommandBase reverserCommand = ReverserPositionCommandBase.Continue;
                ConstantSpeedCommand? constantSpeedCommand = ConstantSpeedCommand.Disable;

                tickResult.HandleCommandSet = new HandleCommandSet(powerCommand, brakeCommand, reverserCommand, constantSpeedCommand);
            }
            else
            {
                if (AtsSound.PlayState == PlayState.PlayingLoop) AtsSound.Stop();
            }

            return tickResult;
        }
    }
}
