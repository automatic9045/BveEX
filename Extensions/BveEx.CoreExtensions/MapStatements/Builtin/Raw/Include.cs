using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Irony.Parsing;

using BveEx.Extensions.MapStatements.Parsing;

namespace BveEx.Extensions.MapStatements.Builtin.Raw
{
    internal class Include : RawParserBase
    {
        private double IncludeLocation = 0;
        private readonly List<object> IncludeArgs = new List<object>(8);

        public override void ConstructGrammar(Grammar grammar, Parsing.TerminalSet terminals)
        {
            NonTerminal exInclude = new NonTerminal("ExInclude");
            NonTerminal exIncludeArgList = new NonTerminal("ExIncludeArgList");

            exInclude.Rule = "ex_include" + exIncludeArgList + ";";
            exIncludeArgList.Rule = grammar.MakePlusRule(exIncludeArgList, grammar.ToTerm(","), terminals.Expression);

            terminals.Statement.Rule |= exInclude;
        }

        public override bool TryParseNode(MapParser parser, ParseTreeNode node)
        {
            switch (node.Term.Name)
            {
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
                    return true;

                default:
                    return false;
            }
        }

        public override bool TryGetSystemVariable(MapParser parser, string key, out object result)
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
    }
}
