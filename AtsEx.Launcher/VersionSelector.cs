using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    public class VersionSelector
    {
        public class AsAtsPlugin
        {
            public CoreHost CoreHost { get; }

            public AsAtsPlugin(Assembly callerAssembly) : base()
            {
                CoreHost = new CoreHost(callerAssembly);
            }
        }
    }
}
