using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Process.Start(App.Instance.BveAssembly.Location, $"\"{scenarioPath ?? string.Empty}\" /bveex-legacy");
            Environment.Exit(0);
        }
    }
}
