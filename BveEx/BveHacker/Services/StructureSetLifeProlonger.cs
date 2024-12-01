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

namespace BveEx.BveHackerServices
{
    internal sealed class StructureSetLifeProlonger : IDisposable
    {
        private readonly HarmonyPatch SetMapMethodPatch;

        private readonly BveHacker BveHacker;

        public StructureSetLifeProlonger(BveHacker bveHacker)
        {
            BveHacker = bveHacker;

            ClassMemberSet trainDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<ObjectDrawer>();
            FastMethod setMapMethod = trainDrawerMembers.GetSourceMethodOf(nameof(ObjectDrawer.SetMap));

            SetMapMethodPatch = HarmonyPatch.Patch(nameof(StructureSetLifeProlonger), setMapMethod.Source, PatchType.Prefix);
            SetMapMethodPatch.Invoked += (_, e) =>
            {
                Map map = Map.FromSource(e.Args[0]);
                StructureSet structures = map.Structures;

                BveHacker.PreviewScenarioCreated += e2 => e2.Scenario.Map.Structures = structures;

                return PatchInvokationResult.DoNothing(e);
            };
        }

        public void Dispose()
        {
            SetMapMethodPatch.Dispose();
        }
    }
}
