using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Irony.Parsing;

using BveEx.Extensions.MapStatements.Parsing;

namespace BveEx.Extensions.MapStatements.Builtin.Raw
{
    internal class BuiltinRawParserSet
    {
        public IReadOnlyList<RawParserBase> Items { get; }

        public BuiltinRawParserSet(Action<MapParser, ParseTreeNode> parseNode)
        {
            Items = new RawParserBase[]
            {
                new If(parseNode),
                new Include(),
                new Operators(),
            };
        }
    }
}
