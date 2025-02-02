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
        internal string Location { get; }

        public PluginBuilder(IBveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins, string identifier, string location)
        {
            BveHacker = bveHacker;
            Extensions = extensions;
            Plugins = plugins;
            Identifier = identifier;
            Location = location;
        }

        protected PluginBuilder(PluginBuilder pluginBuilder)
        {
            BveHacker = pluginBuilder.BveHacker;
            Extensions = pluginBuilder.Extensions;
            Plugins = pluginBuilder.Plugins;
            Identifier = pluginBuilder.Identifier;
            Location = pluginBuilder.Location;
        }
    }
}
