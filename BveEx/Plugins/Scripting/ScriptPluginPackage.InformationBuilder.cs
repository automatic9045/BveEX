using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Plugins.Scripting
{
    internal partial class ScriptPluginPackage
    {
        internal sealed class InformationBuilder
        {
            public string Location { get; }
            public string Title { get; }
            public string Version { get; set; }
            public string Description { get; set; }
            public string Copyright { get; set; }

            public InformationBuilder(string location, string title)
            {
                Location = location;
                Title = title;
            }
        }
    }
}
