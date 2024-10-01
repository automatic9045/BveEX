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
    internal class InitializerBeacon : Beacon
    {
        private bool IsFirstTime = true;

        public InitializerBeacon(INative native, IBveHacker bveHacker,
            IStatement definedStatement, Identifier beaconName, ObservingTargetTrack observingTargetTrack, ObservingTargetTrain observingTargetTrain,
            IPluginScript<ExtendedBeaconGlobalsBase<PassedEventArgs>> script)
            : base(native, bveHacker, definedStatement, beaconName, observingTargetTrack, observingTargetTrain, script)
        {
        }

        internal override void Tick(double currentLocation)
        {
            if (!IsFirstTime) return;

            NotifyPassed(Direction.Forward);
            IsFirstTime = false;
        }
    }
}
