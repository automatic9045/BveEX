using System;
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

using BveEx.Extensions.MapStatements.Builtin;

namespace BveEx.Extensions.MapStatements
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IStatementSet))]
    internal class StatementSet : AssemblyPluginBase, IStatementSet
    {
        private readonly HarmonyPatch ParsePatch;
        private readonly HarmonyPatch PostLoadPatch;

        private BuiltinProcess BuiltinProcess;
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
                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);

                if (0 < clauses.Count && clauses[0].Name.ToLowerInvariant() == "bveex")
                {
                    MapLoader instance = MapLoader.FromSource(e.Instance);

                    MapStatement source = instance.Statements.First(x => x.Clauses == clauses);
                    Statement statement = new Statement(source);

                    bool isPreprocess = BuiltinProcess.TryParse(statement);
                    if (!isPreprocess && !BuiltinProcess.IgnoreStatement)
                    {
                        Statements.Add(statement);
                        StatementLoaded?.Invoke(this, new StatementLoadedEventArgs(statement, instance));
                    }

                    return new PatchInvokationResult(SkipModes.SkipOriginal);
                }

                return BuiltinProcess.IgnoreStatement ? new PatchInvokationResult(SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };

            FastMethod loadMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.Load));
            PostLoadPatch = HarmonyPatch.Patch(nameof(MapStatements), loadMethod.Source, PatchType.Postfix);
            PostLoadPatch.Invoked += (sender, e) =>
            {
                LoadingCompleted?.Invoke(this, EventArgs.Empty);
                return PatchInvokationResult.DoNothing(e);
            };

            BveHacker.ScenarioOpened += OnScenarioOpened;
            BveHacker.ScenarioClosed += OnScenarioClosed;
        }

        private void OnScenarioOpened(ScenarioOpenedEventArgs e)
        {
            BuiltinProcess = new BuiltinProcess();
            Statements = new List<Statement>();
        }

        private void OnScenarioClosed(EventArgs e)
        {
            BuiltinProcess = null;
            Statements = null;
        }

        public override void Dispose()
        {
            ParsePatch.Dispose();
            PostLoadPatch.Dispose();
        }

        public override void Tick(TimeSpan elapsed)
        {
        }

        public IEnumerator<Statement> GetEnumerator() => Statements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Statement FindOfficialStatement(params ClauseFilter[] clauses) => FindOfficialStatements(clauses).FirstOrDefault();
        public IEnumerable<Statement> FindOfficialStatements(params ClauseFilter[] clauses) => Statements.Where(statement => statement.IsOfficialStatement(clauses));

        public Statement FindUserStatement(string userName, params ClauseFilter[] clauses) => FindUserStatements(userName, clauses).FirstOrDefault();
        public IEnumerable<Statement> FindUserStatements(string userName, params ClauseFilter[] clauses) => Statements.Where(statement => statement.IsUserStatement(userName, clauses));
    }
}
