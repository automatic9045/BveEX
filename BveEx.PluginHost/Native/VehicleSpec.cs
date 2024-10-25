using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Native
{
    /// <summary>
    /// 車両諸元情報を提供します。
    /// </summary>
    public class VehicleSpec
    {
        /// <summary>
        /// <see cref="VehicleSpec"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="brakeNotches">ブレーキノッチ数。</param>
        /// <param name="powerNotches">力行ノッチ数。</param>
        /// <param name="atsNotch">ATS 確認ノッチ。</param>
        /// <param name="b67notch">ブレーキ弁 67 度に相当するノッチ。</param>
        /// <param name="cars">編成両数。</param>
        public VehicleSpec(int brakeNotches, int powerNotches, int atsNotch, int b67notch, int cars)
        {
            BrakeNotches = brakeNotches;
            PowerNotches = powerNotches;
            AtsNotch = atsNotch;
            B67Notch = b67notch;
            Cars = cars;
        }

        /// <summary>
        /// ブレーキノッチ数を取得します。
        /// </summary>
        public int BrakeNotches { get; }

        /// <summary>
        /// 力行ノッチ数を取得します。
        /// </summary>
        public int PowerNotches { get; }

        /// <summary>
        /// ATS 確認ノッチを取得します。
        /// </summary>
        public int AtsNotch { get; }

        /// <summary>
        /// ブレーキ弁 67 度に相当するノッチを取得します。
        /// </summary>
        public int B67Notch { get; }

        /// <summary>
        /// 編成両数を取得します。
        /// </summary>
        public int Cars { get; }
    }
}
