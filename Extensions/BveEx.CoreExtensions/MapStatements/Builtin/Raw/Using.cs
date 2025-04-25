using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Irony.Parsing;
using UnembeddedResources;

using BveEx.Extensions.MapStatements.Parsing;

namespace BveEx.Extensions.MapStatements.Builtin.Raw
{
    internal class Using : RawParserBase
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<Using>("CoreExtensions");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> InvalidFormat { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();
        private static readonly Dictionary<string, IReadOnlyList<string>> Empty = new Dictionary<string, IReadOnlyList<string>>(0);

        static Using()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


        private readonly Dictionary<Uri, Dictionary<string, IReadOnlyList<string>>> UsingChains = new Dictionary<Uri, Dictionary<string, IReadOnlyList<string>>>();

        public override void ConstructGrammar(Grammar grammar, Parsing.TerminalSet terminals)
        {
            NonTerminal statementElementChain = new NonTerminal("StatementElementChain");
            statementElementChain.Rule = grammar.MakePlusRule(statementElementChain, grammar.ToTerm("."), terminals.Identifier);

            NonTerminal exUsing = new NonTerminal("ExUsing")
            {
                Rule = "ex_using" + terminals.Identifier + "=" + statementElementChain + ";",
            };

            terminals.Statement.Rule |= exUsing;
        }

        public override bool TryParseNode(MapParser parser, ParseTreeNode node)
        {
            switch (node.Term.Name)
            {
                case "ExUsing":
                    string alias = node.ChildNodes[1].Token.Text.ToLowerInvariant();
                    string[] original = node.ChildNodes[3].ChildNodes.Select(x => x.Token.Text).ToArray();

                    if (alias[0] != '_')
                    {
                        throw new FormatException(string.Format(Resources.Value.InvalidFormat.Value, alias));
                    }

                    Uri mapUri = new Uri(parser.FilePath, UriKind.Absolute);
                    if (!UsingChains.TryGetValue(mapUri, out Dictionary<string, IReadOnlyList<string>> item))
                    {
                        item = new Dictionary<string, IReadOnlyList<string>>();
                        UsingChains[mapUri] = item;
                    }

                    item[alias] = original;
                    return true;

                default:
                    return false;
            }
        }

        public IReadOnlyDictionary<string, IReadOnlyList<string>> GetUsingChains(Uri mapUri)
        {
            UsingChains.TryGetValue(mapUri, out Dictionary<string, IReadOnlyList<string>> item);
            return item ?? Empty;
        }
    }
}
