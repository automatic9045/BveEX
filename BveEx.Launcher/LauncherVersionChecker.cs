using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveEx.Native;

namespace BveEx.Launcher
{
    internal static class LauncherVersionChecker
    {
        private const int LauncherVersion = 4;

        public static void Check()
        {
            Assembly launcherAssembly = Assembly.GetExecutingAssembly();
            Assembly bveExAssembly = typeof(CallerInfo).Assembly;

            LauncherCompatibilityVersionAttribute compatibilityVersionAttribute = bveExAssembly.GetCustomAttribute<LauncherCompatibilityVersionAttribute>();
            if (compatibilityVersionAttribute.Version != LauncherVersion)
            {
                Version launcherAssemblyVersion = launcherAssembly.GetName().Version;
                throw new NotSupportedException($"読み込まれた BveEX Launcher (バージョン {launcherAssemblyVersion}) は現在の BveEX ではサポートされていません。");
            }

        }
    }
}
