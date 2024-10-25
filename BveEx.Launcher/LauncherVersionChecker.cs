using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Native;

namespace AtsEx.Launcher
{
    internal static class LauncherVersionChecker
    {
        private const int LauncherVersion = 3;

        public static void Check()
        {
            Assembly launcherAssembly = Assembly.GetExecutingAssembly();
            Assembly atsExAssembly = typeof(CallerInfo).Assembly;

            LauncherCompatibilityVersionAttribute compatibilityVersionAttribute = atsExAssembly.GetCustomAttribute<LauncherCompatibilityVersionAttribute>();
            if (compatibilityVersionAttribute.Version != LauncherVersion)
            {
                Version launcherAssemblyVersion = launcherAssembly.GetName().Version;
                throw new NotSupportedException($"読み込まれた AtsEX Launcher (バージョン {launcherAssemblyVersion}) は現在の AtsEX ではサポートされていません。");
            }

        }
    }
}
