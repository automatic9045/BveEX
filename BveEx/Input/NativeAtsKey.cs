using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Input;

namespace AtsEx.Input
{
    internal sealed class NativeAtsKey : KeyBase
    {
        public NativeAtsKey() : base()
        {
        }

        internal new void NotifyPressed() => base.NotifyPressed();
        internal new void NotifyReleased() => base.NotifyReleased();
    }
}
