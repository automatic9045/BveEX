using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の揺れを制御します。
    /// </summary>
    public class VehicleVibration : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleVibration>();

            Constructor = members.GetSourceConstructor();

            CarBodyTransformGetMethod = members.GetSourcePropertyGetterOf(nameof(CarBodyTransform));

            FrontAccelerationXGetMethod = members.GetSourcePropertyGetterOf(nameof(FrontAccelerationX));
            RearAccelerationXGetMethod = members.GetSourcePropertyGetterOf(nameof(RearAccelerationX));
            AccelerationZGetMethod = members.GetSourcePropertyGetterOf(nameof(AccelerationZ));

            CarBodyAccelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(CarBodyAcceleration));
            CarBodyDisplacementGetMethod = members.GetSourcePropertyGetterOf(nameof(CarBodyDisplacement));

            PositionerGetMethod = members.GetSourcePropertyGetterOf(nameof(Positioner));

            HalfOfCarLengthGetMethod = members.GetSourcePropertyGetterOf(nameof(HalfOfCarLength));
            HalfOfCarLengthSetMethod = members.GetSourcePropertySetterOf(nameof(HalfOfCarLength));

            HalfOfBogieDistanceGetMethod = members.GetSourcePropertyGetterOf(nameof(HalfOfBogieDistance));
            HalfOfBogieDistanceSetMethod = members.GetSourcePropertySetterOf(nameof(HalfOfBogieDistance));

            SpringHeightGetMethod = members.GetSourcePropertyGetterOf(nameof(SpringHeight));
            SpringHeightSetMethod = members.GetSourcePropertySetterOf(nameof(SpringHeight));

            ViewPointGetMethod = members.GetSourcePropertyGetterOf(nameof(ViewPoint));
            ViewPointSetMethod = members.GetSourcePropertySetterOf(nameof(ViewPoint));

            PassengerField = members.GetSourceFieldOf(nameof(Passenger));
            VerticalSpringsField = members.GetSourceFieldOf(nameof(VerticalSprings));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            UpdateCarBodyTransformMethod = members.GetSourceMethodOf(nameof(UpdateCarBodyTransform));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleVibration"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleVibration(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleVibration"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleVibration FromSource(object src) => src is null ? null : new VehicleVibration(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="VehicleVibration"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="vehicleLocation">自列車の位置情報。</param>
        /// <param name="myTrack">自軌道。</param>
        /// <param name="irregularityObjects">軌道変位のリスト。</param>
        /// <param name="passenger">乗客。</param>
        public VehicleVibration(VehicleLocation vehicleLocation, MyTrack myTrack, MapFunctionList irregularityObjects, Passenger passenger)
            : this(Constructor.Invoke(new object[] { vehicleLocation?.Src, myTrack?.Src, irregularityObjects?.Src, passenger?.Src }))
        {
        }

        private static FastMethod CarBodyTransformGetMethod;
        /// <summary>
        /// 車両中心原点・車両前方を基準とした座標系から運転士視点の座標系に変換する行列を格納している <see cref="Transform"/> を取得します。
        /// </summary>
        public Transform CarBodyTransform => Transform.FromSource(CarBodyTransformGetMethod.Invoke(Src, null));

        private static FastMethod FrontAccelerationXGetMethod;
        /// <summary>
        /// 見かけ上の前台車上左右加速度 [m/s^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 遠心力・重力成分を含みます。
        /// </remarks>
        public double FrontAccelerationX => (double)FrontAccelerationXGetMethod.Invoke(Src, null);

        private static FastMethod RearAccelerationXGetMethod;
        /// <summary>
        /// 見かけ上の後台車上左右加速度 [m/s^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 遠心力・重力成分を含みます。
        /// </remarks>
        public double RearAccelerationX => (double)RearAccelerationXGetMethod.Invoke(Src, null);

        private static FastMethod AccelerationZGetMethod;
        /// <summary>
        /// 見かけ上の前後加速度 [m/s^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 重力成分を含みます。
        /// </remarks>
        public double AccelerationZ => (double)AccelerationZGetMethod.Invoke(Src, null);

        private static FastMethod CarBodyAccelerationGetMethod;
        /// <summary>
        /// 車体の加速度 [m/s^2] を取得します。
        /// </summary>
        public SixDof CarBodyAcceleration => SixDof.FromSource(CarBodyAccelerationGetMethod.Invoke(Src, null));

        private static FastMethod CarBodyDisplacementGetMethod;
        /// <summary>
        /// 車体の変位 [m] を取得します。
        /// </summary>
        public SixDof CarBodyDisplacement => SixDof.FromSource(CarBodyDisplacementGetMethod.Invoke(Src, null));

        private static FastMethod PositionerGetMethod;
        /// <summary>
        /// 自列車をマップ上に配置するための機能を提供する <see cref="VehiclePositioner"/> を取得します。
        /// </summary>
        public VehiclePositioner Positioner => VehiclePositioner.FromSource(PositionerGetMethod.Invoke(Src, null));

        private static FastMethod HalfOfCarLengthGetMethod;
        private static FastMethod HalfOfCarLengthSetMethod;
        /// <summary>
        /// 車両長 [m] の半分、すなわち車両の先頭から中心までの距離を取得・設定します。
        /// </summary>
        public double HalfOfCarLength
        {
            get => (double)HalfOfCarLengthGetMethod.Invoke(Src, null);
            set => HalfOfCarLengthSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod HalfOfBogieDistanceGetMethod;
        private static FastMethod HalfOfBogieDistanceSetMethod;
        /// <summary>
        /// 台車中心間距離 [m] の半分を取得・設定します。
        /// </summary>
        public double HalfOfBogieDistance
        {
            get => (double)HalfOfBogieDistanceGetMethod.Invoke(Src, null);
            set => HalfOfBogieDistanceSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SpringHeightGetMethod;
        private static FastMethod SpringHeightSetMethod;
        /// <summary>
        /// 空気ばねの高さ [m] を取得・設定します。
        /// </summary>
        public double SpringHeight
        {
            get => (double)SpringHeightGetMethod.Invoke(Src, null);
            set => SpringHeightSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ViewPointGetMethod;
        private static FastMethod ViewPointSetMethod;
        /// <summary>
        /// 運転士視点の位置を取得・設定します。
        /// </summary>
        public SixDof ViewPoint
        {
            get => SixDof.FromSource(ViewPointGetMethod.Invoke(Src, null));
            set => ViewPointSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastField PassengerField;
        /// <summary>
        /// 自列車の乗客を取得・設定します。
        /// </summary>
        public Passenger Passenger
        {
            get => Passenger.FromSource(PassengerField.GetValue(Src));
            set => PassengerField.SetValue(Src, value?.Src);
        }

        private static FastField VerticalSpringsField;
        /// <summary>
        /// 空気ばねの縦 (Y) 方向モデルを取得・設定します。
        /// </summary>
        /// <remarks>
        /// 右前、左前、右後、左後の順番で格納されています。
        /// </remarks>
        public WrappedArray<AirSpring> VerticalSprings
        {
            get => WrappedArray<AirSpring>.FromSource(VerticalSpringsField.GetValue(Src) as Array);
            set => VerticalSpringsField.SetValue(Src, value?.Src);
        }

        private static FastMethod InitializeMethod;
        /// <summary>
        /// 初期化します。
        /// </summary>
        public void Initialize() => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
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

        private static FastMethod UpdateCarBodyTransformMethod;
        /// <summary>
        /// 車体の揺れを表す行列を更新します。
        /// </summary>
        public void UpdateCarBodyTransform() => UpdateCarBodyTransformMethod.Invoke(Src, null);
    }
}
