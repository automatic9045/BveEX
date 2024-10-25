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
        internal INative Native { get; }
        internal IBveHacker BveHacker { get; }
        internal IExtensionSet Extensions { get; }
        internal IPluginSet Plugins { get; }
        internal string Identifier { get; }

        public PluginBuilder(INative native, IBveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins, string identifier)
        {
            Native = native;
            BveHacker = bveHacker;
            Extensions = extensions;
            Plugins = plugins;
            Identifier = identifier;
        }

        protected PluginBuilder(PluginBuilder pluginBuilder)
        {
            Native = pluginBuilder.Native;
            BveHacker = pluginBuilder.BveHacker;
            Extensions = pluginBuilder.Extensions;
            Plugins = pluginBuilder.Plugins;
            Identifier = pluginBuilder.Identifier;
        }
    }
}
