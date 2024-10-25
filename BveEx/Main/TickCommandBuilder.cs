using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Handles;
using BveEx.PluginHost.Handles;
using BveEx.PluginHost.Plugins;

namespace BveEx
{
    internal sealed class TickCommandBuilder
    {
        private readonly int CabPowerNotch;
        private readonly int CabBrakeNotch;
        private readonly ReverserPosition CabReverserPosition;

        private int? AtsPowerNotch = null;
        private int? AtsBrakeNotch = null;
        private ReverserPosition? AtsReverserPosition = null;
        private ConstantSpeedMode? AtsConstantSpeedMode = null;

        public HandlePositionSet LatestHandlePositionSet { get; private set; }

        public TickCommandBuilder(PluginHost.Handles.HandleSet handles)
        {
            CabPowerNotch = handles.Power.Notch;
            CabBrakeNotch = handles.Brake.Notch;
            CabReverserPosition = handles.Reverser.Position;

            LatestHandlePositionSet = new HandlePositionSet(CabPowerNotch, CabBrakeNotch, CabReverserPosition, ConstantSpeedMode.Continue);
        }

        public void Override(VehiclePluginTickResult tickResult)
        {
            HandleCommandSet commandSet = tickResult.HandleCommandSet;

            AtsPowerNotch = commandSet.PowerCommand.GetOverridenNotch(AtsPowerNotch ?? CabPowerNotch) ?? AtsPowerNotch;
            AtsBrakeNotch = commandSet.BrakeCommand.GetOverridenNotch(AtsBrakeNotch ?? CabBrakeNotch) ?? AtsBrakeNotch;
            AtsReverserPosition = commandSet.ReverserCommand.GetOverridenPosition(AtsReverserPosition ?? CabReverserPosition) ?? AtsReverserPosition;
            AtsConstantSpeedMode = commandSet.ConstantSpeedCommand?.ToConstantSpeedMode() ?? AtsConstantSpeedMode;

            LatestHandlePositionSet = new HandlePositionSet(
                AtsPowerNotch ?? CabPowerNotch,
                AtsBrakeNotch ?? CabBrakeNotch,
                AtsReverserPosition ?? CabReverserPosition,
                AtsConstantSpeedMode ?? ConstantSpeedMode.Continue);
        }

        public void Override(MapPluginTickResult tickResult)
        {
        }
    }
}
