using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Obsolete]
    internal class TrainPassedGlobals : ExtendedBeaconGlobalsBase<TrainPassedEventArgs>
    {
        public TrainPassedGlobals(INative native, IBveHacker bveHacker, ExtendedBeaconBase<TrainPassedEventArgs> sender, TrainPassedEventArgs eventArgs)
            : base(native, bveHacker, sender, eventArgs)
        {
        }

        internal override TrainPassedEventArgs GetEventArgsWithScriptVariables() => new TrainPassedEventArgs(e, Variables);
    }
}
