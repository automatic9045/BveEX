using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Extensions.MapStatements.Builtin.Preprocess;
using BveEx.Extensions.MapStatements.Builtin.Statements;

namespace BveEx.Extensions.MapStatements.Builtin
{
    internal class BuiltinProcess
    {
        private readonly List<IBlockParser> BlockParsers = new List<IBlockParser>()
        {
            new IfBlock(),
        };

        private readonly List<IParser> StatementParsers = new List<IParser>()
        {
            new Dialog(),
        };

        private int Nest = 0;

        private IBlockParser IgnoreReason = null;
        private int IgnoreNest = int.MaxValue;
        public bool IgnoreStatement => IgnoreNest <= Nest;

        public bool TryParse(Statement statement)
        {
            foreach (IBlockParser blockParser in BlockParsers)
            {
                if (IgnoreNest <= Nest && blockParser != IgnoreReason) continue;

                if (blockParser.CanParse(statement))
                {
                    BlockParseResult result = blockParser.Parse(statement, Nest, IgnoreNest);

                    switch (result.NestOperation)
                    {
                        case BlockParseResult.NestOperationMode.Continue:
                            break;
                        case BlockParseResult.NestOperationMode.Begin:
                            Nest++;
                            break;
                        case BlockParseResult.NestOperationMode.End:
                            Nest--;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    if (!result.IgnoreMyself)
                    {
                        if (result.IgnoreFollowing)
                        {
                            IgnoreReason = blockParser;
                            IgnoreNest = Nest;
                        }
                        else
                        {
                            IgnoreReason = null;
                            IgnoreNest = int.MaxValue;
                        }
                    }

                    return true;
                }
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
