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
    internal class If : RawParserBase
    {
        private readonly Action<MapParser, ParseTreeNode> ParseNode;

        public If(Action<MapParser, ParseTreeNode> parseNode)
        {
            ParseNode = parseNode;
        }

        public override void ConstructGrammar(Grammar grammar, Parsing.TerminalSet terminals)
        {
            NonTerminal exIf = new NonTerminal("ExIf");
            NonTerminal exIfBlock = new NonTerminal("ExIfBlock");
            NonTerminal exElseIfBlockList = new NonTerminal("ExElseIfBlockList");
            NonTerminal exElseIfBlock = new NonTerminal("ExElseIfBlock");
            NonTerminal exElseBlock = new NonTerminal("ExElseBlock");

            exIf.Rule = (exIfBlock + exElseIfBlockList + exElseBlock) | (exIfBlock + exElseIfBlockList);
            exIfBlock.Rule = grammar.ToTerm("ex_if") + "(" + terminals.Expression + ")" + terminals.StatementBlock;
            exElseIfBlockList.Rule = grammar.MakeStarRule(exElseIfBlockList, null, exElseIfBlock);
            exElseIfBlock.Rule = grammar.ToTerm("ex_elif") + "(" + terminals.Expression + ")" + terminals.StatementBlock;
            exElseBlock.Rule = "ex_else" + terminals.StatementBlock;

            terminals.Statement.Rule |= exIf;
        }

        public override bool TryParseNode(MapParser parser, ParseTreeNode node)
        {
            switch (node.Term.Name)
            {
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
                    return true;

                default:
                    return false;
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
    }
}
