using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.BeaconPassed"/> イベントのデータを表します。
    /// </summary>
    public class BeaconPassedEventArgs : EventArgs
    {
        /// <summary>
        /// 地上子の種別を表す値を取得します。
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// 対となる閉塞の信号インデックスを取得します。
        /// </summary>
        /// <seealso cref="SectionManager"/>
        public int SignalIndex { get; }

        /// <summary>
        /// 対となる閉塞までの距離 [m] を取得します。
        /// </summary>
        public float Distance { get; }

        /// <summary>
        /// 地上子に設定された任意の値を取得します。
        /// </summary>
        public int Optional { get; }

        /// <summary>
        /// <see cref="BeaconPassedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="type">地上子の種別を表す値。</param>
        /// <param name="signalIndex">対となる閉塞の信号インデックス。</param>
        /// <param name="distance">対となる閉塞までの距離 [m]。</param>
        /// <param name="optional">地上子に設定された任意の値。</param>
        public BeaconPassedEventArgs(int type, int signalIndex, float distance, int optional) : base()
        {
            Type = type;
            SignalIndex = signalIndex;
            Distance = distance;
            Optional = optional;
        }
    }
}
