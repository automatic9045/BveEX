using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;

namespace BveEx.Launching
{
    internal static class LaunchModeManager
    {
        public static void RestartAsLegacyMode(string scenarioPath)
        {

            Process.Start(App.Instance.BveAssembly.Location, $"{(scenarioPath is null ? string.Empty : $"\"{scenarioPath}\"")} /legacy");
            Environment.Exit(0);
        }
    }
}
