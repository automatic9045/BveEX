using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Plugins.Scripting;

namespace AtsEx.Plugins
{
    internal sealed partial class PluginSourceSet
    {
        public PluginSourceSet Concat(string name, PluginSourceSet other)
        {
            if (PluginType != other.PluginType) throw new InvalidOperationException();

            List<IPluginPackage> newSet = new List<IPluginPackage>();
            newSet.AddRange(this);
            newSet.AddRange(other);

            return new PluginSourceSet(name, PluginType, AllowNonPluginAssembly, newSet);
        }
    }
}
