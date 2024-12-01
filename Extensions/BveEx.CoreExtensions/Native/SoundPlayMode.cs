using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.Native
{
    /// <summary>
    /// ATS サウンドの再生モードを指定します。
    /// </summary>
    public enum SoundPlayMode
    {
        /// <summary>
        /// サウンドの再生を停止することを指定します。
        /// </summary>
        Stop = -10000,

        /// <summary>
        /// サウンドをループ再生することを指定します。
        /// </summary>
        PlayLooping = 0,

        /// <summary>
        /// サウンドを 1 回再生することを指定します。
        /// </summary>
        Play = 1,

        /// <summary>
        /// 前フレームでの再生モードを維持することを指定します。
        /// </summary>
        Continue = 2,
    }
}
