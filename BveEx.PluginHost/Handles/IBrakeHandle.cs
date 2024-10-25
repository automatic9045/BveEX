using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace AtsEx.PluginHost.Handles
{
    /// <summary>
    /// ブレーキハンドルを表します。
    /// </summary>
    public interface IBrakeHandle : IHandle
    {
        /// <summary>
        /// ブレーキハンドルに抑速ブレーキノッチがあるかどうかを取得します。
        /// </summary>
        bool HasHoldingSpeedBrake { get; }

        /// <summary>
        /// 抑速ブレーキのノッチを取得します。ブレーキハンドルに抑速ブレーキノッチが無い場合は <see cref="InvalidOperationException"/> をスローします。
        /// </summary>
        int HoldingSpeedBrakeNotch { get; }

        /// <summary>
        /// 抑速ブレーキノッチを除いた常用ブレーキの段数を取得します。
        /// </summary>
        int ServiceBrakeNotchCount { get; }

        /// <summary>
        /// 抑速ブレーキノッチを除いた常用最小ブレーキのノッチを取得します。
        /// </summary>
        int MinServiceBrakeNotch { get; }

        /// <summary>
        /// 常用最大ブレーキのノッチを取得します。
        /// </summary>
        int MaxServiceBrakeNotch { get; }

        /// <summary>
        /// 非常ブレーキのノッチを取得します。
        /// </summary>
        int EmergencyBrakeNotch { get; }

        /// <summary>
        /// ATS 確認扱いで必要な最小ブレーキノッチを取得します。
        /// </summary>
        int AtsCancelNotch { get; }

        /// <summary>
        /// ブレーキ弁 67 度に相当するノッチを取得します。
        /// </summary>
        int B67Notch { get; }

        /// <summary>
        /// ノッチをニュートラル (ゼロ) に変更するコマンドを取得します。
        /// </summary>
        /// <returns>ノッチをニュートラルに変更する <see cref="NotchCommandBase"/>。</returns>
        NotchCommandBase GetCommandToSetToNeutral();
    }
}
