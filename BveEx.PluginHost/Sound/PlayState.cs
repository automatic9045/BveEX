using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Sound
{
    /// <summary>
    /// サウンドの再生状態を表します。
    /// </summary>
    public enum PlayState
    {
        /// <summary>
        /// サウンドの再生が停止されていることを表します。
        /// </summary>
        Stop,

        /// <summary>
        /// サウンドが 1 回再生中であることを表します。
        /// </summary>
        Playing,

        /// <summary>
        /// サウンドがループ再生中であることを表します。
        /// </summary>
        PlayingLoop,
    }
}
