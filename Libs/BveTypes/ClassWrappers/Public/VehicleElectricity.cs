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
    /// 自列車の電気の系全体を表します。
    /// </summary>
    public class VehicleElectricity : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleElectricity>();

            PerformanceGetMethod = members.GetSourcePropertyGetterOf(nameof(Performance));

            MotorStateGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorState));

            RegenerationLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(RegenerationLimit));
            RegenerationLimitSetMethod = members.GetSourcePropertySetterOf(nameof(RegenerationLimit));

            PowerReAdhesionGetMethod = members.GetSourcePropertyGetterOf(nameof(PowerReAdhesion));

            JerkRegulatorGetMethod = members.GetSourcePropertyGetterOf(nameof(JerkRegulator));

            SlipVelocityCoefficientGetMethod = members.GetSourcePropertyGetterOf(nameof(SlipVelocityCoefficient));
            SlipVelocityCoefficientSetMethod = members.GetSourcePropertySetterOf(nameof(SlipVelocityCoefficient));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleElectricity"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleElectricity(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleElectricity"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleElectricity FromSource(object src) => src is null ? null : new VehicleElectricity(src);

        private static FastMethod PerformanceGetMethod;
        /// <summary>
        /// 自列車の車両性能を取得します。
        /// </summary>
        public VehiclePerformance Performance => VehiclePerformance.FromSource(PerformanceGetMethod.Invoke(Src, null));

        private static FastMethod MotorStateGetMethod;
        /// <summary>
        /// 自列車のモーターの状態を取得します。
        /// </summary>
        public VehicleMotorState MotorState => VehicleMotorState.FromSource(MotorStateGetMethod.Invoke(Src, null));

        private static FastMethod RegenerationLimitGetMethod;
        private static FastMethod RegenerationLimitSetMethod;
        /// <summary>
        /// 電気ブレーキを遮断する走行速度 [m/s] を取得・設定します。
        /// </summary>
        public double RegenerationLimit
        {
            get => RegenerationLimitGetMethod.Invoke(Src, null);
            set => RegenerationLimitSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PowerReAdhesionGetMethod;
        /// <summary>
        /// 自列車の主制御装置の空転・滑走再粘着制御機構を取得します。
        /// </summary>
        public ReAdhesionControl PowerReAdhesion => ReAdhesionControl.FromSource(PowerReAdhesionGetMethod.Invoke(Src, null));

        private static FastMethod JerkRegulatorGetMethod;
        /// <summary>
        /// 自列車のジャーク制御機構を取得します。
        /// </summary>
        public JerkRegulator JerkRegulator => ClassWrappers.JerkRegulator.FromSource(JerkRegulatorGetMethod.Invoke(Src, null));

        private static FastMethod SlipVelocityCoefficientGetMethod;
        private static FastMethod SlipVelocityCoefficientSetMethod;
        /// <summary>
        /// インバーター制御におけるトルク分電流とすべり速度の比を取得・設定します。
        /// </summary>
        public double SlipVelocityCoefficient
        {
            get => SlipVelocityCoefficientGetMethod.Invoke(Src, null);
            set => SlipVelocityCoefficientSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
