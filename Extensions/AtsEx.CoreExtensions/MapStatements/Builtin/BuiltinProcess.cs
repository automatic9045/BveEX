using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Extensions.MapStatements.Builtin.Preprocess;
using AtsEx.Extensions.MapStatements.Builtin.Statements;

namespace AtsEx.Extensions.MapStatements.Builtin
{
    internal class BuiltinProcess
    {
        private readonly IfBlock IfBlock = new IfBlock();

        private readonly List<IParser> StatementParsers = new List<IParser>()
        {
            new Dialog(),
        };

        public bool IgnoreStatement => IfBlock.IgnoreStatement;

        public bool TryParse(Statement statement)
        {
            if (IfBlock.CanParse(statement))
            {
                IfBlock.Parse(statement);
                return true;
            }

            if (!IgnoreStatement)
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

            return false;
        }
    }
}
