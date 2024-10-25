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

        public static void Initialize(INative native, IBveHacker bveHacker)
        {
            Instance = new InstanceStore(native, bveHacker);
        }


        public INative Native { get; }
        public IBveHacker BveHacker { get; }

        private InstanceStore(INative native, IBveHacker bveHacker)
        {
            Native = native;
            BveHacker = bveHacker;
        }
    }
}
