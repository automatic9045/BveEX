using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace AtsEx.Extensions.MapStatements.Builtin.Preprocess
{
    internal class IfBlock : IBlockParser
    {
        private static readonly ClauseFilter RootFilter = ClauseFilter.Element("If", 0);

        private static readonly ClauseFilter BeginIfFilter = ClauseFilter.Function("BeginIf", 1, 3);
        private static readonly ClauseFilter ElseIfFilter = ClauseFilter.Function("ElseIf", 1, 3);
        private static readonly ClauseFilter ElseFilter = ClauseFilter.Function("Else", 0);
        private static readonly ClauseFilter EndFilter = ClauseFilter.Function("End", 0);

        /// <summary>
        /// 各ネストの if ブロックにおいて、既にマッチする条件が出現したかどうか。
        /// </summary>
        private readonly Dictionary<int, bool> HasMatched = new Dictionary<int, bool>();

        public bool CanParse(Statement statement) => statement.FilterMatches(RootFilter);

        public BlockParseResult Parse(Statement statement, int nest, int ignoreNest)
        {
            IReadOnlyList<MapStatementClause> clauses = statement.Source.Clauses;
            if (clauses.Count != 3) throw new SyntaxException(statement);

            if (statement.FilterMatches(RootFilter, BeginIfFilter))
            {
                if (ignoreNest <= nest) return BlockParseResult.Ignored(BlockParseResult.NestOperationMode.Begin);

                bool matches = Matches();
                HasMatched.Add(nest + 1, matches);
                return BlockParseResult.Effective(BlockParseResult.NestOperationMode.Begin, !matches);
            }
            else if (statement.FilterMatches(RootFilter, ElseIfFilter))
            {
                if (ignoreNest < nest) return BlockParseResult.Ignored(BlockParseResult.NestOperationMode.Continue);

                if (!HasMatched.ContainsKey(nest)) throw new SyntaxException(statement);
                if (HasMatched[nest]) return BlockParseResult.Effective(BlockParseResult.NestOperationMode.Continue, true);

                bool matches = Matches();
                HasMatched[nest] = matches;
                return BlockParseResult.Effective(BlockParseResult.NestOperationMode.Continue, !matches);
            }
            else if (statement.FilterMatches(RootFilter, ElseFilter))
            {
                if (ignoreNest < nest) return BlockParseResult.Ignored(BlockParseResult.NestOperationMode.Continue);

                if (!HasMatched.ContainsKey(nest)) throw new SyntaxException(statement);

                bool hasMatched = HasMatched[nest];
                return BlockParseResult.Effective(BlockParseResult.NestOperationMode.Continue, hasMatched);
            }
            else if (statement.FilterMatches(RootFilter, EndFilter))
            {
                if (ignoreNest < nest) return BlockParseResult.Ignored(BlockParseResult.NestOperationMode.End);

                if (!HasMatched.Remove(nest)) throw new SyntaxException(statement);

                return BlockParseResult.Effective(BlockParseResult.NestOperationMode.End, false);
            }
            else
            {
                throw new SyntaxException(statement);
            }


            bool Matches()
            {
                try
                {
                    List<object> statementArgs = clauses[2].Args;
                    return Condition.Parse(statementArgs);
                }
                catch
                {
                    throw new SyntaxException(statement);
                }
            }
        }
    }
}
