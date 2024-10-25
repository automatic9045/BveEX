using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Native
{
    /// <summary>
    /// 警笛の種類を指定します。
    /// </summary>
    public enum HornType
    {
        /// <summary>
        /// 警笛 1。
        /// </summary>
        Primary = 0,

        /// <summary>
        /// 警笛 2。
        /// </summary>
        Secondary = 1,

        /// <summary>
        /// ミュージックホーン。
        /// </summary>
        Music = 2,
    }
}
