using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.Plugins;
using AtsEx.PluginHost.MapStatements;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx
{
    internal partial class ScenarioService
    {
        private class PluginLoader
        {
            private readonly NativeImpl Native;
            private readonly BveHacker BveHacker;
            private readonly IExtensionSet Extensions;

            public PluginLoader(NativeImpl native, BveHacker bveHacker, IExtensionSet extensions)
            {
                Native = native;
                BveHacker = bveHacker;
                Extensions = extensions;
            }

            public PluginSet Load(PluginSourceSet vehiclePluginUsing)
            {
                PluginLoadErrorResolver loadErrorResolver = new PluginLoadErrorResolver(BveHacker.LoadErrorManager);

                PluginSet loadedPlugins = new PluginSet();
                Plugins.PluginLoader pluginLoader = new Plugins.PluginLoader(Native, BveHacker, Extensions, loadedPlugins);

                Dictionary<string, PluginBase> vehiclePlugins;
                try
                {
                    vehiclePlugins = pluginLoader.Load(vehiclePluginUsing);
                }
                catch (Exception ex)
                {
                    vehiclePlugins = new Dictionary<string, PluginBase>();
                    loadErrorResolver.Resolve(null, ex);
                }

                Dictionary<string, PluginBase> mapPlugins = new Dictionary<string, PluginBase>();
                try
                {
                    PluginHost.MapStatements.Identifier mapPluginUsingIdentifier = new PluginHost.MapStatements.Identifier(Namespace.Root, "mappluginusing");
                    IEnumerable<IHeader> mapPluginUsingHeaders = BveHacker.MapHeaders.GetAll(mapPluginUsingIdentifier);

                    foreach (IHeader header in mapPluginUsingHeaders)
                    {
                        string mapPluginUsingPath = Path.Combine(Path.GetDirectoryName(BveHacker.ScenarioInfo.RouteFiles.SelectedFile.Path), header.Argument);
                        LoadMapPluginUsing(mapPluginUsingPath);
                    }


                    void LoadMapPluginUsing(string path)
                    {
                        PluginSourceSet mapPluginUsing = PluginSourceSet.FromPluginUsing(PluginType.MapPlugin, false, path);
                        Dictionary<string, PluginBase> plugins = pluginLoader.Load(mapPluginUsing);

                        foreach (KeyValuePair<string, PluginBase> plugin in plugins)
                        {
                            mapPlugins.Add(plugin.Key, plugin.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    loadErrorResolver.Resolve(null, ex);
                }

                loadedPlugins.SetPlugins(vehiclePlugins, mapPlugins);
                return loadedPlugins;
            }
        }
    }
}
