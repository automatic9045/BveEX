using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.PluginHost.Plugins;

namespace AtsEx.PluginHost
{
    public sealed class App
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<App>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ProductName { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ProductShortName { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        public static App Instance { get; private set; }

        public static bool IsInitialized { get; private set; } = false;

        static App()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private App(Process targetProcess, Assembly bveAssembly, Assembly launcherAssembly, Assembly atsExAssembly, LaunchMode launchMode)
        {
            Process = targetProcess;
            BveAssembly = bveAssembly;
            AtsExLauncherAssembly = launcherAssembly;
            AtsExAssembly = atsExAssembly;
            AtsExPluginHostAssembly = Assembly.GetExecutingAssembly();

            AtsExVersion = AtsExAssembly.GetName().Version;
            BveVersion = BveAssembly.GetName().Version;

            LaunchMode = launchMode;
        }

        public static void CreateInstance(Process targetProcess, Assembly bveAssembly, Assembly launcherAssembly, Assembly atsExAssembly, LaunchMode launchMode)
        {
            Instance = new App(targetProcess, bveAssembly, launcherAssembly, atsExAssembly, launchMode);
            IsInitialized = true;
        }

        /// <summary>
        /// プロダクト名を取得します。
        /// </summary>
        public string ProductName { get; } = Resources.Value.ProductName.Value;

        /// <summary>
        /// 短いプロダクト名を取得します。
        /// </summary>
        public string ProductShortName { get; } = Resources.Value.ProductShortName.Value;


        /// <summary>
        /// 制御対象の BVE を実行している <see cref="System.Diagnostics.Process"/> を取得します。
        /// </summary>
        public Process Process { get; }

        /// <summary>
        /// 実行元の BVE の <see cref="Assembly"/> を取得します。
        /// </summary>
        public Assembly BveAssembly { get; }

        /// <summary>
        /// AtsEX Launcher の <see cref="Assembly"/> を取得します。
        /// </summary>
        public Assembly AtsExLauncherAssembly { get; }

        /// <summary>
        /// AtsEX の <see cref="Assembly"/> を取得します。
        /// </summary>
        public Assembly AtsExAssembly { get; }

        /// <summary>
        /// AtsEX プラグインホストの <see cref="Assembly"/> を取得します。
        /// </summary>
        public Assembly AtsExPluginHostAssembly { get; }

        /// <summary>
        /// AtsEX のバージョンを取得します。
        /// </summary>
        public Version AtsExVersion { get; }

        /// <summary>
        /// 実行元の BVE のバージョンを取得します。
        /// </summary>
        public Version BveVersion { get; }

        /// <summary>
        /// AtsEX 本体の呼出モードを取得します。
        /// </summary>
        public LaunchMode LaunchMode { get; }
    }
}
