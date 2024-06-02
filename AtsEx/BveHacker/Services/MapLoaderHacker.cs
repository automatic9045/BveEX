using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace AtsEx.BveHackerServices
{
    internal class MapLoaderHacker : IDisposable
    {
        private readonly HarmonyPatch Patch;

        public MapLoader MapLoader { get; private set; } = null;

        public MapLoaderHacker(BveTypeSet bveTypes)
        {
            ClassMemberSet mapLoaderMembers = bveTypes.GetClassInfoOf<MapLoader>();
            FastConstructor constructor = mapLoaderMembers.GetSourceConstructor();
            Patch = HarmonyPatch.Patch(nameof(MapLoaderHacker), constructor.Source, PatchType.Postfix);
            Patch.Invoked += (sender, e) =>
            {
                MapLoader = MapLoader.FromSource(e.Instance);
                return PatchInvokationResult.DoNothing(e);
            };
        }

        public void Dispose()
        {
            Patch.Dispose();
        }
    }
}
