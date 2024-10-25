using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveEx.Plugins;
using BveEx.Plugins.Extensions;
using BveEx.Plugins.Scripting;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Plugins.Extensions
{
    internal static class ExtensionSetFactory
    {
        public static ExtensionSet Load(BveHacker bveHacker)
        {
            ExtensionSet extensions = new ExtensionSet();
            PluginLoader pluginLoader = new PluginLoader(null, bveHacker, extensions, null);

            string extensionsDirectory = App.Instance.ExtensionDirectory;
            Directory.CreateDirectory(extensionsDirectory);

            string pluginUsingPath = Path.Combine(extensionsDirectory, "PluginUsing.xml");
            PluginSourceSet fromPluginUsing = File.Exists(pluginUsingPath)
                ? PluginSourceSet.FromPluginUsing(PluginType.Extension, true, pluginUsingPath) : PluginSourceSet.Empty(PluginType.Extension);
            PluginSourceSet fromDirectory = PluginSourceSet.FromDirectory(null, PluginType.Extension, true, extensionsDirectory);

            Dictionary<string, PluginBase> loadedExtensions = pluginLoader.Load(fromPluginUsing.Concat(null, fromDirectory));

            extensions.SetExtensions(loadedExtensions.Values);
            return extensions;
        }
    }
}
