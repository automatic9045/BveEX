using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
    internal partial class HeaderSet
    {
        public static HeaderSet FromMap(string filePath)
        {
            (IDictionary<Identifier, IReadOnlyList<Header>> headers, IReadOnlyList<Header> privateHeaders) = Load(filePath, 0);
            return new HeaderSet(headers, privateHeaders);
        }

        private static (IDictionary<Identifier, IReadOnlyList<Header>> Headers, IReadOnlyList<Header> PrivateHeaders) Load(string filePath, int readDepth)
        {
            ConcurrentDictionary<Identifier, IReadOnlyList<Header>> headers = new ConcurrentDictionary<Identifier, IReadOnlyList<Header>>();
            List<Header> privateHeaders = new List<Header>();

            string text;
            using (StreamReader sr = new StreamReader(filePath))
            {
                text = sr.ReadToEnd();
            }

            bool useAtsEx = false;
            IEnumerable<MapTextParser.TextWithPosition> statements = MapTextParser.GetStatementsFromText(text);
            int i = -1;
            foreach (MapTextParser.TextWithPosition s in statements)
            {
                i++;
                if (!useAtsEx && 10 <= i) break;

                if (s.Text.StartsWith("include'") && s.Text.EndsWith("'") && s.Text.Length - s.Text.Replace("'", "").Length == 2)
                {
                    string includePath = s.Text.Split('\'')[1];
                    HeaderParser.HeaderInfo headerInfo = HeaderParser.Parse(includePath, filePath, s.LineIndex, s.CharIndex);
                    switch (headerInfo.Type)
                    {
                        case HeaderParser.HeaderType.Invalid:
                        {
                            if (!useAtsEx) return (headers, privateHeaders);
                            if (readDepth <= 0) break;

                            string includeRelativePath = includePath;
                            string includeAbsolutePath = Path.Combine(Path.GetDirectoryName(filePath), includeRelativePath);

                            if (!File.Exists(includeAbsolutePath)) continue;

                            (IDictionary<Identifier, IReadOnlyList<Header>> headersInIncludedMap, IReadOnlyList<Header> privateHeadersInIncludedMap) = Load(includeAbsolutePath, readDepth - 1);

                            foreach (KeyValuePair<Identifier, IReadOnlyList<Header>> pair in headersInIncludedMap)
                            {
                                List<Header> list = headers.GetOrAdd(pair.Key, new List<Header>()) as List<Header>;
                                list.AddRange(pair.Value);
                            }

                            privateHeaders.AddRange(privateHeadersInIncludedMap);
                            break;
                        }

                        case HeaderParser.HeaderType.Public:
                        {
                            if (!useAtsEx) return (headers, privateHeaders);

                            List<Header> list = headers.GetOrAdd(headerInfo.Header.Name, new List<Header>()) as List<Header>;
                            list.Add(headerInfo.Header);
                            break;
                        }

                        case HeaderParser.HeaderType.UseAtsEx:
                        case HeaderParser.HeaderType.NoMapPlugin:
                        {
                            privateHeaders.Add(headerInfo.Header);
                            useAtsEx = true;
                            break;
                        }

                        case HeaderParser.HeaderType.ReadDepth:
                        {
                            privateHeaders.Add(headerInfo.Header);
                            int.TryParse(headerInfo.Header.Argument, out readDepth);
                            break;
                        }
                    }
                }
            }

            return (headers, privateHeaders);
        }
    }
}
