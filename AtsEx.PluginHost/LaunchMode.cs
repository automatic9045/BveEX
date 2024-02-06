using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// AtsEX 本体の呼出モードを表します。
    /// </summary>
    public enum LaunchMode
    {
        /// <summary>
        /// AtsEx.Caller から ATS プラグインとして起動されたことを表します。
        /// </summary>
        Ats,

        /// <summary>
        /// AtsEx.Caller.InputDevice から入力デバイスプラグインとして起動されたことを表します。
        /// </summary>
        InputDevice,
    }

    public static class LaunchModeConverter
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(LaunchModeConverter), "PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Ats { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> InputDevice { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static LaunchModeConverter()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public static Resource<string> GetTypeStringResource(this LaunchMode launchMode)
        {
            switch (launchMode)
            {
                case LaunchMode.Ats:
                    return Resources.Value.Ats;

                case LaunchMode.InputDevice:
                    return Resources.Value.InputDevice;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetTypeString(this LaunchMode launchMode) => GetTypeStringResource(launchMode).Value;
    }
}
