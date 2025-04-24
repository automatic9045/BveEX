using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Extensions.LoadErrorManager;

namespace BveEx.Extensions.MapStatements.Builtin.Statements
{
    internal class Error : IParser
    {
        private static readonly ClauseFilter RootFilter = ClauseFilter.Element("Error", 0);
        private static readonly ClauseFilter ThrowFilter = ClauseFilter.Function("Throw", 1);


        private readonly ILoadErrorManager LoadErrorManager;

        public Error(ILoadErrorManager loadErrorManager)
        {
            LoadErrorManager = loadErrorManager;
        }

        public bool CanParse(Statement statement) => statement.IsOfficialStatement(RootFilter);

        public void Parse(Statement statement)
        {
            IReadOnlyList<MapStatementClause> clauses = statement.Source.Clauses;
            if (clauses.Count != 3) throw new SyntaxException(statement);
            if (clauses[0].Keys.Count != 0) throw new SyntaxException(statement);

            if (statement.IsOfficialStatement(RootFilter, ThrowFilter))
            {
                MapStatementClause firstClause = statement.Source.Clauses[0];
                LoadErrorManager.Throw(clauses[2].Args[0].ToString(), statement.Source.FileName, firstClause.LineIndex, firstClause.CharIndex);
            }
            else
            {
                throw new SyntaxException(statement);
            }
        }
    }
}
