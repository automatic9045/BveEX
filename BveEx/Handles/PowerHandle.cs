using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Handles;

namespace BveEx.Handles
{
    internal class PowerHandle : HandleBase, IPowerHandle
    {
        public int PowerNotchCount => MaxNotch;
        public int MaxPowerNotch => MaxNotch;
        public int HoldingSpeedNotchCount => -MinNotch;
        public int MaxHoldingSpeedNotch => -MinNotch;

        /// <summary>
        /// <see cref="PowerHandle"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="powerNotchCount">力行の段数。</param>
        /// <param name="holdingSpeedNotchCount">抑速ブレーキの段数。</param>
        public PowerHandle(int powerNotchCount, int holdingSpeedNotchCount) : base(-holdingSpeedNotchCount, powerNotchCount)
        {
            if (powerNotchCount <= 0) throw new ArgumentOutOfRangeException(nameof(powerNotchCount));
            if (holdingSpeedNotchCount < 0) throw new ArgumentOutOfRangeException(nameof(holdingSpeedNotchCount));
        }

        /// <summary>
        /// 抑速ノッチを持たない <see cref="PowerHandle"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="powerNotchCount">力行の段数。</param>
        public PowerHandle(int powerNotchCount) : this(powerNotchCount, 0)
        {
        }

        public static PowerHandle FromNotchInfo(NotchInfo source)
        {
            int powerNotchCount = source.PowerNotchCount;
            int holdingSpeedNotchCount = -source.HoldingSpeedNotchCount;

            return new PowerHandle(powerNotchCount, holdingSpeedNotchCount);
        }

        public NotchCommandBase GetCommandToSetToNeutral() => new NotchCommandBase.SetNotchCommand(0);
        public NotchCommandBase GetCommandToSetToMaxPowerNotch() => new NotchCommandBase.SetNotchCommand(MaxPowerNotch);
    }
}
