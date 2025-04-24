using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Extensions.LoadErrorManager;

namespace BveEx.Extensions.MapStatements.Builtin.Statements
{
    internal class BuiltinStatementSet
    {
        private readonly List<IParser> StatementParsers;

        public BuiltinStatementSet(ILoadErrorManager loadErrorManager)
        {
            StatementParsers = new List<IParser>()
            {
                new Dialog(),
                new Error(loadErrorManager),
            };
        }

        public void Parse(Statement statement)
        {
            foreach (IParser parser in StatementParsers)
            {
                if (parser.CanParse(statement))
                {
                    parser.Parse(statement);
                    break;
                }
            }
        }
    }
}
