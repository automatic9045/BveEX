using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace BveEx.Extensions.ConductorPatch
{
    internal class HarmonyPatchSet : IDisposable
    {
        public HarmonyPatch Constructor { get; }
        public HarmonyPatch OnJumped { get; }
        public HarmonyPatch OnDoorStateChanged { get; }
        public HarmonyPatch OnTick { get; }

        public HarmonyPatchSet(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Conductor>();

            Constructor = HarmonyPatch.Patch(nameof(ConductorPatchFactory), members.GetSourceConstructor().Source, PatchType.Postfix);
            OnJumped = HarmonyPatch.Patch(nameof(ConductorPatchFactory), members.GetSourceMethodOf(nameof(Conductor.OnJumped)).Source, PatchType.Prefix);
            OnDoorStateChanged = HarmonyPatch.Patch(nameof(ConductorPatchFactory), members.GetSourceMethodOf(nameof(Conductor.OnDoorStateChanged)).Source, PatchType.Prefix);
            OnTick = HarmonyPatch.Patch(nameof(ConductorPatchFactory), members.GetSourceMethodOf(nameof(Conductor.OnTick)).Source, PatchType.Prefix);
        }

        public void Dispose()
        {
            Constructor.Dispose();
            OnJumped.Dispose();
            OnDoorStateChanged.Dispose();
            OnTick.Dispose();
        }
    }
}
