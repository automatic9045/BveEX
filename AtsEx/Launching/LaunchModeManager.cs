using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;

namespace AtsEx.Launching
{
    internal static class LaunchModeManager
    {
        public static void RestartAsNormalMode(string scenarioPath)
        {
            string legacyFilePath = Path.Combine(Path.GetDirectoryName(App.Instance.AtsExLauncherAssembly.Location), ".LEGACY");
            try
            {
                File.Delete(legacyFilePath);
            }
            catch { }

            Process.Start(App.Instance.BveAssembly.Location, scenarioPath is null ? string.Empty : $"\"{scenarioPath}\"");
            Environment.Exit(0);
        }
    }
}
