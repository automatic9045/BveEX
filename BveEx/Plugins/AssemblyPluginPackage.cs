using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Plugins
{
    internal class AssemblyPluginPackage : IPluginPackage
    {
        public Identifier Identifier { get; }
        public Assembly Assembly { get; }

        public AssemblyPluginPackage(Identifier identifier, Assembly assembly)
        {
            Identifier = identifier;
            Assembly = assembly;
        }
    }
}
