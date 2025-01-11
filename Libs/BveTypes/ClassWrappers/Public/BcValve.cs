using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ブレーキシリンダ電磁弁を表します。
    /// </summary>
    public class BcValve : Valve
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BcValve>();

            IsBcServoGetMethod = members.GetSourcePropertyGetterOf(nameof(IsBcServo));
            IsBcServoSetMethod = members.GetSourcePropertySetterOf(nameof(IsBcServo));

            RapidReleaseSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(RapidReleaseSpeed));
            RapidReleaseSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(RapidReleaseSpeed));

            RapidApplySpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(RapidApplySpeed));
            RapidApplySpeedSetMethod = members.GetSourcePropertySetterOf(nameof(RapidApplySpeed));

            ReleaseStartMarginField = members.GetSourceFieldOf(nameof(ReleaseStartMargin));
            ApplyStartMarginField = members.GetSourceFieldOf(nameof(ApplyStartMargin));
            ReleaseStopMarginField = members.GetSourceFieldOf(nameof(ReleaseStopMargin));
            ApplyStopMarginField = members.GetSourceFieldOf(nameof(ApplyStopMargin));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BcValve"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BcValve(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BcValve"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new BcValve FromSource(object src) => src is null ? null : new BcValve(src);

        private static FastMethod IsBcServoGetMethod;
        private static FastMethod IsBcServoSetMethod;
        /// <summary>
        /// 圧力比例制御式かどうかを取得・設定します。
        /// </summary>
        public bool IsBcServo
        {
            get => (bool)IsBcServoGetMethod.Invoke(Src, null);
            set => IsBcServoSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RapidReleaseSpeedGetMethod;
        private static FastMethod RapidReleaseSpeedSetMethod;
        /// <summary>
        /// 滑走再粘着制御における急排気速度 [Pa^(1/2)/s] を取得・設定します。
        /// </summary>
        public double RapidReleaseSpeed
        {
            get => (double)RapidReleaseSpeedGetMethod.Invoke(Src, null);
            set => RapidReleaseSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RapidApplySpeedGetMethod;
        private static FastMethod RapidApplySpeedSetMethod;
        /// <summary>
        /// 滑走再粘着制御における急給気速度 [Pa^(1/2)/s] を取得・設定します。
        /// </summary>
        public double RapidApplySpeed
        {
            get => (double)RapidApplySpeedGetMethod.Invoke(Src, null);
            set => RapidApplySpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField ReleaseStartMarginField;
        /// <summary>
        /// 排気が開始する際の圧力指令と現在圧力の差 [Pa] を取得・設定します。
        /// </summary>
        public double ReleaseStartMargin
        {
            get => (double)ReleaseStartMarginField.GetValue(Src);
            set => ReleaseStartMarginField.SetValue(Src, value);
        }

        private static FastField ApplyStartMarginField;
        /// <summary>
        /// 給気が開始する際の圧力指令と現在圧力の差 [Pa] を取得・設定します。
        /// </summary>
        public double ApplyStartMargin
        {
            get => (double)ApplyStartMarginField.GetValue(Src);
            set => ApplyStartMarginField.SetValue(Src, value);
        }

        private static FastField ReleaseStopMarginField;
        /// <summary>
        /// 排気が停止する際の圧力指令と現在圧力の差 [Pa] を取得・設定します。
        /// </summary>
        public double ReleaseStopMargin
        {
            get => (double)ReleaseStopMarginField.GetValue(Src);
            set => ReleaseStopMarginField.SetValue(Src, value);
        }

        private static FastField ApplyStopMarginField;
        /// <summary>
        /// 給気が停止する際の圧力指令と現在圧力の差 [Pa] を取得・設定します。
        /// </summary>
        public double ApplyStopMargin
        {
            get => (double)ApplyStopMarginField.GetValue(Src);
            set => ApplyStopMarginField.SetValue(Src, value);
        }
    }
}
