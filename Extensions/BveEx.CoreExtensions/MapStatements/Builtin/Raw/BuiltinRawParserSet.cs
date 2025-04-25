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
        private readonly Using Using;

        public IReadOnlyList<RawParserBase> Items { get; }

        public BuiltinRawParserSet(Action<MapParser, ParseTreeNode> parseNode)
        {
            Using = new Using();

            Items = new RawParserBase[]
            {
                new If(parseNode),
                new Include(),
                new Loop(parseNode),
                new Operators(),
                Using,
            };
        }

        public IReadOnlyDictionary<string, IReadOnlyList<string>> GetUsingChains(Uri mapUri) => Using.GetUsingChains(mapUri);
    }
}
