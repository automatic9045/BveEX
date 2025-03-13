using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;

using BveEx.PluginHost;

namespace BveEx.Launching
{
    internal class LaunchModeManager
    {
        private readonly MainForm MainForm;
        private readonly ScenarioSelectionForm ScenarioSelectionForm;
        private readonly HarmonyPatch Patch;

        private bool SkipSaveScenarioSelectionSettings = false;

        public LaunchModeManager(MainForm mainForm, ScenarioSelectionForm scenarioSelectionForm, MethodInfo saveSettingsMethod)
        {
            MainForm = mainForm;
            ScenarioSelectionForm = scenarioSelectionForm;

            Patch = HarmonyPatch.Patch(null, saveSettingsMethod, PatchType.Prefix);
            Patch.Invoked += (sender, e) => SkipSaveScenarioSelectionSettings ? new PatchInvokationResult(SkipModes.SkipPatches | SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
        }

        public void RestartAsLegacyMode(string scenarioPath)
        {
            ScenarioSelectionForm.SaveSettings();
            SkipSaveScenarioSelectionSettings = true;

            MainForm.Preferences.SaveToXml();
            MainForm.Preferences = null;

            foreach (Form form in Application.OpenForms)
            {
                form.Hide();
            }

            Process.Start(App.Instance.BveAssembly.Location, $"{(scenarioPath is null ? string.Empty : $"\"{scenarioPath}\"")} /legacy");
            Environment.Exit(0);
        }
    }
}
