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
        private readonly PluginLoadErrorResolver LoadErrorResolver;
        private readonly PluginLoader PluginLoader;

        public VehiclePluginFactory(BveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins)
        {
            LoadErrorResolver = new PluginLoadErrorResolver(bveHacker.LoadingProgressForm);
            PluginLoader = new PluginLoader(bveHacker, extensions, plugins);
        }

        public Dictionary<string, PluginBase> Load(string vehiclePath)
        {
            Dictionary<string, PluginBase> items;
            try
            {
                PluginSourceSet pluginUsing = PluginSourceSet.ResolvePluginUsingToLoad(PluginType.VehiclePlugin, true, vehiclePath);
                items = PluginLoader.Load(pluginUsing);
            }
            catch (Exception ex)
            {
                items = new Dictionary<string, PluginBase>();
                LoadErrorResolver.Resolve(null, ex);
            }

            return items;
        }
    }
}
