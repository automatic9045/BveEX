using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace BveEx.Extensions.MapStatements.Parsing
{
    /// <summary>
    /// 既定のマップ構文 文法にて定義されている、代表的な <see cref="BnfTerm"/> のセットを表します。
    /// </summary>
    public class TerminalSet
    {
        private readonly FieldInfo DataField;

        public NonTerminal StatementList { get; }
        public NonTerminal Statement { get; }
        public NonTerminal Expression { get; }
        public NonTerminal OperatorExpression { get; }
        public NonTerminal PrimaryExpression { get; }
        public NonTerminal UnaryExpression { get; }
        public NonTerminal Operator { get; }
        public NonTerminal Member { get; }
        public IdentifierTerminal Identifier { get; }
        public NonTerminal UnaryOperator { get; }
        public NonTerminal StatementBlock { get; }

        internal TerminalSet(Grammar grammar)
        {
            DataField = typeof(BnfExpression)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
                .First(field => field.FieldType.IsSubclassOf(typeof(List<BnfTermList>)));

            StatementList = grammar.Root;

            IEnumerable<BnfTerm> statementListRules = GetExpressionData(StatementList.Rule);
            Statement = statementListRules.First(x => x.Name == "Statement") as NonTerminal;

            IEnumerable<BnfTerm> statementRules = GetExpressionData(Statement.Rule);
            Expression = statementRules.First(x => x.Name == "Expression") as NonTerminal;

            IEnumerable<BnfTerm> expressionRules = GetExpressionData(Expression.Rule);
            OperatorExpression = expressionRules.First(x => x.Name == "OperatorExpression") as NonTerminal;
            PrimaryExpression = expressionRules.First(x => x.Name == "PrimaryExpression") as NonTerminal;
            UnaryExpression = expressionRules.First(x => x.Name == "UnaryExpression") as NonTerminal;

            IEnumerable<BnfTerm> operatorExpressionRules = GetExpressionData(OperatorExpression.Rule);
            Operator = operatorExpressionRules.First(x => x.Name == "Operator") as NonTerminal;

            IEnumerable<BnfTerm> primaryExpressionRules = GetExpressionData(PrimaryExpression.Rule);
            Member = primaryExpressionRules.First(x => x.Name == "Member") as NonTerminal;

            IEnumerable<BnfTerm> memberRules = GetExpressionData(Member.Rule);
            Identifier = memberRules.First(x => x.Name == "Identifier") as IdentifierTerminal;

            IEnumerable<BnfTerm> unaryExpressionRules = GetExpressionData(UnaryExpression.Rule);
            UnaryOperator = unaryExpressionRules.First(x => x.Name == "UnaryOperator") as NonTerminal;

            StatementBlock = new NonTerminal("StatementBlock")
            {
                Rule = Statement | ("{" + StatementList + "}") | ("{" + "}")
            };
        }

        public IEnumerable<BnfTerm> GetExpressionData(BnfExpression instance)
        {
            List<BnfTermList> terms = DataField.GetValue(instance) as List<BnfTermList>;
            return terms.SelectMany(x => x);
        }
    }
}