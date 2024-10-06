using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    public partial class VersionSelector
    {
        [Obsolete("ver1.0-RC10 以前の AtsEX Caller Input Device 向けです。")]
        public class AsInputDevice : VersionSelector
        {
            public AsInputDevice(Assembly callerAssembly) : base(callerAssembly)
            {
            }
        }
    }
}
