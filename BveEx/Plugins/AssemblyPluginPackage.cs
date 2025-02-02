using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Plugins
{
    internal class AssemblyPluginPackage : IPluginPackage
    {
        public Identifier Identifier { get; }
        public string Path { get; }
        public Assembly Assembly { get; }

        public AssemblyPluginPackage(Identifier identifier, string path, Assembly assembly)
        {
            Identifier = identifier;
            Path = path;
            Assembly = assembly;
        }
    }
}
