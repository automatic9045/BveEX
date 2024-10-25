using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.BveHackerServices
{
    internal static class HeaderParser
    {
        public static bool IsNoMapPluginHeader(string text)
        {
            return text.StartsWith("[[atsex::nompi]]", StringComparison.OrdinalIgnoreCase);
        }
    }
}
