using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace AtsEx
{
    internal abstract partial class AtsEx
    {
        internal partial class AsInputDevice
        {
            private class PatchSet : IDisposable
            {
                public readonly HarmonyPatch LoadScenarioPatch;
                public readonly HarmonyPatch DisposeScenarioPatch;

                public readonly HarmonyPatch OnSetBeaconDataPatch;
                public readonly HarmonyPatch OnKeyDownPatch;
                public readonly HarmonyPatch OnKeyUpPatch;
                public readonly HarmonyPatch OnDoorStateChangedPatch;
                public readonly HarmonyPatch OnSetSignalPatch;
                public readonly HarmonyPatch OnSetReverserPatch;
                public readonly HarmonyPatch OnSetBrakePatch;
                public readonly HarmonyPatch OnSetPowerPatch;
                public readonly HarmonyPatch OnSetVehicleSpecPatch;
                public readonly HarmonyPatch OnInitializePatch;
                public readonly HarmonyPatch PreviewElapsePatch;
                public readonly HarmonyPatch PostElapsePatch;

                public PatchSet(ClassMemberSet mainFormMembers, ClassMemberSet scenarioMembers, ClassMemberSet atsPluginMembers)
                {
                    LoadScenarioPatch = HarmonyPatch.Patch(nameof(AtsEx), mainFormMembers.GetSourceMethodOf(nameof(MainForm.LoadScenario)).Source, PatchType.Prefix);
                    DisposeScenarioPatch = HarmonyPatch.Patch(nameof(AtsEx), scenarioMembers.GetSourceMethodOf(nameof(Scenario.Dispose)).Source, PatchType.Prefix);

                    OnSetBeaconDataPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetBeaconData)).Source, PatchType.Prefix);
                    OnKeyDownPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnKeyDown)).Source, PatchType.Prefix);
                    OnKeyUpPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnKeyUp)).Source, PatchType.Prefix);
                    OnDoorStateChangedPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnDoorStateChanged)).Source, PatchType.Prefix);
                    OnSetSignalPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetSignal)).Source, PatchType.Prefix);
                    OnSetReverserPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetReverser)).Source, PatchType.Prefix);
                    OnSetBrakePatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetBrake)).Source, PatchType.Prefix);
                    OnSetPowerPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetPower)).Source, PatchType.Prefix);
                    OnSetVehicleSpecPatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetVehicleSpec)).Source, PatchType.Postfix);
                    OnInitializePatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnInitialize)).Source, PatchType.Prefix);
                    PreviewElapsePatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnElapse)).Source, PatchType.Prefix);
                    PostElapsePatch = HarmonyPatch.Patch(nameof(AtsEx), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnElapse)).Source, PatchType.Postfix);
                }

                public void Dispose()
                {
                    LoadScenarioPatch.Dispose();
                    DisposeScenarioPatch.Dispose();

                    OnSetBeaconDataPatch.Dispose();
                    OnKeyDownPatch.Dispose();
                    OnKeyUpPatch.Dispose();
                    OnDoorStateChangedPatch.Dispose();
                    OnSetSignalPatch.Dispose();
                    OnSetReverserPatch.Dispose();
                    OnSetBrakePatch.Dispose();
                    OnSetPowerPatch.Dispose();
                    OnSetVehicleSpecPatch.Dispose();
                    OnInitializePatch.Dispose();
                    PreviewElapsePatch.Dispose();
                    PostElapsePatch.Dispose();
                }
            }
        }
    }
}
