using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace AtsEx.MapStatements
{
    internal class HeaderErrorPreResolver : IDisposable
    {
        private readonly HarmonyPatch HarmonyPatch;

        private HeaderErrorPreResolver(MethodBase target)
        {
            HarmonyPatch = HarmonyPatch.Patch(nameof(HeaderErrorPreResolver), target, PatchType.Prefix);
            HarmonyPatch.Invoked += (sender, e) =>
            {
                string filePath = (string)e.Args[0];

                HeaderParser.HeaderInfo header = HeaderParser.Parse(filePath.ToLowerInvariant(), string.Empty, 0, 0);
                return header.Type == HeaderParser.HeaderType.Invalid ? PatchInvokationResult.DoNothing(e) : new PatchInvokationResult(SkipModes.SkipOriginal);
            };
        }

        public static HeaderErrorPreResolver Patch(BveTypeSet bveTypes)
        {
            ClassMemberSet mapLoaderMembers = bveTypes.GetClassInfoOf<MapLoader>();
            FastMethod includeMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.Include));
            return new HeaderErrorPreResolver(includeMethod.Source);
        }

        public void Dispose()
        {
            HarmonyPatch.Dispose();
        }
    }
}
