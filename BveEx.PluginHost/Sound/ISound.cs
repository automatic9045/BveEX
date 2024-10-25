using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Sound
{
    /// <summary>
    /// 再生・ループ再生・停止が可能なサウンドを表します。
    /// </summary>
    public interface ISound
    {
        /// <summary>
        /// サウンドの再生状態を取得します。
        /// </summary>
        PlayState PlayState { get; }

        /// <summary>
        /// サウンドを再生します。
        /// </summary>
        void Play();

        /// <summary>
        /// サウンドをループ再生します。
        /// </summary>
        void PlayLoop();

        /// <summary>
        /// サウンドの再生を停止します。
        /// </summary>
        void Stop();

        /// <summary>
        /// サウンドの再生を一度停止し、その後再度再生します。
        /// </summary>
        void StopAndPlay();
    }
}
