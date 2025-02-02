using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Plugins
{
    internal class MapPluginFactory
    {
        private readonly PluginLoadErrorResolver LoadErrorResolver;
        private readonly PluginLoader PluginLoader;

        public MapPluginFactory(BveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins)
        {
            LoadErrorResolver = new PluginLoadErrorResolver(bveHacker.LoadingProgressForm);
            PluginLoader = new PluginLoader(bveHacker, extensions, plugins);
        }

        public Dictionary<string, PluginBase> LoadPluginUsing(string path)
        {
            PluginSourceSet pluginSources;
            try
            {
                pluginSources = PluginSourceSet.FromPluginUsing(PluginType.MapPlugin, false, path);
            }
            catch (Exception ex)
            {
                pluginSources = new PluginSourceSet(path, PluginType.MapPlugin, false, new IPluginPackage[0]);
                LoadErrorResolver.Resolve(null, ex);
            }

            return Load(pluginSources);
        }

        public Dictionary<string, PluginBase> LoadAssembly(string path, Identifier identifier)
        {
            PluginSourceSet pluginSources;
            try
            {
                pluginSources = new PluginSourceSet(Path.GetFileName(path), PluginType.MapPlugin, false, new IPluginPackage[]
                {
                    new AssemblyPluginPackage(identifier, path, Assembly.LoadFrom(path)),
                });
            }
            catch (Exception ex)
            {
                pluginSources = new PluginSourceSet(path, PluginType.MapPlugin, false, new IPluginPackage[0]);
                LoadErrorResolver.Resolve(null, ex);
            }

            return Load(pluginSources);
        }

        public Dictionary<string, PluginBase> Load(PluginSourceSet pluginSources)
        {
            Dictionary<string, PluginBase> items;
            try
            {
                items = PluginLoader.Load(pluginSources);
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
