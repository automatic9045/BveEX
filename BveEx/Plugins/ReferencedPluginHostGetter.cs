using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Plugins
{
    internal static class ReferencedPluginHostGetter
    {
        public static AssemblyName GetReferencedPluginHost(this Assembly assembly)
        {
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
            AssemblyName referencedPluginHost = referencedAssemblies.FirstOrDefault(asm => asm.Name == "BveEx.PluginHost");

            return referencedPluginHost;
        }
    }
}
