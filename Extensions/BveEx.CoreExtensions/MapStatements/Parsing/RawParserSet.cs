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
using Irony;
using Irony.Parsing;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.Extensions.MapStatements.Builtin.Raw;

namespace BveEx.Extensions.MapStatements.Parsing
{
    internal class RawParserSet : IDisposable
    {
        private readonly HarmonyPatch ParsePatch;
        private readonly HarmonyPatch GetSystemVariablePatch;
        private readonly HarmonyPatch CalculateSignPatch;
        private readonly HarmonyPatch CalculateOperatorPatch;
        private readonly HarmonyPatch MapGrammarCtorPatch;

        private readonly BuiltinRawParserSet Builtin;
        private readonly List<RawParserBase> Items;

        public RawParserSet(BveTypeSet bveTypes)
        {
            Builtin = new BuiltinRawParserSet(ParseNode);
            Items = new List<RawParserBase>(Builtin.Items);


            ClassMemberSet mapParserMembers = bveTypes.GetClassInfoOf<MapParser>();

            FastMethod parseMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.Parse));
            ParsePatch = HarmonyPatch.Patch(nameof(MapStatements), parseMethod.Source, PatchType.Prefix);
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
                        ParseNode(instance, node);
                    }
                }

                return new PatchInvokationResult(SkipModes.SkipOriginal);
            };
            
            FastMethod getSystemVariableMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.GetSystemVariable));
            GetSystemVariablePatch = HarmonyPatch.Patch(nameof(MapStatements), getSystemVariableMethod.Source, PatchType.Prefix);
            GetSystemVariablePatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);
                ParseTreeNode node = (ParseTreeNode)e.Args[1];

                if (0 < clauses.Count) return PatchInvokationResult.DoNothing(e);

                string key = ((string)node.Token.Value).ToLowerInvariant();

                bool parsed = false;
                object result = null;
                foreach (RawParserBase exParser in Items)
                {
                    parsed = exParser.TryGetSystemVariable(instance, key, out result);
                    if (parsed) break;
                }

                return parsed ? new PatchInvokationResult(result, SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };

            FastMethod calculateSignMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.CalculateSign));
            CalculateSignPatch = HarmonyPatch.Patch(nameof(MapStatements), calculateSignMethod.Source, PatchType.Prefix);
            CalculateSignPatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                ParseTreeNode node = (ParseTreeNode)e.Args[0];

                object value = instance.GetValue(node.ChildNodes[1]);
                string operatorName = node.ChildNodes[0].Term.Name;

                bool parsed = false;
                object result = null;
                foreach (RawParserBase exParser in Items)
                {
                    parsed = exParser.TryCalculateSign(value, operatorName, out result);
                    if (parsed) break;
                }

                return parsed ? new PatchInvokationResult(result, SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };

            FastMethod calculateOperatorMethod = mapParserMembers.GetSourceMethodOf(nameof(MapParser.CalculateOperator));
            CalculateOperatorPatch = HarmonyPatch.Patch(nameof(MapStatements), calculateOperatorMethod.Source, PatchType.Prefix);
            CalculateOperatorPatch.Invoked += (sender, e) =>
            {
                MapParser instance = MapParser.FromSource(e.Instance);

                ParseTreeNode node = (ParseTreeNode)e.Args[0];

                object left = instance.GetValue(node.ChildNodes[0]);
                object right = instance.GetValue(node.ChildNodes[2]);
                string operatorName = node.ChildNodes[1].Term.Name;

                bool parsed = false;
                object result = null;
                foreach (RawParserBase exParser in Items)
                {
                    parsed = exParser.TryCalculateOperator(left, right, operatorName, out result);
                    if (parsed) break;
                }

                return parsed ? new PatchInvokationResult(result, SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };


            ClassMemberSet mapGrammarMembers = bveTypes.GetClassInfoOf<MapGrammar>();

            FastConstructor mapGrammarCtor = mapGrammarMembers.GetSourceConstructor();
            MapGrammarCtorPatch = HarmonyPatch.Patch(nameof(MapStatements), mapGrammarCtor.Source, PatchType.Postfix);
            MapGrammarCtorPatch.Invoked += (sender, e) =>
            {
                MapGrammar instance = MapGrammar.FromSource(e.Instance);
                TerminalSet terminals = new TerminalSet(instance.Src);

                foreach (RawParserBase exParser in Items)
                {
                    exParser.ConstructGrammar(instance.Src, terminals);
                }

                return PatchInvokationResult.DoNothing(e);
            };


            void ParseNode(MapParser parser, ParseTreeNode node)
            {
                try
                {
                    bool parsed = false;
                    foreach (RawParserBase exParser in Items)
                    {
                        parsed = exParser.TryParseNode(parser, node);
                        if (parsed) break;
                    }

                    if (!parsed)
                    {
                        switch (node.Term.Name)
                        {
                            case "IncludeStatement":
                                parser.Include(node);
                                break;

                            case "VariableDeclarator":
                                parser.SetUserVariable(node);
                                break;

                            case "SYNTAX_ERROR":
                                break;

                            default:
                                parser.SetLocation(node);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ParseTreeNode item = node;

                    int lineIndex = 0;
                    int columnIndex = 0;
                    while (true)
                    {
                        if (item.Token is null)
                        {
                            if (item.ChildNodes.Count <= 0) break;
                        }
                        else
                        {
                            lineIndex = item.Token.Location.Line + 2;
                            columnIndex = item.Token.Location.Column + 1;
                            break;
                        }

                        item = item.ChildNodes[0];
                    }

                    LoadError error = new LoadError(ex.Message, parser.FilePath, lineIndex, columnIndex);
                    parser.Parent.ThrowError(error);
                }
            }
        }

        public void Dispose()
        {
            ParsePatch.Dispose();
            GetSystemVariablePatch.Dispose();
            CalculateSignPatch.Dispose();
            CalculateOperatorPatch.Dispose();
            MapGrammarCtorPatch.Dispose();
        }

        public void Add(RawParserBase parser) => Items.Add(parser);

        public IReadOnlyDictionary<string, IReadOnlyList<string>> GetUsingChains(Uri mapUri) => Builtin.GetUsingChains(mapUri);
    }
}
