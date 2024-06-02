using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Utilities;
using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    internal sealed partial class StatementSet : IStatementSet, IEnumerable<Statement>
    {
        private readonly IDictionary<Identifier, IReadOnlyList<Statement>> Statements;
        private readonly IEnumerable<ObservableTrain> Trains;

        public StatementSet(IDictionary<Identifier, IReadOnlyList<Statement>> statements, IEnumerable<ObservableTrain> trains)
        {
            Statements = statements;
            Trains = trains;
        }

        public IReadOnlyList<IStatement> GetAll(Identifier identifier) => Statements.TryGetValue(identifier, out IReadOnlyList<Statement> result) ? result : new List<Statement>();

        public IEnumerator<Statement> GetEnumerator() => new EnumerableInDictionaryEnumerator<Identifier, Statement>(Statements);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Tick(double vehicleLocation, double preTrainLocation)
        {
            Parallel.ForEach(Trains, train => train.UpdateLocation());
            foreach (Statement statement in this) statement.Tick(vehicleLocation, preTrainLocation);
        }
    }
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
}
