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
        private readonly HarmonyPatch ParsePatch;
        private readonly HarmonyPatch GetSystemVariablePatch;
        private readonly HarmonyPatch CalculateSignPatch;
        private readonly HarmonyPatch CalculateOperatorPatch;
        private readonly HarmonyPatch MapGrammarCtorPatch;

        public MapLoader MapLoader { get; private set; } = null;
        private readonly ExMapParser ExParser = new ExMapParser();

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

            FastMethod parseMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.Parse));
            ParsePatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), parseMethod.Source, PatchType.Prefix);
            ParsePatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                string sourceText = (string)e.Args[0];
                string filePath = (string)e.Args[1];

                instance.FilePath = filePath;
                MapGrammar grammar = new MapGrammar();
                Parser parser = new Parser(grammar.Src);
                ParseTree parseTree = parser.Parse(sourceText);

                foreach (LogMessage log in parseTree.ParserMessages)
                {
                    LoadError error = new LoadError(log.Message, filePath, log.Location.Line + 2, log.Location.Column + 1);
                    instance.Parent.ThrowError(error);
                }

                if (parseTree.Root != null)
                {
                    foreach (ParseTreeNode node in parseTree.Root.ChildNodes)
                    {
                        ExParser.ParseNode(instance, node);
                    }
                }

                return new PatchInvokationResult(SkipModes.SkipOriginal);
            };

            FastMethod getSystemVariableMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.GetSystemVariable));
            GetSystemVariablePatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), getSystemVariableMethod.Source, PatchType.Prefix);
            GetSystemVariablePatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);
                ParseTreeNode node = (ParseTreeNode)e.Args[1];

                if (0 < clauses.Count) return PatchInvokationResult.DoNothing(e);

                string key = ((string)node.Token.Value).ToLowerInvariant();

                bool parsed = ExParser.TryGetSystemVariable(instance, key, out object result);
                return parsed ? new PatchInvokationResult(result, SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };

            FastMethod calculateSignMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.CalculateSign));
            CalculateSignPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), calculateSignMethod.Source, PatchType.Prefix);
            CalculateSignPatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                ParseTreeNode node = (ParseTreeNode)e.Args[0];

                object value = instance.GetValue(node.ChildNodes[1]);
                string operatorName = node.ChildNodes[0].Term.Name;

                bool parsed = ExParser.TryCalculateSign(value, operatorName, out object result);
                return parsed ? new PatchInvokationResult(result, SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };

            FastMethod calculateOperatorMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.CalculateOperator));
            CalculateOperatorPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), calculateOperatorMethod.Source, PatchType.Prefix);
            CalculateOperatorPatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                ParseTreeNode node = (ParseTreeNode)e.Args[0];

                object left = instance.GetValue(node.ChildNodes[0]);
                object right = instance.GetValue(node.ChildNodes[2]);
                string operatorName = node.ChildNodes[1].Term.Name;

                bool parsed = ExParser.TryCalculateOperator(left, right, operatorName, out object result);
                return parsed ? new PatchInvokationResult(result, SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };


            ClassMemberSet mapGrammarMembers = bveTypes.GetClassInfoOf<MapGrammar>();

            FastConstructor mapGrammarCtor = mapGrammarMembers.GetSourceConstructor();
            MapGrammarCtorPatch = HarmonyPatch.Patch(nameof(MapLoaderHacker), mapGrammarCtor.Source, PatchType.Postfix);
            MapGrammarCtorPatch.Invoked += (sender, e) =>
            {
                MapGrammar instance = MapGrammar.FromSource(e.Instance);
                ExParser.ConstructGrammar(instance.Src);

                return PatchInvokationResult.DoNothing(e);
            };
        }

        public void Dispose()
        {
            MapLoaderCtorPatch.Dispose();
            ParseStatementPatch.Dispose();
            IncludePatch.Dispose();
            ParsePatch.Dispose();
            GetSystemVariablePatch.Dispose();
            CalculateSignPatch.Dispose();
            CalculateOperatorPatch.Dispose();
            MapGrammarCtorPatch.Dispose();
        }

        public void Clear()
        {
            MapLoader = null;
        }
    }
}
