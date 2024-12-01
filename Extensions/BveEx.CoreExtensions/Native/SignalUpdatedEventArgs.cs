using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.Native
{
    /// <summary>
    /// <see cref="INative.SignalUpdated"/> イベントのデータを表します。
    /// </summary>
    public class SignalUpdatedEventArgs
    {
        /// <summary>
        /// 信号インデックスを取得します。
        /// </summary>
        public int SignalIndex { get; }

        /// <summary>
        /// <see cref="SignalUpdatedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="signalIndex">信号インデックス。</param>
        public SignalUpdatedEventArgs(int signalIndex)
        {
            SignalIndex = signalIndex;
        }
    }
}
