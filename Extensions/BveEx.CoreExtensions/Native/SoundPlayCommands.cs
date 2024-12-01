using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.Native
{
    /// <summary>
    /// ATS サウンドの再生コマンドに関する変換機能を提供します。
    /// </summary>
    public static class SoundPlayCommands
    {
        /// <summary>
        /// <see cref="SoundPlayMode"/> を再生コマンドに変換します。
        /// </summary>
        /// <param name="mode">変換元となる再生モード。</param>
        /// <returns>変換後のコマンド値。</returns>
        public static int ToCommand(this SoundPlayMode mode) => (int)mode;

        /// <summary>
        /// 音量を指定して、ループ再生を指示するための再生コマンドを作成します。
        /// </summary>
        /// <param name="volume">下げる音量の符号付き大きさ [B]。0 または負の値で指定してください。</param>
        /// <returns>作成されたコマンド値。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="volume"/> が 0 より大きいです。</exception>
        public static int PlayLooping(int volume) => 0 < volume ? throw new ArgumentOutOfRangeException(nameof(volume)) : volume;

        /// <summary>
        /// 再生コマンドの整数値から、再生モードを取得します。
        /// </summary>
        /// <param name="command">コマンド値。</param>
        /// <returns><paramref name="command"/> の再生モード。</returns>
        public static SoundPlayMode GetMode(int command)
        {
            return command <= (int)SoundPlayMode.Stop ? SoundPlayMode.Stop
                : command <= (int)SoundPlayMode.PlayLooping ? SoundPlayMode.PlayLooping
                : command == (int)SoundPlayMode.Play ? SoundPlayMode.Play
                : SoundPlayMode.Continue;
        }
    }
}
