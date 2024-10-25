using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Handles
{
    /// <summary>
    /// 力行ハンドルを表します。
    /// </summary>
    public interface IPowerHandle : IHandle
    {
        /// <summary>
        /// 力行の段数を取得します。
        /// </summary>
        int PowerNotchCount { get; }

        /// <summary>
        /// 力行の最大ノッチを取得します。
        /// </summary>
        int MaxPowerNotch { get; }

        /// <summary>
        /// 抑速ブレーキの段数を取得します。
        /// </summary>
        int HoldingSpeedNotchCount { get; }

        /// <summary>
        /// 抑速ブレーキの最大ノッチを取得します。
        /// </summary>
        int MaxHoldingSpeedNotch { get; }

        /// <summary>
        /// ノッチをニュートラル (ゼロ) に変更するコマンドを取得します。
        /// </summary>
        /// <returns>ノッチをニュートラルに変更する <see cref="NotchCommandBase"/>。</returns>
        NotchCommandBase GetCommandToSetToNeutral();

        /// <summary>
        /// ノッチを力行最大に変更するコマンドを取得します。
        /// </summary>
        /// <returns>ノッチを力行最大に変更する <see cref="NotchCommandBase"/>。</returns>
        NotchCommandBase GetCommandToSetToMaxPowerNotch();
    }
}
