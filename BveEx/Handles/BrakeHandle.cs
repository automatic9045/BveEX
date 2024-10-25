using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using BveEx.PluginHost.Handles;

namespace BveEx.Handles
{
    internal class BrakeHandle : HandleBase, IBrakeHandle
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<BrakeHandle>(@"Core\Handles");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> NoHoldingSpeedBrake { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static BrakeHandle()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public bool HasHoldingSpeedBrake { get; }
        public int HoldingSpeedBrakeNotch => HasHoldingSpeedBrake ? 1 : throw new InvalidOperationException(Resources.Value.NoHoldingSpeedBrake.Value);
        public int ServiceBrakeNotchCount => MaxNotch - 1 - (HasHoldingSpeedBrake ? 1 : 0);
        public int MinServiceBrakeNotch => HasHoldingSpeedBrake ? 2 : 1;
        public int MaxServiceBrakeNotch => MaxNotch - 1;
        public int EmergencyBrakeNotch => MaxNotch;
        public int AtsCancelNotch { get; }
        public int B67Notch { get; }

        /// <summary>
        /// <see cref="BrakeHandle"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="serviceBrakeNotchCount">抑速ブレーキノッチを除いた常用ブレーキの段数。</param>
        /// <param name="hasHoldingSpeedBrake">ブレーキハンドルに抑速ブレーキノッチがあるかどうか。</param>
        public BrakeHandle(int serviceBrakeNotchCount, int atsCancelNotch, int b67Notch, bool hasHoldingSpeedBrake) : base(0, (hasHoldingSpeedBrake ? 1 : 0) + serviceBrakeNotchCount + 1)
        {
            if (serviceBrakeNotchCount <= 0) throw new ArgumentOutOfRangeException(nameof(serviceBrakeNotchCount));
            if (atsCancelNotch <= 0 || MaxNotch <= atsCancelNotch) throw new ArgumentOutOfRangeException(nameof(atsCancelNotch));
            if (b67Notch <= 0 || MaxNotch <= b67Notch) throw new ArgumentOutOfRangeException(nameof(b67Notch));

            AtsCancelNotch = atsCancelNotch;
            B67Notch = b67Notch;
            HasHoldingSpeedBrake = hasHoldingSpeedBrake;
        }

        public static BrakeHandle FromNotchInfo(NotchInfo source)
        {
            int serviceBrakeNotchCount = source.BrakeNotchCount;
            bool hasHoldingSpeedBrake = source.HasHoldingSpeedBrake;

            return new BrakeHandle(serviceBrakeNotchCount - (hasHoldingSpeedBrake ? 1 : 0), source.AtsCancelNotch, source.B67Notch, hasHoldingSpeedBrake);
        }

        public NotchCommandBase GetCommandToSetToNeutral() => new NotchCommandBase.SetNotchCommand(0);
    }
}
