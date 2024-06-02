using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.MapStatements.Builtin
{
    internal class BuiltinProcess
    {
        public bool IgnoreStatement { get; } = false;

        public bool TryParse(Statement statement)
        {
            return false;
        }
    }
}
