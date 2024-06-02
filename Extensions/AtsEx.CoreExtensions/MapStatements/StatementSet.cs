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

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

using AtsEx.Extensions.MapStatements.Builtin;

namespace AtsEx.Extensions.MapStatements
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IStatementSet))]
    internal class StatementSet : AssemblyPluginBase, IStatementSet
    {
        private readonly BuiltinProcess BuiltinProcess;
        private readonly HarmonyPatch Patch;

        private List<Statement> Statements = null;

        public event EventHandler<StatementLoadedEventArgs> StatementLoaded;

        public StatementSet(PluginBuilder builder) : base(builder)
        {
            BuiltinProcess = new BuiltinProcess();

            ClassMemberSet mapLoaderMembers = BveHacker.BveTypes.GetClassInfoOf<MapLoader>();
            FastMethod parseStatementMethod = mapLoaderMembers.GetSourceMethodOf(nameof(MapLoader.ParseStatement));
            Patch = HarmonyPatch.Patch(nameof(MapStatements), parseStatementMethod.Source, PatchType.Prefix);
            Patch.Invoked += (sender, e) =>
            {
                WrappedList<MapStatementClause> clauses = WrappedList<MapStatementClause>.FromSource((IList)e.Args[0]);

                if (0 < clauses.Count && clauses[0].Name.ToLowerInvariant() == "atsex")
                {
                    MapLoader instance = MapLoader.FromSource(e.Instance);

                    MapStatement source = instance.Statements.First(x => x.Clauses == clauses);
                    Statement statement = new Statement(source);

                    bool isPreprocess = BuiltinProcess.TryParse(statement);
                    if (!isPreprocess && !BuiltinProcess.IgnoreStatement)
                    {
                        Statements.Add(statement);
                        StatementLoaded?.Invoke(this, new StatementLoadedEventArgs(statement));
                    }

                    return new PatchInvokationResult(SkipModes.SkipOriginal);
                }

                return BuiltinProcess.IgnoreStatement ? new PatchInvokationResult(SkipModes.SkipOriginal) : PatchInvokationResult.DoNothing(e);
            };

            BveHacker.ScenarioOpened += OnScenarioOpened;
            BveHacker.ScenarioClosed += OnScenarioClosed;
        }

        private void OnScenarioOpened(ScenarioOpenedEventArgs e)
        {
            Statements = new List<Statement>();
        }

        private void OnScenarioClosed(EventArgs e)
        {
            Statements = null;
        }

        public override void Dispose()
        {
            Patch.Dispose();
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            return new ExtensionTickResult();
        }

        public IEnumerator<Statement> GetEnumerator() => Statements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<Statement> FindStatements(IEnumerable<ClauseFilter> clauses) => Statements.Where(statement => statement.FilterMatches(clauses));

        public Statement FindOfficialStatement(params ClauseFilter[] clauses) => FindOfficialStatements(clauses).FirstOrDefault();
        public IEnumerable<Statement> FindOfficialStatements(params ClauseFilter[] clauses)
        {
            if (0 < clauses.Length && clauses[0].Name.ToLowerInvariant() == "user") throw new ArgumentException();
            return FindStatements(clauses);
        }

        public Statement FindUserStatement(string userName, params ClauseFilter[] clauses) => FindUserStatements(userName, clauses).FirstOrDefault();
        public IEnumerable<Statement> FindUserStatements(string userName, params ClauseFilter[] clauses)
        {
            ClauseFilter[] userClauses = new[]
            {
                new ClauseFilter("User", ClauseType.Element), new ClauseFilter(userName, ClauseType.Element),
            };

            return FindStatements(userClauses.Concat(clauses));
        }
    }
}
