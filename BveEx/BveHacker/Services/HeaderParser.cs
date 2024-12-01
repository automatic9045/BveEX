using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.BveHackerServices
{
    internal static class HeaderParser
    {
        public static bool IsNoMapPluginHeader(string text)
        {
            return text.StartsWith("[[bveex::nompi]]", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsLegacyHeader(string text)
        {
            return IsHeader("[[", "]]") || IsHeader("<", ">");


            bool IsHeader(string startBracket, string endBracket)
            {
                return text.StartsWith(startBracket + "atsex::", StringComparison.OrdinalIgnoreCase) && text.Contains(endBracket);
            }
        }
    }
}
