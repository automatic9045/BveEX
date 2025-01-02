using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.PluginHost;

namespace AtsEx.Launching
{
    internal static class LaunchModeManager
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(LaunchModeManager), "Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ConflictedMessage { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ConflictedApproach { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static LaunchModeManager()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public static void RestartAsNormalMode(string scenarioPath)
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Contains("/bveex-modechanged", StringComparer.OrdinalIgnoreCase))
            {
                ErrorDialog.Show(5, Resources.Value.ConflictedMessage.Value, Resources.Value.ConflictedApproach.Value);
                throw new InvalidOperationException(Resources.Value.ConflictedMessage.Value);
            }

            string legacyFilePath = Path.Combine(Path.GetDirectoryName(App.Instance.AtsExLauncherAssembly.Location), ".LEGACY");
            try
            {
                File.Delete(legacyFilePath);
            }
            catch { }

            Process.Start(App.Instance.BveAssembly.Location, scenarioPath is null ? string.Empty : $"\"{scenarioPath}\" /bveex-modechanged");
            Environment.Exit(0);
        }
    }
}
