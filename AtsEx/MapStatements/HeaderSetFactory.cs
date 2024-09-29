using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
    internal class HeaderSetFactory
    {
        private readonly ConcurrentDictionary<Identifier, IReadOnlyList<Header>> PublicHeaders = new ConcurrentDictionary<Identifier, IReadOnlyList<Header>>();
        private readonly List<Header> PrivateHeaders = new List<Header>();

        public bool Register(string text, string filePath, int lineIndex, int charIndex)
        {
            HeaderParser.HeaderInfo headerInfo = HeaderParser.Parse(text, filePath, lineIndex, charIndex);
            switch (headerInfo.Type)
            {
                case HeaderParser.HeaderType.Invalid:
                    return false;

                case HeaderParser.HeaderType.Public:
                    List<Header> list = (List<Header>)PublicHeaders.GetOrAdd(headerInfo.Header.Name, new List<Header>());
                    list.Add(headerInfo.Header);
                    return true;

                case HeaderParser.HeaderType.UseAtsEx:
                case HeaderParser.HeaderType.NoMapPlugin:
                case HeaderParser.HeaderType.ReadDepth:
                    PrivateHeaders.Add(headerInfo.Header);
                    return true;

                default:
                    throw new NotSupportedException();
            }
        }

        public HeaderSet Build()
        {
            return new HeaderSet(PublicHeaders, PrivateHeaders);
        }
    }
}
