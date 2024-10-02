using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
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
