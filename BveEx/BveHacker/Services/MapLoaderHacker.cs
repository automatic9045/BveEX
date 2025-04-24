using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using BveTypes.ClassWrappers.Extensions;
using FastMember;
using Irony;
using Irony.Parsing;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.Launching;

namespace BveEx.BveHackerServices
{
    internal class MapLoaderHacker : IDisposable
    {
        private readonly HarmonyPatch MapLoaderCtorPatch;
        private readonly HarmonyPatch ParseStatementPatch;
        private readonly HarmonyPatch IncludePatch;

        public MapLoader MapLoader { get; private set; } = null;

        public MapLoaderHacker(BveTypeSet bveTypes)
        {
            ClassMemberSet mapLoaderMembers = bveTypes.GetClassInfoOf<MapLoader>();

            FastConstructor mapLoaderCtor = mapLoaderMembers.GetSourceConstructor();
            MapLoaderCtorPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), mapLoaderCtor.Source, PatchType.Postfix);
            MapLoaderCtorPatch.Invoked += (sender, e) =>
            {
                MapLoader = MapLoader.FromSource(e.Instance);

                return PatchInvokationResult.DoNothing(e);
            };

            FastMethod parseStatementMethod = mapLoaderMembers.GetSourceMethodOf(nameof(BveTypes.ClassWrappers.MapLoader.ParseStatement));
            ParseStatementPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), parseStatementMethod.Source, PatchType.Prefix);
            ParseStatementPatch.Invoked += (sender, e) =>
            {
                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);

                if (0 < clauses.Count && clauses[0].Name.ToLowerInvariant() == "atsex")
                {
                    throw new LaunchModeException();
                }

                return PatchInvokationResult.DoNothing(e);
            };


            ClassMemberSet mapParserMembers = bveTypes.GetClassInfoOf<MapParser>();

            FastMethod includeMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.Include));
            IncludePatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), includeMethod.Source, PatchType.Prefix);
            IncludePatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                ParseTreeNode node = (ParseTreeNode)e.Args[0];
                string argText = Convert.ToString(node.ChildNodes[1].Token?.Value);

                if (HeaderParser.IsLegacyHeader(argText))
                {
                    throw new LaunchModeException();
                }
                else if (!HeaderParser.IsNoMapPluginHeader(argText))
                {
                    object result = instance.GetValue(node.ChildNodes[1]);
                    instance.Parent.Include(Convert.ToString(result));
                }

                return new PatchInvokationResult(SkipModes.SkipOriginal);
            };
        }

        public void Dispose()
        {
            MapLoaderCtorPatch.Dispose();
            ParseStatementPatch.Dispose();
            IncludePatch.Dispose();
        }

        public void Clear()
        {
            MapLoader = null;
        }
    }
}
