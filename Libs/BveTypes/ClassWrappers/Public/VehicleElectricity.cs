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
    public class VehicleElectricity : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleElectricity>();

            PerformanceGetMethod = members.GetSourcePropertyGetterOf(nameof(Performance));

            TimeDelayedHandlesGetMethod = members.GetSourcePropertyGetterOf(nameof(TimeDelayedHandles));

            MotorStateGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorState));

            BreakerGetMethod = members.GetSourcePropertyGetterOf(nameof(Breaker));

            RegenerationLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(RegenerationLimit));
            RegenerationLimitSetMethod = members.GetSourcePropertySetterOf(nameof(RegenerationLimit));

            PowerReAdhesionGetMethod = members.GetSourcePropertyGetterOf(nameof(PowerReAdhesion));

            JerkRegulatorGetMethod = members.GetSourcePropertyGetterOf(nameof(JerkRegulator));

            SlipVelocityCoefficientGetMethod = members.GetSourcePropertyGetterOf(nameof(SlipVelocityCoefficient));
            SlipVelocityCoefficientSetMethod = members.GetSourcePropertySetterOf(nameof(SlipVelocityCoefficient));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
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
        /// 車両性能を取得します。
        /// </summary>
        public VehiclePerformance Performance => VehiclePerformance.FromSource(PerformanceGetMethod.Invoke(Src, null));

        private static FastMethod TimeDelayedHandlesGetMethod;
        /// <summary>
        /// 出力を遅延させたハンドルのセットを取得します。
        /// </summary>
        public TimeDelayedHandleSet TimeDelayedHandles => TimeDelayedHandleSet.FromSource(TimeDelayedHandlesGetMethod.Invoke(Src, null));

        private static FastMethod MotorStateGetMethod;
        /// <summary>
        /// モーターの状態を取得します。
        /// </summary>
        public VehicleMotorState MotorState => VehicleMotorState.FromSource(MotorStateGetMethod.Invoke(Src, null));

        private static FastMethod BreakerGetMethod;
        /// <summary>
        /// 遮断器を取得します。
        /// </summary>
        public CircuitBreaker Breaker => CircuitBreaker.FromSource(BreakerGetMethod.Invoke(Src, null));

        private static FastMethod RegenerationLimitGetMethod;
        private static FastMethod RegenerationLimitSetMethod;
        /// <summary>
        /// 電空協調制御を行う最低走行速度 [m/s] を取得・設定します。
        /// </summary>
        public double RegenerationLimit
        {
            get => (double)RegenerationLimitGetMethod.Invoke(Src, null);
            set => RegenerationLimitSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PowerReAdhesionGetMethod;
        /// <summary>
        /// 主制御装置の空転・滑走再粘着制御機構を取得します。
        /// </summary>
        public ReAdhesionControl PowerReAdhesion => ReAdhesionControl.FromSource(PowerReAdhesionGetMethod.Invoke(Src, null));

        private static FastMethod JerkRegulatorGetMethod;
        /// <summary>
        /// ジャーク制御機構を取得します。
        /// </summary>
        public JerkRegulator JerkRegulator => JerkRegulator.FromSource(JerkRegulatorGetMethod.Invoke(Src, null));

        private static FastMethod SlipVelocityCoefficientGetMethod;
        private static FastMethod SlipVelocityCoefficientSetMethod;
        /// <summary>
        /// インバーター制御におけるトルク分電流とすべり速度の比を取得・設定します。
        /// </summary>
        public double SlipVelocityCoefficient
        {
            get => (double)SlipVelocityCoefficientGetMethod.Invoke(Src, null);
            set => SlipVelocityCoefficientSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize() => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドはオリジナルではないため、<see cref="ClassMemberSet.GetSourceMethodOf(string, Type[])"/> メソッドから参照することはできません。<br/>
        /// このメソッドのオリジナルバージョンは <see cref="Tick(double)"/> です。
        /// </remarks>
        /// <param name="elapsed">前フレームからの経過時間。</param>
        /// <seealso cref="Tick(double)"/>
        public void Tick(TimeSpan elapsed) => Tick(elapsed.TotalSeconds);
    }
}
