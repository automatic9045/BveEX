using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ObjectiveHarmonyPatch;

namespace AtsEx.Extensions.ConductorPatch
{
    internal class ConductorPatchInvoker
    {
        private readonly HarmonyPatchSet HarmonyPatches;

        public ConductorPatch ActivePatch { get; set; } = null;

        public ConductorPatchInvoker(HarmonyPatchSet harmonyPatches)
        {
            HarmonyPatches = harmonyPatches;

            HarmonyPatches.OnJumped.Invoked += OnJumped;
            HarmonyPatches.OnDoorStateChanged.Invoked += OnDoorStateChanged;
            HarmonyPatches.OnTick.Invoked += OnTick;
        }

        private PatchInvokationResult OnJumped(object sender, PatchInvokedEventArgs e)
        {
            if (ActivePatch is null) return PatchInvokationResult.DoNothing(e);

            MethodOverrideMode overrideMode = ActivePatch.Conductor.OnJumped((int)e.Args[0], (bool)e.Args[1]);
            return CreateInvokationResult(overrideMode);
        }

        private PatchInvokationResult OnDoorStateChanged(object sender, PatchInvokedEventArgs e)
        {
            if (ActivePatch is null) return PatchInvokationResult.DoNothing(e);

            MethodOverrideMode overrideMode = ActivePatch.Conductor.OnDoorStateChanged();
            return CreateInvokationResult(overrideMode);
        }

        private PatchInvokationResult OnTick(object sender, PatchInvokedEventArgs e)
        {
            if (ActivePatch is null) return PatchInvokationResult.DoNothing(e);

            MethodOverrideMode overrideMode = ActivePatch.Conductor.OnTick();
            return CreateInvokationResult(overrideMode);
        }

        private PatchInvokationResult CreateInvokationResult(MethodOverrideMode overrideMode)
        {
            switch (overrideMode)
            {
                case MethodOverrideMode.SkipOriginal:
                    return new PatchInvokationResult(SkipModes.SkipOriginal);

                case MethodOverrideMode.RunOriginal:
                    return new PatchInvokationResult(SkipModes.Continue);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
