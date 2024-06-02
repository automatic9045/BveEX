using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.MapStatements.Builtin.Preprocess
{
    internal interface IBlockParser : IParser
    {
        bool IgnoreStatement { get; }
    }
}
