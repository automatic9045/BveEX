﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using BveTypes.ClassWrappers.Extensions;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

using BveEx.Extensions.LoadErrorManager;
using BveEx.Extensions.MapStatements.Builtin.Statements;
using BveEx.Extensions.MapStatements.Parsing;

namespace BveEx.Extensions.MapStatements
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IStatementSet))]
    internal class StatementSet : AssemblyPluginBase, IStatementSet
    {
        private readonly HarmonyPatch ParsePatch;
        private readonly HarmonyPatch PostLoadPatch;
        private readonly RawParserSet RawParsers;

        private BuiltinStatementSet BuiltinStatements;
        private List<Statement> Statements = null;

        public override string Title { get; } = nameof(MapStatements);
        public override string Description { get; } = "プラグインからオリジナルのマップ構文を簡単に定義・参照できるようにします。";

        public event EventHandler<StatementLoadedEventArgs> StatementLoaded;
        public event EventHandler LoadingCompleted;

        public StatementSet(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet mapLoaderMembers = BveHacker.BveTypes.GetClassInfoOf<MapLoader>();

            FastMethod parseStatementMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.ParseStatement));
            ParsePatch = HarmonyPatch.Patch(nameof(MapStatements), parseStatementMethod.Source, PatchType.Prefix);
            ParsePatch.Invoked += (sender, e) =>
            {
                MapLoader instance = MapLoader.FromSource(e.Instance);
                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);

                if (0 < clauses.Count)
                {
                    string firstClauseName = clauses[0].Name.ToLowerInvariant();

                    if (firstClauseName[0] == '_')
                    {
                        Uri mapUri = new Uri(instance.FilePath, UriKind.Absolute);
                        IReadOnlyDictionary<string, IReadOnlyList<string>> usingChains = RawParsers.GetUsingChains(mapUri);

                        if (usingChains.TryGetValue(firstClauseName, out IReadOnlyList<string> original))
                        {
                            MapStatement source = instance.Statements.First(x => x.Clauses == clauses);
                            MapStatement fixedSource = new MapStatement(source.Location, new WrappedList<MapStatementClause>(original.Count + source.Clauses.Count - 1), source.FileName);

                            foreach (string clauseName in original)
                            {
                                MapStatementClause clause = new MapStatementClause(clauseName, source.Clauses[0].LineIndex, source.Clauses[0].CharIndex);
                                fixedSource.Clauses.Add(clause);
                            }

                            MapStatementClause originalLastClause = fixedSource.Clauses[fixedSource.Clauses.Count - 1];
                            originalLastClause.Keys = source.Clauses[0].Keys;

                            foreach (MapStatementClause clause in source.Clauses.Skip(1))
                            {
                                fixedSource.Clauses.Add(clause);
                            }

                            RegisterStatement(fixedSource, source);

                            return new PatchInvokationResult(SkipModes.SkipOriginal);
                        }
                    }
                    else if (firstClauseName == "bveex")
                    {
                        MapStatement source = instance.Statements.First(x => x.Clauses == clauses);
                        RegisterStatement(source, source);

                        return new PatchInvokationResult(SkipModes.SkipOriginal);
                    }


                    void RegisterStatement(MapStatement source, MapStatement originalSource)
                    {
                        Statement statement = new Statement(source, originalSource);

                        BuiltinStatements.Parse(statement);
                        Statements.Add(statement);
                        StatementLoaded?.Invoke(this, new StatementLoadedEventArgs(statement, instance));
                    }
                }

                return PatchInvokationResult.DoNothing(e);
            };

            FastMethod loadMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.Load));
            PostLoadPatch = HarmonyPatch.Patch(nameof(MapStatements), loadMethod.Source, PatchType.Postfix);
            PostLoadPatch.Invoked += (sender, e) =>
            {
                LoadingCompleted?.Invoke(this, EventArgs.Empty);
                return PatchInvokationResult.DoNothing(e);
            };

            RawParsers = new RawParserSet(BveHacker.BveTypes);

            BveHacker.ScenarioOpened += OnScenarioOpened;
            BveHacker.ScenarioClosed += OnScenarioClosed;
        }

        private void OnScenarioOpened(ScenarioOpenedEventArgs e)
        {
            ILoadErrorManager loadErrorManager = Extensions.GetExtension<ILoadErrorManager>();
            BuiltinStatements = new BuiltinStatementSet(loadErrorManager);
            Statements = new List<Statement>();
        }

        private void OnScenarioClosed(EventArgs e)
        {
            BuiltinStatements = null;
            Statements = null;
        }

        public override void Dispose()
        {
            ParsePatch.Dispose();
            PostLoadPatch.Dispose();
            RawParsers.Dispose();
        }

        public override void Tick(TimeSpan elapsed)
        {
        }

        public IEnumerator<Statement> GetEnumerator() => Statements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void RegisterParser(RawParserBase parser) => RawParsers.Add(parser);

        public Statement FindOfficialStatement(params ClauseFilter[] clauses) => FindOfficialStatements(clauses).FirstOrDefault();
        public IEnumerable<Statement> FindOfficialStatements(params ClauseFilter[] clauses) => Statements.Where(statement => statement.IsOfficialStatement(clauses));

        public Statement FindUserStatement(string userName, params ClauseFilter[] clauses) => FindUserStatements(userName, clauses).FirstOrDefault();
        public IEnumerable<Statement> FindUserStatements(string userName, params ClauseFilter[] clauses) => Statements.Where(statement => statement.IsUserStatement(userName, clauses));
    }
}
