using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Extensions.LoadErrorManager;
using BveEx.Extensions.MapStatements.Builtin.Statements;

namespace BveEx.Extensions.MapStatements.Builtin
{
    internal class BuiltinProcess
    {
        private readonly List<IParser> StatementParsers;

        public BuiltinProcess(ILoadErrorManager loadErrorManager)
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
