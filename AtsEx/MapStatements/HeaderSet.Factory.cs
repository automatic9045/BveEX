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
        private readonly static Identifier UseAtsExHeader;
        private readonly static string UseAtsExHeaderFullName;
        private readonly static string UseAtsExHeaderFullNameObsolete;

        private readonly static Identifier NoMapPluginHeader;
        private readonly static string NoMapPluginHeaderFullName;

        private readonly static Identifier ReadDepthHeader;
        private readonly static string ReadDepthHeaderFullName;
        private readonly static string ReadDepthHeaderFullNameObsolete;

        private const string HeaderNameOpenBracket = "<";
        private const string HeaderNameCloseBracket = ">";

        private const string VisibleHeaderNameOpenBracket = "[[";
        private const string VisibleHeaderNameCloseBracket = "]]";

        static HeaderSet()
        {
            NoMapPluginHeader = new Identifier(Namespace.Root, "nompi");
            NoMapPluginHeaderFullName = VisibleHeaderNameOpenBracket + NoMapPluginHeader.FullName + VisibleHeaderNameCloseBracket;

            UseAtsExHeader = new Identifier(Namespace.Root, "useatsex");
            UseAtsExHeaderFullName = HeaderNameOpenBracket + UseAtsExHeader.FullName + HeaderNameCloseBracket;
            UseAtsExHeaderFullNameObsolete = VisibleHeaderNameOpenBracket + UseAtsExHeader.FullName + VisibleHeaderNameCloseBracket;

            ReadDepthHeader = new Identifier(Namespace.Root, "readdepth");
            ReadDepthHeaderFullName = HeaderNameOpenBracket + ReadDepthHeader.FullName + HeaderNameCloseBracket;
            ReadDepthHeaderFullNameObsolete = VisibleHeaderNameOpenBracket + ReadDepthHeader.FullName + VisibleHeaderNameCloseBracket;
        }

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

            int includeStatementCount = 0;
            bool useAtsEx = false;
            IEnumerable<MapTextParser.TextWithPosition> statements = MapTextParser.GetStatementsFromText(text);
            int i = -1;
            foreach (MapTextParser.TextWithPosition s in statements)
            {
                i++;

                if (s.Text.StartsWith("include'") && s.Text.EndsWith("'") && s.Text.Length - s.Text.Replace("'", "").Length == 2)
                {
                    string includePath = s.Text.Split('\'')[1];

                    int headerCloseBracketIndex = includePath.IndexOf(HeaderNameCloseBracket);

                    if (TryCreateHeader(UseAtsExHeaderFullName, UseAtsExHeaderFullNameObsolete) is Header useAtsExHeader)
                    {
                        privateHeaders.Add(useAtsExHeader);
                        useAtsEx = true;
                    }
                    else if (TryCreateHeader(NoMapPluginHeaderFullName) is Header noMapPluginHeader)
                    {
                        privateHeaders.Add(noMapPluginHeader);
                        useAtsEx = true;
                    }
                    else if (TryCreateHeader(ReadDepthHeaderFullName, ReadDepthHeaderFullNameObsolete) is Header readDepthHeader)
                    {
                        privateHeaders.Add(readDepthHeader);
                        int.TryParse(readDepthHeader.Argument, out readDepth);
                    }
                    else
                    {
                        if (!useAtsEx) break;

                        if (includePath.StartsWith(HeaderNameOpenBracket) && headerCloseBracketIndex != -1)
                        {
                            string headerFullName = includePath.Substring(HeaderNameOpenBracket.Length, headerCloseBracketIndex - HeaderNameOpenBracket.Length);
                            string headerArgument = includePath.Substring(headerCloseBracketIndex + HeaderNameCloseBracket.Length);

                            Identifier identifier = Identifier.Parse(headerFullName);
                            Header header = new Header(identifier, headerArgument, filePath, s.LineIndex, s.CharIndex);
                            if (header.Name.Namespace is null || !header.Name.Namespace.IsChildOf(Namespace.Root)) continue;

                            List<Header> list = headers.GetOrAdd(identifier, new List<Header>()) as List<Header>;
                            list.Add(header);
                        }
                        else if (0 < readDepth)
                        {
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
                        }
                    }

                    includeStatementCount++;


                    Header TryCreateHeader(params string[] fullNames)
                    {
                        foreach (string fullName in fullNames)
                        {
                            if (!includePath.StartsWith(fullName)) continue;

                            string headerArgument = includePath.Substring(fullName.Length);
                            Header header = new Header(UseAtsExHeader, headerArgument, filePath, s.LineIndex, s.CharIndex);
                            return header;
                        }

                        return null;
                    }
                }

                if (!useAtsEx && 10 - 1 <= i) break;
            }

            return (headers, privateHeaders);
        }
    }
}
