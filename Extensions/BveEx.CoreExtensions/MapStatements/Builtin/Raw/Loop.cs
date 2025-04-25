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
    internal class Loop : RawParserBase
    {
        private readonly Action<MapParser, ParseTreeNode> ParseNode;

        public Loop(Action<MapParser, ParseTreeNode> parseNode)
        {
            ParseNode = parseNode;
        }

        public override void ConstructGrammar(Grammar grammar, Parsing.TerminalSet terminals)
        {
            NonTerminal exWhile = new NonTerminal("ExWhile")
            {
                Rule = grammar.ToTerm("ex_while") + "(" + terminals.Expression + ")" + terminals.StatementBlock,
            };

            terminals.Statement.Rule |= exWhile;
        }

        public override bool TryParseNode(MapParser parser, ParseTreeNode node)
        {
            switch (node.Term.Name)
            {
                case "ExWhile":
                    while (true)
                    {
                        object condition = parser.GetValue(node.ChildNodes[1]);
                        if (!ToBool(condition)) break;

                        ParseStatementBlock(node.ChildNodes[2]);
                    }
                    return true;

                default:
                    return false;
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
    }
}
