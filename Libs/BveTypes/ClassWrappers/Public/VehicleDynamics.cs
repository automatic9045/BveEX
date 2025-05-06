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
    /// 自列車の力学系全体を表します。
    /// </summary>
    public class VehicleDynamics : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleDynamics>();

            Constructor = members.GetSourceConstructor();

            InertiaRatioGetMethod = members.GetSourcePropertyGetterOf(nameof(InertiaRatio));

            CurveResistanceFactorGetMethod = members.GetSourcePropertyGetterOf(nameof(CurveResistanceFactor));
            CurveResistanceFactorSetMethod = members.GetSourcePropertySetterOf(nameof(CurveResistanceFactor));

            RunningResistanceFactorAGetMethod = members.GetSourcePropertyGetterOf(nameof(RunningResistanceFactorA));
            RunningResistanceFactorASetMethod = members.GetSourcePropertySetterOf(nameof(RunningResistanceFactorA));

            RunningResistanceFactorBGetMethod = members.GetSourcePropertyGetterOf(nameof(RunningResistanceFactorB));
            RunningResistanceFactorBSetMethod = members.GetSourcePropertySetterOf(nameof(RunningResistanceFactorB));

            RunningResistanceFactorCGetMethod = members.GetSourcePropertyGetterOf(nameof(RunningResistanceFactorC));
            RunningResistanceFactorCSetMethod = members.GetSourcePropertySetterOf(nameof(RunningResistanceFactorC));

            CarLengthGetMethod = members.GetSourcePropertyGetterOf(nameof(CarLength));
            CarLengthSetMethod = members.GetSourcePropertySetterOf(nameof(CarLength));

            TrailerCarGetMethod = members.GetSourcePropertyGetterOf(nameof(TrailerCar));

            MotorCarGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCar));

            FirstCarGetMethod = members.GetSourcePropertyGetterOf(nameof(FirstCar));
            FirstCarSetMethod = members.GetSourcePropertySetterOf(nameof(FirstCar));

            TrackAlignmentField = members.GetSourceFieldOf(nameof(TrackAlignment));
            TotalMassField = members.GetSourceFieldOf(nameof(TotalMass));
            TotalInertiaField = members.GetSourceFieldOf(nameof(TotalInertia));
            MassRatioField = members.GetSourceFieldOf(nameof(MassRatio));

            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            SetupMethod = members.GetSourceMethodOf(nameof(Setup));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleDynamics"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleDynamics(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleDynamics"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleDynamics FromSource(object src) => src is null ? null : new VehicleDynamics(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="VehicleDynamics"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="vehicleLocation">自列車の位置情報。</param>
        /// <param name="vehicleVibration">自列車の揺れ。</param>
        /// <param name="passengerLoad">乗客 1 人当たりの質量 [kg]。</param>
        /// <param name="myTrack">自軌道。</param>
        /// <param name="adhesionObjects">車輪 - レール間の粘着特性のリスト。</param>
        /// <param name="brakeShoeFrictionObjects">粘着係数のリスト。</param>
        public VehicleDynamics(VehicleLocation vehicleLocation, VehicleVibration vehicleVibration, ValueContainer passengerLoad,
            MyTrack myTrack, MapFunctionList adhesionObjects, MapFunctionList brakeShoeFrictionObjects)
            : this(Constructor.Invoke(new object[] { vehicleLocation?.Src, vehicleVibration?.Src, passengerLoad?.Src, myTrack?.Src, adhesionObjects?.Src, brakeShoeFrictionObjects?.Src }))
        {
        }

        private static FastMethod InertiaRatioGetMethod;
        /// <summary>
        /// 空車時に対する自列車全体の慣性比を取得します。
        /// </summary>
        public ValueContainer InertiaRatio => ValueContainer.FromSource(InertiaRatioGetMethod.Invoke(Src, null));

        private static FastMethod CurveResistanceFactorGetMethod;
        private static FastMethod CurveResistanceFactorSetMethod;
        /// <summary>
        /// 曲線抵抗の係数を取得・設定します。
        /// </summary>
        public double CurveResistanceFactor
        {
            get => (double)CurveResistanceFactorGetMethod.Invoke(Src, null);
            set => CurveResistanceFactorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RunningResistanceFactorAGetMethod;
        private static FastMethod RunningResistanceFactorASetMethod;
        /// <summary>
        /// 速度の単位を [m/s] としたときの走行抵抗の係数 a を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 車両パラメーターファイルで定義する係数は速度の単位を [km/h] としたときのもののため、
        /// ここで取得・設定する値はその 3.6 ^ 2 = 12.96 倍となります。
        /// </remarks>
        public double RunningResistanceFactorA
        {
            get => (double)RunningResistanceFactorAGetMethod.Invoke(Src, null);
            set => RunningResistanceFactorASetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RunningResistanceFactorBGetMethod;
        private static FastMethod RunningResistanceFactorBSetMethod;
        /// <summary>
        /// 速度の単位を [m/s] としたときの走行抵抗の係数 b を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 車両パラメーターファイルで定義する係数は速度の単位を [km/h] としたときのもののため、
        /// ここで取得・設定する値はその 3.6 倍となります。
        /// </remarks>
        public double RunningResistanceFactorB
        {
            get => (double)RunningResistanceFactorBGetMethod.Invoke(Src, null);
            set => RunningResistanceFactorBSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RunningResistanceFactorCGetMethod;
        private static FastMethod RunningResistanceFactorCSetMethod;
        /// <summary>
        /// 走行抵抗の係数 c を取得・設定します。
        /// </summary>
        public double RunningResistanceFactorC
        {
            get => (double)RunningResistanceFactorCGetMethod.Invoke(Src, null);
            set => RunningResistanceFactorCSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CarLengthGetMethod;
        private static FastMethod CarLengthSetMethod;
        /// <summary>
        /// 1 両当たりの長さ [m] を取得・設定します。
        /// </summary>
        public double CarLength
        {
            get => (double)CarLengthGetMethod.Invoke(Src, null);
            set => CarLengthSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TrailerCarGetMethod;
        /// <summary>
        /// 付随車の情報を提供する <see cref="CarInfo"/> を取得します。
        /// </summary>
        public CarInfo TrailerCar => CarInfo.FromSource(TrailerCarGetMethod.Invoke(Src, null));

        private static FastMethod MotorCarGetMethod;
        /// <summary>
        /// 動力車の情報を提供する <see cref="CarInfo"/> を取得します。
        /// </summary>
        public CarInfo MotorCar => CarInfo.FromSource(MotorCarGetMethod.Invoke(Src, null));

        private static FastMethod FirstCarGetMethod;
        private static FastMethod FirstCarSetMethod;
        /// <summary>
        /// 先頭車両の情報を提供する <see cref="CarInfo"/> を取得・設定します。
        /// </summary>
        public CarInfo FirstCar
        {
            get => CarInfo.FromSource(FirstCarGetMethod.Invoke(Src, null));
            set => FirstCarSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastField TrackAlignmentField;
        /// <summary>
        /// 自列車の軌道形状を取得・設定します。
        /// </summary>
        public MyTrackAlignment TrackAlignment
        {
            get => MyTrackAlignment.FromSource(TrackAlignmentField.GetValue(Src));
            set => TrackAlignmentField.SetValue(Src, value?.Src);
        }

        private static FastField TotalMassField;
        /// <summary>
        /// 乗客を含めた自列車全体の質量 [kg] を取得・設定します。
        /// </summary>
        public double TotalMass
        {
            get => (double)TotalMassField.GetValue(Src);
            set => TotalMassField.SetValue(Src, value);
        }

        private static FastField TotalInertiaField;
        /// <summary>
        /// 乗客を含めた自列車全体の慣性モーメントの質量相当 [kg] を取得・設定します。
        /// </summary>
        public double TotalInertia
        {
            get => (double)TotalInertiaField.GetValue(Src);
            set => TotalInertiaField.SetValue(Src, value);
        }

        private static FastField MassRatioField;
        /// <summary>
        /// 空車時に対する自列車全体の質量比を取得・設定します。
        /// </summary>
        public double MassRatio
        {
            get => (double)MassRatioField.GetValue(Src);
            set => MassRatioField.SetValue(Src, value);
        }

        private static FastMethod TickMethod;
        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <param name="elapsedSeconds">前フレームからの経過時間 [s]。</param>
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

        private static FastMethod InitializeMethod;
        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="speed">速度 [m/s]。</param>
        public void Initialize(double speed) => InitializeMethod.Invoke(Src, new object[] { speed });

        private static FastMethod SetupMethod;
        /// <summary>
        /// 両数に依存するパラメータを計算します。
        /// </summary>
        public void Setup() => SetupMethod.Invoke(Src, null);
    }
}
