using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Native
{
    /// <summary>
    /// BveEX Launcher の互換性バージョンを取得します。
    /// </summary>
    public class LauncherCompatibilityVersionAttribute : Attribute
    {
        public int Version { get; }

        public LauncherCompatibilityVersionAttribute(int version)
        {
            Version = version;
        }
    }
}
