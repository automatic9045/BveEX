using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Irony.Parsing;

namespace BveEx.BveHackerServices
{
    internal class ExMapParser
    {
        private readonly FieldInfo DataField;

        private double IncludeLocation = 0;
        private readonly List<object> IncludeArgs = new List<object>(8);

        public ExMapParser()
        {
            DataField = typeof(BnfExpression)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
                .First(field => field.FieldType.IsSubclassOf(typeof(List<BnfTermList>)));
        }

        private IEnumerable<BnfTerm> GetExpressionData(BnfExpression instance)
        {
            List<BnfTermList> terms = DataField.GetValue(instance) as List<BnfTermList>;
            return terms.SelectMany(x => x);
        }

        public void ConstructGrammar(Grammar grammar)
        {
            grammar.RegisterOperators(5, "*", "/", "%");
            grammar.RegisterOperators(4, "+", "-");
            grammar.RegisterOperators(3, "<", ">", "<=", ">=");
            grammar.RegisterOperators(2, "==", "!=");
            grammar.RegisterOperators(1, "&&");
            grammar.RegisterOperators(0, "||");


            NonTerminal statementList = grammar.Root;

            IEnumerable<BnfTerm> statementListRules = GetExpressionData(statementList.Rule);
            NonTerminal statement = statementListRules.First(x => x.Name == "Statement") as NonTerminal;

            IEnumerable<BnfTerm> statementRules = GetExpressionData(statement.Rule);
            NonTerminal expression = statementRules.First(x => x.Name == "Expression") as NonTerminal;

            IEnumerable<BnfTerm> expressionRules = GetExpressionData(expression.Rule);
            NonTerminal operatorExpression = expressionRules.First(x => x.Name == "OperatorExpression") as NonTerminal;
            NonTerminal unaryExpression = expressionRules.First(x => x.Name == "UnaryExpression") as NonTerminal;

            IEnumerable<BnfTerm> operatorExpressionRules = GetExpressionData(operatorExpression.Rule);
            NonTerminal @operator = operatorExpressionRules.First(x => x.Name == "Operator") as NonTerminal;

            IEnumerable<BnfTerm> unaryExpressionRules = GetExpressionData(unaryExpression.Rule);
            NonTerminal unaryOperator = unaryExpressionRules.First(x => x.Name == "UnaryOperator") as NonTerminal;


            @operator.Rule |= grammar.ToTerm("<") | ">" | "<=" | ">=" | "==" | "!=" | "&&" | "||";
            unaryOperator.Rule |= grammar.ToTerm("!");


            NonTerminal statementBlock = new NonTerminal("StatementBlock");

            NonTerminal exIf = new NonTerminal("ExIf");
            NonTerminal exIfBlock = new NonTerminal("ExIfBlock");
            NonTerminal exElseIfBlockList = new NonTerminal("ExElseIfBlockList");
            NonTerminal exElseIfBlock = new NonTerminal("ExElseIfBlock");
            NonTerminal exElseBlock = new NonTerminal("ExElseBlock");

            NonTerminal exInclude = new NonTerminal("ExInclude");
            NonTerminal exIncludeArgList = new NonTerminal("ExIncludeArgList");

            statementBlock.Rule = statement | ("{" + statementList + "}") | ("{" + "}");

            exIf.Rule = (exIfBlock + exElseIfBlockList + exElseBlock) | (exIfBlock + exElseIfBlockList);
            exIfBlock.Rule = grammar.ToTerm("ex_if") + "(" + expression + ")" + statementBlock;
            exElseIfBlockList.Rule = grammar.MakeStarRule(exElseIfBlockList, null, exElseIfBlock);
            exElseIfBlock.Rule = grammar.ToTerm("ex_elif") + "(" + expression + ")" + statementBlock;
            exElseBlock.Rule = "ex_else" + statementBlock;

            exInclude.Rule = "ex_include" + exIncludeArgList + ";";
            exIncludeArgList.Rule = grammar.MakePlusRule(exIncludeArgList, grammar.ToTerm(","), expression);


            statement.Rule |= exIf | exInclude;
        }

        private bool ToBool(object value)
        {
            switch (value)
            {
                case string stringValue:
                    return !(stringValue is null);

                case double doubleValue:
                    return doubleValue != 0;

                case int intValue:
                    return intValue != 0;

                default:
                    throw new NotSupportedException();
            }
        }

