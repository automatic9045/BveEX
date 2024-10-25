using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher.Hosting
{
    internal class ReleaseInfo
    {
        public Version Version { get; }
        public Uri Url { get; }

        private readonly Func<string> UpdateDetailsFactory;

        public ReleaseInfo(Version version, Uri url, Func<string> updateDetailsFactory)
        {
            Version = version;
            Url = url;
            UpdateDetailsFactory = updateDetailsFactory;
        }

        public string GetUpdateDetails() => UpdateDetailsFactory();
    }
}
