using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Handles
{
    /// <summary>
    /// ブレーキなどのハンドルを表します。
    /// </summary>
    public interface IHandle
    {
        /// <summary>
        /// <see cref="MaxNotch"/> を超えたノッチを設定することが許可されているかを取得します。
        /// </summary>
        bool CanSetNotchOutOfRange { get; }

        /// <summary>
        /// 最小のノッチを取得します。
        /// </summary>
        int MinNotch { get; }

        /// <summary>
        /// 最大のノッチを取得します。
        /// </summary>
        int MaxNotch { get; }

        /// <summary>
        /// 現在のノッチを取得します。
        /// </summary>
        int Notch { get; }

        /// <summary>
        /// <see cref="Notch"/> の値が更新された時に発生します。
        /// </summary>
        event EventHandler NotchChanged;

        /// <summary>
        /// <see cref="MaxNotch"/> を超えたノッチを設定すると例外をスローするようにします。
        /// </summary>
        /// <remarks>
        /// TASC プラグインなどが <see cref="MaxNotch"/> を超えたノッチを設定することを想定して、既定では例外をスローしないようになっています。
        /// </remarks>
        void ProhibitNotchesOutOfRange();

        /// <summary>
        /// 指定した値にノッチを変更するコマンドを取得します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。</param>
        /// <returns>ノッチを <paramref name="notch"/> で指定した値に変更する <see cref="NotchCommandBase"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="notch"/> が <see cref="MinNotch"/> 未満か <see cref="MaxNotch"/> より大きいです。</exception>
        NotchCommandBase GetCommandToSetNotchTo(int notch);
    }
}
