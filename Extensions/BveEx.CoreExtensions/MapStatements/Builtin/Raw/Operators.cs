using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Irony.Parsing;

using BveEx.Extensions.MapStatements.Parsing;
using System.Globalization;

namespace BveEx.Extensions.MapStatements.Builtin.Raw
{
    internal class Operators : RawParserBase
    {
        public override void ConstructGrammar(Grammar grammar, Parsing.TerminalSet terminals)
        {
            grammar.RegisterOperators(5, "*", "/", "%");
            grammar.RegisterOperators(4, "+", "-");
            grammar.RegisterOperators(3, "<", ">", "<=", ">=");
            grammar.RegisterOperators(2, "==", "!=");
            grammar.RegisterOperators(1, "&&");
            grammar.RegisterOperators(0, "||");

            terminals.Operator.Rule |= grammar.ToTerm("<") | ">" | "<=" | ">=" | "==" | "!=" | "&&" | "||";
            terminals.UnaryOperator.Rule |= grammar.ToTerm("!");
        }

        public override bool TryCalculateSign(object value, string operatorName, out object result)
        {
            switch (operatorName)
            {
                case "!":
                    result = !ToBool(value) ? 1 : 0;
                    return true;
            }

            result = null;
            return false;
        }

        public override bool TryCalculateOperator(object left, object right, string operatorName, out object result)
        {
            if ((left is int || left is double) && (right is int || right is double))
            {
                double leftDouble = Convert.ToDouble(left, CultureInfo.InvariantCulture);
                double rightDouble = Convert.ToDouble(right, CultureInfo.InvariantCulture);

                switch (operatorName)
                {
                    case "==":
                        result = leftDouble == rightDouble ? 1 : 0;
                        return true;
                    case "!=":
                        result = leftDouble != rightDouble ? 1 : 0;
                        return true;
                    case "&&":
                        result = ToBool(leftDouble) && ToBool(rightDouble) ? 1 : 0;
                        return true;
                    case "||":
                        result = ToBool(leftDouble) || ToBool(rightDouble) ? 1 : 0;
                        return true;
                    case "<":
                        result = leftDouble < rightDouble ? 1 : 0;
                        return true;
                    case ">":
                        result = leftDouble > rightDouble ? 1 : 0;
                        return true;
                    case "<=":
                        result = leftDouble <= rightDouble ? 1 : 0;
                        return true;
                    case ">=":
                        result = leftDouble >= rightDouble ? 1 : 0;
                        return true;
                }
            }
            else
            {
                switch (operatorName)
                {
                    case "==":
                        result = left == right ? 1 : 0;
                        return true;
                    case "!=":
                        result = left != right ? 1 : 0;
                        return true;
                    case "&&":
                        result = ToBool(left) && ToBool(right) ? 1 : 0;
                        return true;
                    case "||":
                        result = ToBool(left) || ToBool(right) ? 1 : 0;
                        return true;
                }
            }

            result = null;
            return false;
        }
    }
}
