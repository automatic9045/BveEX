using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveTypes.ClassWrappers;

namespace AtsEx.BveHackerServices
{
    internal sealed class StructureSetLifeProlonger : IDisposable
    {
        private readonly HarmonyPatch SetRouteMethodPatch;

        private readonly BveHacker BveHacker;

        public StructureSetLifeProlonger(BveHacker bveHacker)
        {
            BveHacker = bveHacker;

            ClassMemberSet trainDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<ObjectDrawer>();
            FastMethod setRouteMethod = trainDrawerMembers.GetSourceMethodOf(nameof(ObjectDrawer.SetRoute));

            SetRouteMethodPatch = HarmonyPatch.Patch(nameof(StructureSetLifeProlonger), setRouteMethod.Source, PatchType.Prefix);
            SetRouteMethodPatch.Invoked += (_, e) =>
            {
                Route route = Route.FromSource(e.Args[0]);
                StructureSet structures = route.Structures;

                BveHacker.PreviewScenarioCreated += e2 => e2.Scenario.Route.Structures = structures;

                return PatchInvokationResult.DoNothing(e);
            };
        }

        public void Dispose()
        {
            SetRouteMethodPatch.Dispose();
        }
    }
}
