using AtsEx.PluginHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Obsolete]
    internal class PassedGlobals : ExtendedBeaconGlobalsBase<PassedEventArgs>
    {
        public PassedGlobals(INative native, PluginHost.IBveHacker bveHacker,
            ExtendedBeaconBase<PassedEventArgs> sender, PassedEventArgs eventArgs)
            : base(native, bveHacker, sender, eventArgs)
        {
        }

        internal override PassedEventArgs GetEventArgsWithScriptVariables() => new PassedEventArgs(e, Variables);
    }
}
