using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.Handles;
using AtsEx.PluginHost.Handles;
using AtsEx.PluginHost.Plugins;

namespace AtsEx
{
    internal sealed class TickCommandBuilder
    {
        private readonly int PowerNotch;
        private readonly int BrakeNotch;
        private readonly ReverserPosition ReverserPosition;

        private int? AtsPowerNotch = null;
        private int? AtsBrakeNotch = null;
        private ReverserPosition? AtsReverserPosition = null;
        private ConstantSpeedCommand? AtsConstantSpeedCommand = null;

        public TickCommandBuilder(PluginHost.Handles.HandleSet handles)
        {
            PowerNotch = handles.Power.Notch;
            BrakeNotch = handles.Brake.Notch;
            ReverserPosition = handles.Reverser.Position;
        }

        public void Override(VehiclePluginTickResult tickResult)
        {
            HandleCommandSet commandSet = tickResult.HandleCommandSet;

            if (AtsPowerNotch is null) AtsPowerNotch = commandSet.PowerCommand.GetOverridenNotch(PowerNotch);
            if (AtsBrakeNotch is null) AtsBrakeNotch = commandSet.BrakeCommand.GetOverridenNotch(BrakeNotch);
            if (AtsReverserPosition is null) AtsReverserPosition = commandSet.ReverserCommand.GetOverridenPosition(ReverserPosition);
            if (AtsConstantSpeedCommand is null) AtsConstantSpeedCommand = commandSet.ConstantSpeedCommand;
        }

        public void Override(MapPluginTickResult tickResult)
        {
        }

        public HandlePositionSet Compile()
            => new HandlePositionSet(
                AtsPowerNotch ?? PowerNotch,
                AtsBrakeNotch ?? BrakeNotch,
                AtsReverserPosition ?? ReverserPosition,
                AtsConstantSpeedCommand ?? ConstantSpeedCommand.Continue);
    }
}
