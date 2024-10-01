using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Obsolete]
    public interface IExtensionMain : IExtension
    {
        IExtendedBeaconSet ExtendedBeacons { get; }
    }
}
