using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Native
{
    public class CallerInfo
    {
        public Process Process { get; }
        public AppDomain AppDomain { get; }
        public Assembly BveAssembly { get; }
        public Assembly AtsExCallerAssembly { get; }
        public Assembly AtsExLauncherAssembly { get; }

        public CallerInfo(Process targetProcess, AppDomain targetAppDomain, Assembly targetAssembly, Assembly atsExCallerAssembly, Assembly atsExLauncherAssembly)
        {
            Process = targetProcess;
            AppDomain = targetAppDomain;
            BveAssembly = targetAssembly;
            AtsExCallerAssembly = atsExCallerAssembly;
            AtsExLauncherAssembly = atsExLauncherAssembly;
        }
    }
}
