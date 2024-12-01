using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.PluginHost.Plugins
{
    public class PluginBuilder
    {
        internal IBveHacker BveHacker { get; }
        internal IExtensionSet Extensions { get; }
        internal IPluginSet Plugins { get; }
        internal string Identifier { get; }

        public PluginBuilder(IBveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins, string identifier)
        {
            BveHacker = bveHacker;
            Extensions = extensions;
            Plugins = plugins;
            Identifier = identifier;
        }

        protected PluginBuilder(PluginBuilder pluginBuilder)
        {
            BveHacker = pluginBuilder.BveHacker;
            Extensions = pluginBuilder.Extensions;
            Plugins = pluginBuilder.Plugins;
            Identifier = pluginBuilder.Identifier;
        }
    }
}
