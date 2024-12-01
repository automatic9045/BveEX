using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// プラグインの種類を指定します。
    /// </summary>
    public enum PluginType
    {
        /// <summary>
        /// 拡張機能を表します。
        /// </summary>
        Extension,

        /// <summary>
        /// 車両プラグインを表します。
        /// </summary>
        VehiclePlugin,

        /// <summary>
        /// マッププラグインを表します。
        /// </summary>
        MapPlugin,
    }

    public static class PluginTypeConverter
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(PluginTypeConverter), "PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Extension { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> VehiclePlugin { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> MapPlugin { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static PluginTypeConverter()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public static Resource<string> GetTypeStringResource(this PluginType pluginType)
        {
            switch (pluginType)
            {
                case PluginType.Extension:
                    return Resources.Value.Extension;

                case PluginType.VehiclePlugin:
                    return Resources.Value.VehiclePlugin;

                case PluginType.MapPlugin:
                    return Resources.Value.MapPlugin;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetTypeString(this PluginType pluginType) => GetTypeStringResource(pluginType).Value;

        public static Color GetTypeColor(this PluginType pluginType)
        {
            switch (pluginType)
            {
                case PluginType.Extension:
                    return Color.White;

                case PluginType.VehiclePlugin:
                    return Color.LightBlue;

                case PluginType.MapPlugin:
                    return Color.LightGreen;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
