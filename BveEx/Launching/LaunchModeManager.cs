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
            string legacyFilePath = Path.Combine(Path.GetDirectoryName(App.Instance.BveExLauncherAssembly.Location), ".LEGACY");
            File.Create(legacyFilePath).Close();
            File.SetAttributes(legacyFilePath, FileAttributes.Hidden);

            Process.Start(App.Instance.BveAssembly.Location, scenarioPath is null ? string.Empty : $"\"{scenarioPath}\"");
            Environment.Exit(0);
        }
    }
}
