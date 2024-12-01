using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace BveEx.Extensions.Native
{
    internal class PatchSet : IDisposable
    {
        public readonly HarmonyPatch ConstructorPatch;
        public readonly HarmonyPatch OnSetBeaconDataPatch;
        public readonly HarmonyPatch OnKeyDownPatch;
        public readonly HarmonyPatch OnKeyUpPatch;
        public readonly HarmonyPatch OnDoorStateChangedPatch;
        public readonly HarmonyPatch OnSetSignalPatch;
        public readonly HarmonyPatch OnSetVehicleSpecPatch;
        public readonly HarmonyPatch OnInitializePatch;

        public PatchSet(ClassMemberSet atsPluginMembers)
        {
            ConstructorPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceConstructor().Source, PatchType.Postfix);
            OnSetBeaconDataPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetBeaconData)).Source, PatchType.Prefix);
            OnKeyDownPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnKeyDown)).Source, PatchType.Prefix);
            OnKeyUpPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnKeyUp)).Source, PatchType.Prefix);
            OnDoorStateChangedPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnDoorStateChanged)).Source, PatchType.Prefix);
            OnSetSignalPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetSignal)).Source, PatchType.Prefix);
            OnSetVehicleSpecPatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnSetVehicleSpec)).Source, PatchType.Postfix);
            OnInitializePatch = HarmonyPatch.Patch(nameof(Native), atsPluginMembers.GetSourceMethodOf(nameof(AtsPlugin.OnInitialize)).Source, PatchType.Prefix);
        }

        public void Dispose()
        {
            ConstructorPatch.Dispose();
            OnSetBeaconDataPatch.Dispose();
            OnKeyDownPatch.Dispose();
            OnKeyUpPatch.Dispose();
            OnDoorStateChangedPatch.Dispose();
            OnSetSignalPatch.Dispose();
            OnSetVehicleSpecPatch.Dispose();
            OnInitializePatch.Dispose();
        }
    }
}
