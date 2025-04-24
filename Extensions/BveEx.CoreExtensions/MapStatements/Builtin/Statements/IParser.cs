using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.MapStatements.Builtin.Statements
{
    internal interface IParser
    {
        bool CanParse(Statement statement);
        void Parse(Statement statement);
    }
}
