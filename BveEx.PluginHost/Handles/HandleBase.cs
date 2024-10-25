using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic9045.AtsEx.PluginHost.Handles
{
    /// <summary>
    /// ブレーキなどのハンドルを表します。
    /// </summary>
    public abstract class HandleBase
    {
        /// <summary>
        /// 最小のノッチを取得します。
        /// </summary>
        public int MinNotch { get; }

        /// <summary>
        /// 最大のノッチを取得します。
        /// </summary>
        public int MaxNotch { get; }

        /// <summary>
        /// <see cref="HandleBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="minNotch">最小のノッチ。</param>
        /// <param name="maxNotch">最大のノッチ。</param>
        public HandleBase(int minNotch, int maxNotch)
        {
            MinNotch = minNotch;
            MaxNotch = maxNotch;
        }

        /// <summary>
        /// 指定した値にノッチを変更するコマンドを取得します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。</param>
        /// <returns>ノッチを <paramref name="notch"/> で指定した値に変更する <see cref="NotchCommandBase.SetNotchCommand"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="notch"/> が <see cref="MinNotch"/> 未満か <see cref="MaxNotch"/> より大きいです。</exception>
        public NotchCommandBase.SetNotchCommand GetCommandToSetNotchTo(int notch)
        {
            return notch < MinNotch ? throw new ArgumentOutOfRangeException(nameof(notch))
                : MaxNotch < notch ? throw new ArgumentOutOfRangeException(nameof(notch))
                : new NotchCommandBase.SetNotchCommand(notch);
        }
    }
}
