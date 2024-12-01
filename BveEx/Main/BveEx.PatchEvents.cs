using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;

using BveEx.Native;

namespace BveEx
{
    internal partial class BveEx
    {
        private void ListenPatchEvents()
        {
            Patches.LoadScenarioPatch.Invoked += (sender, e) =>
            {
                ScenarioInfo scenarioInfo = ScenarioInfo.FromSource(e.Args[0]);

                ScenarioOpened?.Invoke(this, new ValueEventArgs<ScenarioInfo>(scenarioInfo));
                return PatchInvokationResult.DoNothing(e);
            };

            Patches.DisposeScenarioPatch.Invoked += (sender, e) =>
            {
                Scenario scenario = Scenario.FromSource(e.Instance);

                ScenarioClosed?.Invoke(this, new ValueEventArgs<Scenario>(scenario));
                return PatchInvokationResult.DoNothing(e);
            };


            Patches.OnSetVehicleSpecPatch.Invoked += (sender, e) =>
            {
                OnSetVehicleSpec?.Invoke(this, EventArgs.Empty);
                return PatchInvokationResult.DoNothing(e);
            };

            Patches.OnInitializePatch.Invoked += (sender, e) =>
            {
                OnInitialize?.Invoke(this, EventArgs.Empty);
                return PatchInvokationResult.DoNothing(e);
            };

            Patches.PostElapsePatch.Invoked += (sender, e) =>
            {
                AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);

                TimeSpan now = TimeSpan.FromMilliseconds((int)e.Args[0]);

                PostElapse?.Invoke(this, new ValueEventArgs<TimeSpan>(now));
                return PatchInvokationResult.DoNothing(e);
            };
        }
    }
}
