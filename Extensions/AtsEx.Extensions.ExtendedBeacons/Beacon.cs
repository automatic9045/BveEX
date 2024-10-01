using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;
using AtsEx.Scripting;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Obsolete]
    internal class Beacon : ExtendedBeaconBase<PassedEventArgs>
    {
        private double OldLocation = 0d;

        public Beacon(INative native, IBveHacker bveHacker,
            IStatement definedStatement, Identifier beaconName, ObservingTargetTrack observingTargetTrack, ObservingTargetTrain observingTargetTrain,
            IPluginScript<ExtendedBeaconGlobalsBase<PassedEventArgs>> script)
            : base(native, bveHacker, definedStatement, beaconName, observingTargetTrack, observingTargetTrain, script)
        {
        }

        internal virtual void Tick(double currentLocation)
        {
            if (OldLocation < Location && Location <= currentLocation)
            {
                NotifyPassed(Direction.Forward);
            }
            else if (Location < OldLocation && currentLocation <= Location)
            {
                NotifyPassed(Direction.Backward);
            }

            OldLocation = currentLocation;
        }

        protected void NotifyPassed(Direction direction)
        {
            PassedEventArgs eventArgs = new PassedEventArgs(direction);
            PassedGlobals globals = new PassedGlobals(Native, BveHacker, this, eventArgs);
            Script.Run(globals);
            base.NotifyPassed(globals.GetEventArgsWithScriptVariables());
        }
    }
}