        public void ParseNode(MapParser parser, ParseTreeNode node)
        {
            try
            {
                switch (node.Term.Name)
                {
                    case "IncludeStatement":
                        parser.Include(node);
                        break;

                    case "VariableDeclarator":
                        parser.SetUserVariable(node);
                        break;

                    case "ExIf":
                        if (!ParseIfBlock(node.ChildNodes[0]))
                        {
                            bool isAnyTrue = false;
                            IEnumerable<ParseTreeNode> elseIfBlocks = node.ChildNodes[1].ChildNodes;
                            foreach (ParseTreeNode elseIfBlock in elseIfBlocks)
                            {
                                if (ParseIfBlock(elseIfBlock))
                                {
                                    isAnyTrue = true;
                                    break;
                                }
                            }

                            if (!isAnyTrue && 3 <= node.ChildNodes.Count)
                            {
                                ParseStatementBlock(node.ChildNodes[2].ChildNodes[1]);
                            }
                        }
                        break;

                    case "ExInclude":
                        IncludeLocation = parser.Location;

                        IncludeArgs.Clear();
                        foreach (ParseTreeNode argNode in node.ChildNodes[1].ChildNodes)
                        {
                            object value = parser.GetValue(argNode);
                            IncludeArgs.Add(value);
                        }

                        Uri baseUri = new Uri(Path.GetDirectoryName(((MapLoader)parser.Parent).FilePath) + Path.DirectorySeparatorChar);
                        string relativePath = Convert.ToString(IncludeArgs[0]);
                        Uri uri = new Uri(baseUri, relativePath);
                        IncludeArgs[0] = uri.LocalPath;
                        parser.Parent.Include(uri.LocalPath);
                        break;

                    case "SYNTAX_ERROR":
                        break;

                    default:
                        parser.SetLocation(node);
                        break;
                }
            }
            catch (Exception ex)
            {
                ParseTreeNode item = node;

                int lineIndex = 0;
                int columnIndex = 0;
                while (true)
                {
                    if (item.Token is null)
                    {
                        if (item.ChildNodes.Count <= 0) break;
                    }
                    else
                    {
                        lineIndex = item.Token.Location.Line + 2;
                        columnIndex = item.Token.Location.Column + 1;
                        break;
                    }

                    item = item.ChildNodes[0];
                }

                LoadError error = new LoadError(ex.Message, parser.FilePath, lineIndex, columnIndex);
                parser.Parent.ThrowError(error);
            }


            bool ParseIfBlock(ParseTreeNode blockNode)
            {
                object condition = parser.GetValue(blockNode.ChildNodes[1]);
                if (ToBool(condition))
                {
                    ParseStatementBlock(blockNode.ChildNodes[2]);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            void ParseStatementBlock(ParseTreeNode blockNode)
            {
                ParseTreeNode child = blockNode.ChildNodes[0];
                switch (child.Term.Name)
                {
                    case "StatementList":
                        foreach (ParseTreeNode statement in child.ChildNodes)
                        {
                            ParseNode(parser, statement);
                        }
                        break;

                    default:
                        ParseNode(parser, child);
                        break;
                }
            }
        }

        public bool TryGetSystemVariable(MapParser parser, string key, out object result)
        {
            if (key.StartsWith("ex_arg", StringComparison.OrdinalIgnoreCase))
            {
                if (key == "ex_argdistance")
                {
                    result = IncludeLocation;
                    return true;
                }

                string indexText = key.Substring(6);
                if (int.TryParse(indexText, out int index) && 0 <= index)
                {
                    result = index < IncludeArgs.Count ? IncludeArgs[index] : 0;
                    return true;
                }
            }

            switch (key)
            {
                case "ex_relativedir":
                    Uri rootUri = new Uri(Path.GetDirectoryName(((MapLoader)parser.Parent).FilePath) + Path.DirectorySeparatorChar);
                    Uri subUri = new Uri(Path.GetDirectoryName(parser.FilePath) + Path.DirectorySeparatorChar);

                    Uri relativeUri = rootUri.MakeRelativeUri(subUri);
                    string relativePath = relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar);

                    result = relativePath;
                    return true;

                default:
                    result = default;
                    return false;
            }
        }

        public bool TryCalculateSign(object value, string operatorName, out object result)
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

        public bool TryCalculateOperator(object left, object right, string operatorName, out object result)
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
