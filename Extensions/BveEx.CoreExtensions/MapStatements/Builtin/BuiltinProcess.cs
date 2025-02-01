using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Extensions.MapStatements.Builtin.Statements;

namespace BveEx.Extensions.MapStatements.Builtin
{
    internal class BuiltinProcess
    {
        private readonly List<IParser> StatementParsers = new List<IParser>()
        {
            new Dialog(),
        };

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
