using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Extensions.MapStatements.Builtin.Preprocess;

namespace AtsEx.Extensions.MapStatements.Builtin
{
    internal class BuiltinProcess
    {
        private readonly IfBlock IfBlock = new IfBlock();

        public bool IgnoreStatement => IfBlock.IgnoreStatement;

        public bool TryParse(Statement statement)
        {
            if (IfBlock.CanParse(statement))
            {
                IfBlock.Parse(statement);
                return true;
            }

            return false;
        }
    }
}
