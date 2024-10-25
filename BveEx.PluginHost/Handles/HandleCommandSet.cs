using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static readonly HandleCommandSet DoNothing = new HandleCommandSet(NotchCommandBase.Continue, NotchCommandBase.Continue, ReverserPositionCommandBase.Continue, null);

        /// <summary>
        /// 力行ノッチの出力を編集するための <see cref="NotchCommandBase"/> を取得します。
        /// </summary>
        public NotchCommandBase PowerCommand { get; }

        /// <summary>
        /// ブレーキノッチの出力を編集するための <see cref="NotchCommandBase"/> を取得します。
        /// </summary>
        public NotchCommandBase BrakeCommand { get; }

        /// <summary>
        /// 逆転器の位置の出力を編集するための <see cref="ReverserPositionCommandBase"/> を取得します。
        /// </summary>
        public ReverserPositionCommandBase ReverserCommand { get; }

        /// <summary>
        /// 定速制御の状態の出力を編集するための <see cref="Handles.ConstantSpeedCommand"/> を取得します。
        /// </summary>
        public ConstantSpeedCommand? ConstantSpeedCommand { get; }

        /// <summary>
        /// <see cref="HandleCommandSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="powerCommand">力行ノッチの出力を編集するための <see cref="NotchCommandBase"/>。</param>
        /// <param name="brakeCommand">ブレーキノッチの出力を編集するための <see cref="NotchCommandBase"/>。</param>
        /// <param name="reverserCommand">逆転器の位置の出力を編集するための <see cref="ReverserPositionCommandBase"/>。</param>
        /// <param name="constantSpeedCommand">
        /// 定速制御の状態の出力を編集するための <see cref="Handles.ConstantSpeedCommand"/>。<br/>
        /// 出力を編集しない場合は <see langword="null"/> を指定します。<see cref="ConstantSpeedCommand.Continue"/> は前フレームと同じ値を指定することを表します。
        /// </param>
        public HandleCommandSet(NotchCommandBase powerCommand, NotchCommandBase brakeCommand, ReverserPositionCommandBase reverserCommand, ConstantSpeedCommand? constantSpeedCommand)
        {
            PowerCommand = powerCommand ?? throw new ArgumentNullException(nameof(powerCommand));
            BrakeCommand = brakeCommand ?? throw new ArgumentNullException(nameof(brakeCommand));
            ReverserCommand = reverserCommand ?? throw new ArgumentNullException(nameof(reverserCommand));
            ConstantSpeedCommand = constantSpeedCommand;
        }
    }
}
