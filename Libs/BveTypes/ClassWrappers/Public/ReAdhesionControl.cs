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
    /// 再粘着制御を表します。
    /// </summary>
    public class ReAdhesionControl : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ReAdhesionControl>();

            ModeGetMethod = members.GetSourcePropertyGetterOf(nameof(Mode));

            IsEnabledGetMethod = members.GetSourcePropertyGetterOf(nameof(IsEnabled));
            IsEnabledSetMethod = members.GetSourcePropertySetterOf(nameof(IsEnabled));

            SlipAccelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(SlipAcceleration));
            SlipAccelerationSetMethod = members.GetSourcePropertySetterOf(nameof(SlipAcceleration));

            SlipDecelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(SlipDeceleration));
            SlipDecelerationSetMethod = members.GetSourcePropertySetterOf(nameof(SlipDeceleration));

            SlipVelocityGetMethod = members.GetSourcePropertyGetterOf(nameof(SlipVelocity));
            SlipVelocitySetMethod = members.GetSourcePropertySetterOf(nameof(SlipVelocity));

            BalanceAccelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(BalanceAcceleration));
            BalanceAccelerationSetMethod = members.GetSourcePropertySetterOf(nameof(BalanceAcceleration));

            HoldingTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(HoldingTime));
            HoldingTimeSetMethod = members.GetSourcePropertySetterOf(nameof(HoldingTime));

            ReferenceAccelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(ReferenceAcceleration));
            ReferenceAccelerationSetMethod = members.GetSourcePropertySetterOf(nameof(ReferenceAcceleration));

            ReferenceDecelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(ReferenceDeceleration));
            ReferenceDecelerationSetMethod = members.GetSourcePropertySetterOf(nameof(ReferenceDeceleration));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ReAdhesionControl"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ReAdhesionControl(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ReAdhesionControl"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ReAdhesionControl FromSource(object src) => src is null ? null : new ReAdhesionControl(src);

        private static FastMethod ModeGetMethod;
        /// <summary>
        /// 再粘着制御の動作状態を取得します。
        /// </summary>
        public ReAdhesionControlMode Mode => (ReAdhesionControlMode)ModeGetMethod.Invoke(Src, null);

        private static FastMethod IsEnabledGetMethod;
        private static FastMethod IsEnabledSetMethod;
        /// <summary>
        /// 再粘着制御を使用するかどうかを取得・設定します。
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)IsEnabledGetMethod.Invoke(Src, null);
            set => IsEnabledSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SlipAccelerationGetMethod;
        private static FastMethod SlipAccelerationSetMethod;
        /// <summary>
        /// 点 A の軸加速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double SlipAcceleration
        {
            get => (double)SlipAccelerationGetMethod.Invoke(Src, null);
            set => SlipAccelerationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SlipDecelerationGetMethod;
        private static FastMethod SlipDecelerationSetMethod;
        /// <summary>
        /// 点 A の軸減速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double SlipDeceleration
        {
            get => (double)SlipDecelerationGetMethod.Invoke(Src, null);
            set => SlipDecelerationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SlipVelocityGetMethod;
        private static FastMethod SlipVelocitySetMethod;
        /// <summary>
        /// 点 A の軸速度と基準速度の差 [m/s] を取得・設定します。
        /// </summary>
        public double SlipVelocity
        {
            get => (double)SlipVelocityGetMethod.Invoke(Src, null);
            set => SlipVelocitySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BalanceAccelerationGetMethod;
        private static FastMethod BalanceAccelerationSetMethod;
        /// <summary>
        /// 点 B の軸加速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double BalanceAcceleration
        {
            get => (double)BalanceAccelerationGetMethod.Invoke(Src, null);
            set => BalanceAccelerationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod HoldingTimeGetMethod;
        private static FastMethod HoldingTimeSetMethod;
        /// <summary>
        /// 点 A から点 C までの最小時間 [s] を取得・設定します。
        /// </summary>
        public double HoldingTime
        {
            get => (double)HoldingTimeGetMethod.Invoke(Src, null);
            set => HoldingTimeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ReferenceAccelerationGetMethod;
        private static FastMethod ReferenceAccelerationSetMethod;
        /// <summary>
        /// 基準速度計算用の加速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double ReferenceAcceleration
        {
            get => (double)ReferenceAccelerationGetMethod.Invoke(Src, null);
            set => ReferenceAccelerationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ReferenceDecelerationGetMethod;
        private static FastMethod ReferenceDecelerationSetMethod;
        /// <summary>
        /// 基準速度計算用の減速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double ReferenceDeceleration
        {
            get => (double)ReferenceDecelerationGetMethod.Invoke(Src, null);
            set => ReferenceDecelerationSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
