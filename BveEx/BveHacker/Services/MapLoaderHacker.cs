using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using BveTypes.ClassWrappers.Extensions;
using FastMember;
using Irony.Parsing;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.Launching;

namespace BveEx.BveHackerServices
{
    internal class MapLoaderHacker : IDisposable
    {
        private readonly HarmonyPatch ConstructorPatch;
        private readonly HarmonyPatch ParseStatementPatch;
        private readonly HarmonyPatch IncludePatch;
        private readonly HarmonyPatch GetSystemVariablePatch;

        public MapLoader MapLoader { get; private set; } = null;

        public MapLoaderHacker(BveTypeSet bveTypes)
        {
            ClassMemberSet mapLoaderMembers = bveTypes.GetClassInfoOf<MapLoader>();

            FastConstructor constructor = mapLoaderMembers.GetSourceConstructor();
            ConstructorPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), constructor.Source, PatchType.Postfix);
            ConstructorPatch.Invoked += (sender, e) =>
            {
                MapLoader = MapLoader.FromSource(e.Instance);

                return PatchInvokationResult.DoNothing(e);
            };

            FastMethod parseStatementMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.ParseStatement));
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
                    MapLoader.Include(Convert.ToString(result));
                }

                return new PatchInvokationResult(SkipModes.SkipOriginal);
            };

            FastMethod getSystemVariableMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.GetSystemVariable));
            GetSystemVariablePatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), getSystemVariableMethod.Source, PatchType.Prefix);
            GetSystemVariablePatch.Invoked += (sender, e) =>
            {
                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);
                ParseTreeNode node = (ParseTreeNode)e.Args[1];

                if (0 < clauses.Count) return PatchInvokationResult.DoNothing(e);

                string key = ((string)node.Token.Value).ToLowerInvariant();
                switch (key)
                {
                    case "ex_relativedir":
                        MapParser instance = MapParser.FromSource(e.Instance);

                        Uri rootUri = new Uri(Path.GetDirectoryName(MapLoader.FilePath) + Path.DirectorySeparatorChar);
                        Uri subUri = new Uri(Path.GetDirectoryName(instance.FilePath) + Path.DirectorySeparatorChar);

                        Uri relativeUri = rootUri.MakeRelativeUri(subUri);
                        string relativePath = relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar);

                        return new PatchInvokationResult(relativePath, SkipModes.SkipOriginal);

                    default:
                        return PatchInvokationResult.DoNothing(e);
                }
            };
        }

        public void Dispose()
        {
            ConstructorPatch.Dispose();
            ParseStatementPatch.Dispose();
            IncludePatch.Dispose();
            GetSystemVariablePatch.Dispose();
        }

        public void Clear()
        {
            MapLoader = null;
        }
    }
}
