using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.PluginHost.Handles
{
    /// <summary>
    /// プラグインからハンドルの出力を編集するためのコマンドのセットを表します。
    /// </summary>
    public class HandleCommandSet
    {
        /// <summary>
        /// ハンドルの出力を一切編集しないことを表す <see cref="HandleCommandSet"/> を取得します。
        /// </summary>
        public static readonly HandleCommandSet DoNothing = new HandleCommandSet();

        /// <summary>
        /// 力行ノッチの出力を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 出力を編集しない場合は <see langword="null"/> を指定します。
        /// </remarks>
        public int? PowerNotch { get; set; } = null;

        /// <summary>
        /// ブレーキノッチの出力を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 出力を編集しない場合は <see langword="null"/> を指定します。
        /// </remarks>
        public int? BrakeNotch { get; set; } = null;

        /// <summary>
        /// 逆転器の位置の出力を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 出力を編集しないことを表すには、<see langword="null"/> を指定します。
        /// </remarks>
        public ReverserPosition? ReverserPosition { get; set; } = null;

        /// <summary>
        /// 定速制御の状態の出力を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 出力を編集しない場合は <see langword="null"/> を指定します。<see cref="ConstantSpeedMode.Continue"/> は前フレームと同じ値を指定することを表します。
        /// </remarks>
        public ConstantSpeedMode? ConstantSpeedMode { get; set; } = null;

        /// <summary>
        /// <see cref="HandleCommandSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public HandleCommandSet()
        {
        }
    }
}
