using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
    internal static class HeaderParser
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

        static HeaderParser()
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

        public static HeaderInfo Parse(string text, string filePath, int lineIndex, int charIndex)
        {
            if (TryCreateHeader(UseAtsExHeaderFullName, UseAtsExHeaderFullNameObsolete) is Header useAtsExHeader)
            {
                return new HeaderInfo(HeaderType.UseAtsEx, useAtsExHeader);
            }
            else if (TryCreateHeader(NoMapPluginHeaderFullName) is Header noMapPluginHeader)
            {
                return new HeaderInfo(HeaderType.NoMapPlugin, noMapPluginHeader);
            }
            else if (TryCreateHeader(ReadDepthHeaderFullName, ReadDepthHeaderFullNameObsolete) is Header readDepthHeader)
            {
                return new HeaderInfo(HeaderType.ReadDepth, readDepthHeader);
            }

            int headerCloseBracketIndex = text.IndexOf(HeaderNameCloseBracket);
            if (text.StartsWith(HeaderNameOpenBracket) && headerCloseBracketIndex != -1)
            {
                string headerFullName = text.Substring(HeaderNameOpenBracket.Length, headerCloseBracketIndex - HeaderNameOpenBracket.Length);
                string headerArgument = text.Substring(headerCloseBracketIndex + HeaderNameCloseBracket.Length);

                Identifier identifier = Identifier.Parse(headerFullName);
                Header header = new Header(identifier, headerArgument, filePath, lineIndex, charIndex);

                return header.Name.Namespace is null || !header.Name.Namespace.IsChildOf(Namespace.Root)
                    ? new HeaderInfo(HeaderType.Invalid, null)
                    : new HeaderInfo(HeaderType.Public, header);
            }

            return new HeaderInfo(HeaderType.Invalid, null);


            Header TryCreateHeader(params string[] fullNames)
            {
                foreach (string fullName in fullNames)
                {
                    if (!text.StartsWith(fullName)) continue;

                    string headerArgument = text.Substring(fullName.Length);
                    Header header = new Header(UseAtsExHeader, headerArgument, filePath, lineIndex, charIndex);
                    return header;
                }

                return null;
            }
        }


        internal struct HeaderInfo
        {
            public HeaderType Type { get; }
            public Header Header { get; }

            public HeaderInfo(HeaderType type, Header header)
            {
                Type = type;
                Header = header;
            }
        }

        internal enum HeaderType
        {
            Invalid,
            Public,
            UseAtsEx,
            NoMapPlugin,
            ReadDepth,
        }
    }
}
