using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Samples.MapPlugins.StationController
{
    internal sealed class InstanceStore
    {
        public static InstanceStore Instance { get; private set; } = null;
        public static bool IsInitialized => !(Instance is null);

        public static void Initialize(IExtensionSet extensions, IBveHacker bveHacker)
        {
            Instance = new InstanceStore(extensions, bveHacker);
        }


        public IExtensionSet Extensions { get; }
        public IBveHacker BveHacker { get; }

        private InstanceStore(IExtensionSet extensions, IBveHacker bveHacker)
        {
            Extensions = extensions;
            BveHacker = bveHacker;
        }
    }
}
