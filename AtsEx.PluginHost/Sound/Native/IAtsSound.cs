using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Sound.Native
{
    /// <summary>
    /// ATS プラグインから制御可能な ATS サウンドを表します。
    /// </summary>
    public interface IAtsSound : ISound, IDisposable
    {
        /// <summary>
        /// 音量を指定してサウンドをループ再生します。
        /// </summary>
        /// <param name="volumeDecibel">下げる音量の符号付き大きさ [dB]。0 または負の値で指定してください。</param>
        void PlayLoop(double volumeDecibel);

        /// <summary>
        /// 音量を指定してサウンドをループ再生します。
        /// </summary>
        /// <param name="volumeRatio">音源の本来の音量に対する、再生する音量の比。0 以上 1 以下の値で指定してください。</param>
        void PlayLoopRatio(double volumeRatio);
    }
}
