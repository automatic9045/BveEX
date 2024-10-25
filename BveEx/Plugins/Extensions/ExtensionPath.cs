using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;

namespace BveEx.Plugins.Extensions
{
    internal static class ExtensionPath
    {
        public static string GetRelativePath(string path)
        {
            Uri extensionDirecctory = new Uri(App.Instance.ExtensionDirectory + @"\", UriKind.Absolute);
            Uri uriPath = new Uri(path, UriKind.Absolute);

            string result = extensionDirecctory.MakeRelativeUri(uriPath).ToString();
            return result;
        }
    }
}
