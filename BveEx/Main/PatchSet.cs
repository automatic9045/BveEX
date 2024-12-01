using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace BveEx
{
    internal class PatchSet : IDisposable
    {
        public readonly HarmonyPatch LoadScenarioPatch;
        public readonly HarmonyPatch DisposeScenarioPatch;

        public readonly HarmonyPatch OnLoadPatch;
        public readonly HarmonyPatch OnInitializePatch;
        public readonly HarmonyPatch OnElapsePatch;

        public PatchSet(ClassMemberSet mainFormMembers, ClassMemberSet scenarioMembers, ClassMemberSet atsPluginMembers)
        {
            LoadScenarioPatch = HarmonyPatch.Patch(nameof(BveEx), mainFormMembers.GetSourceMethodOf(nameof(MainForm.LoadScenario)).Source, PatchType.Prefix);
            DisposeScenarioPatch = HarmonyPatch.Patch(nameof(BveEx), scenarioMembers.GetSourceMethodOf(nameof(Scenario.Dispose)).Source, PatchType.Prefix);

            OnLoadPatch = HarmonyPatch.Patch(nameof(BveEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.LoadLibrary)).Source, PatchType.Prefix);
            OnInitializePatch = HarmonyPatch.Patch(nameof(BveEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnInitialize)).Source, PatchType.Prefix);
            OnElapsePatch = HarmonyPatch.Patch(nameof(BveEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnElapse)).Source, PatchType.Prefix);
        }

        public void Dispose()
        {
            LoadScenarioPatch.Dispose();
            DisposeScenarioPatch.Dispose();

            OnLoadPatch.Dispose();
            OnInitializePatch.Dispose();
            OnElapsePatch.Dispose();
        }
    }
}
