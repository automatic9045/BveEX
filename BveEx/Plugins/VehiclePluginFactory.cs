using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Plugins
{
    internal class VehiclePluginFactory
    {
        private readonly BveHacker BveHacker;
        private readonly IExtensionSet Extensions;
        private readonly IPluginSet LoadedPlugins;

        public VehiclePluginFactory(BveHacker bveHacker, IExtensionSet extensions, IPluginSet loadedPlugins)
        {
            BveHacker = bveHacker;
            Extensions = extensions;
            LoadedPlugins = loadedPlugins;
        }

        public Dictionary<string, PluginBase> Load(PluginSourceSet pluginUsing)
        {
            PluginLoadErrorResolver loadErrorResolver = new PluginLoadErrorResolver(BveHacker.LoadingProgressForm);
            PluginLoader pluginLoader = new PluginLoader(BveHacker, Extensions, LoadedPlugins);

            Dictionary<string, PluginBase> items;
            try
            {
                items = pluginLoader.Load(pluginUsing);
            }
            catch (Exception ex)
            {
                items = new Dictionary<string, PluginBase>();
                loadErrorResolver.Resolve(null, ex);
            }

            return items;
        }
    }
}
