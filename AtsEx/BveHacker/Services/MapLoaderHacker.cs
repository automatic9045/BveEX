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

using AtsEx.MapStatements;

namespace AtsEx.BveHackerServices
{
    internal class MapLoaderHacker : IDisposable
    {
        private readonly HarmonyPatch ConstructorPatch;
        private readonly HarmonyPatch RegisterFilePatch;
        private readonly HarmonyPatch IncludePatch;
        private readonly HarmonyPatch GetSystemVariablePatch;

        private HeaderSetFactory HeadersFactory = null;

        public MapLoader MapLoader { get; private set; } = null;
        public HeaderSet Headers { get; private set; } = null;

        public MapLoaderHacker(BveTypeSet bveTypes)
        {
            ClassMemberSet mapLoaderMembers = bveTypes.GetClassInfoOf<MapLoader>();

            FastConstructor constructor = mapLoaderMembers.GetSourceConstructor();
            ConstructorPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), constructor.Source, PatchType.Postfix);
            ConstructorPatch.Invoked += (sender, e) =>
            {
                HeadersFactory = new HeaderSetFactory();
                MapLoader = MapLoader.FromSource(e.Instance);

                return PatchInvokationResult.DoNothing(e);
            };

            FastMethod registerFileMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.RegisterFile));
            RegisterFilePatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), registerFileMethod.Source, PatchType.Postfix);
            RegisterFilePatch.Invoked += (sender, e) =>
            {
                string filePath = (string)e.Args[0];
                if (filePath == MapLoader.FilePath)
                {
                    Headers = HeadersFactory.Build();
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
                ParseTreeNode argNode = node.ChildNodes[1];
                string argText = Convert.ToString(argNode.Token?.Value);

                SourceLocation sourceLocation = node.Span.Location;
                bool isHeader = HeadersFactory.Register(argText.ToLowerInvariant(), instance.FilePath, sourceLocation.Line + 2, sourceLocation.Column + 1);

                if (!isHeader)
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
            RegisterFilePatch.Dispose();
            IncludePatch.Dispose();
        }

        public void Clear()
        {
            HeadersFactory = null;
            MapLoader = null;
            Headers = null;
        }
    }
}
