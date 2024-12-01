using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;

namespace BveEx.Samples.VehiclePlugins.StateViewer
{
    internal sealed class InstanceStore
    {
        public static InstanceStore Instance { get; private set; } = null;
        public static bool IsInitialized => !(Instance is null);

        public static void Initialize(IBveHacker bveHacker)
        {
            Instance = new InstanceStore(bveHacker);
        }


        public IBveHacker BveHacker { get; }

        private InstanceStore(IBveHacker bveHacker)
        {
            BveHacker = bveHacker;
        }
    }
}
