using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins;

namespace BveEx.Plugins
{
    internal sealed partial class PluginSourceSet
    {
        public static PluginSourceSet FromDirectory(string name, PluginType pluginType, bool allowNonPluginAssembly, string directoryName)
        {
            List<IPluginPackage> assemblies =
                Directory.GetFiles(directoryName, "*.dll", SearchOption.AllDirectories)
                .Select(x => (IPluginPackage)new AssemblyPluginPackage(new RandomIdentifier(), Assembly.LoadFrom(x)))
                .ToList();

            return new PluginSourceSet(name, pluginType, allowNonPluginAssembly, assemblies);
        }
    }
}
