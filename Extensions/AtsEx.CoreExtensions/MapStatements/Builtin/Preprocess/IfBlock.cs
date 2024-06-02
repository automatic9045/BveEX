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
        private static readonly ClauseFilter RootFilter = new ClauseFilter("If", ClauseType.Element);

        private static readonly ClauseFilter BeginIfFilter = new ClauseFilter("BeginIf", ClauseType.Function);
        private static readonly ClauseFilter ElseIfFilter = new ClauseFilter("ElseIf", ClauseType.Function);
        private static readonly ClauseFilter ElseFilter = new ClauseFilter("Else", ClauseType.Function);
        private static readonly ClauseFilter EndFilter = new ClauseFilter("End", ClauseType.Function);

        private bool IsIn = false;
        private bool HasMatched = false;
        private bool Match = false;

        private IfBlock Child = null;

        public bool IgnoreStatement
        {
            get
            {
                if (!(Child is null)) return Child.IgnoreStatement;
                if (IsIn) return !Match;
                return false;
            }
        }

        private event EventHandler BlockFinished;

        public bool CanParse(Statement statement) => statement.FilterMatches(RootFilter);

        public void Parse(Statement statement)
        {
            if (!(Child is null))
            {
                Child.Parse(statement);
                return;
            }

            IReadOnlyList<MapStatementClause> clauses = statement.Source.Clauses;
            if (clauses.Count != 3) throw new SyntaxException(statement);
            if (clauses[0].Keys.Count != 0) throw new SyntaxException(statement);
            if (clauses[1].Keys.Count != 0) throw new SyntaxException(statement);
            if (clauses[2].Keys.Count != 0) throw new SyntaxException(statement);

            if (statement.FilterMatches(RootFilter, BeginIfFilter))
            {
                if (IsIn)
                {
                    Child = new IfBlock();
                    Child.BlockFinished += (sender, e) => Child = null;
                }
                else
                {
                    IsIn = true;
                    HasMatched = false;
                    SetMatch();
                }
            }
            else if (statement.FilterMatches(RootFilter, ElseIfFilter))
            {
                if (!IsIn) throw new SyntaxException(statement);

                if (HasMatched)
                {
                    Match = false;
                }
                else
                {
                    SetMatch();
                }
            }
            else if (statement.FilterMatches(RootFilter, ElseFilter))
            {
                if (!IsIn) throw new SyntaxException(statement);

                Match = !HasMatched;
            }
            else if (statement.FilterMatches(RootFilter, EndFilter))
            {
                if (!IsIn) throw new SyntaxException(statement);

                IsIn = false;
            }
            else
            {
                throw new SyntaxException(statement);
            }


            void SetMatch()
            {
                try
                {
                    List<object> statementArgs = clauses[2].Args;
                    Match = Condition.Parse(statementArgs);
                    HasMatched |= Match;
                }
                catch
                {
                    throw new SyntaxException(statement);
                }
            }
        }
    }
}
