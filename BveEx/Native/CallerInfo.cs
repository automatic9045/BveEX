using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Native
{
    public class CallerInfo
    {
        public Process Process { get; }
        public AppDomain AppDomain { get; }
        public Assembly BveAssembly { get; }
        public Assembly CallerAssembly { get; }
        public Assembly LauncherAssembly { get; }

        public CallerInfo(Process targetProcess, AppDomain targetAppDomain, Assembly targetAssembly, Assembly callerAssembly, Assembly launcherAssembly)
        {
            Process = targetProcess;
            AppDomain = targetAppDomain;
            BveAssembly = targetAssembly;
            CallerAssembly = callerAssembly;
            LauncherAssembly = launcherAssembly;
        }
    }
}
