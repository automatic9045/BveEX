using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.MapStatements.Builtin.Preprocess
{
    internal interface IBlockParser
    {
        bool CanParse(Statement statement);
        BlockParseResult Parse(Statement statement, int nest, int ignoreNest);
    }
}
