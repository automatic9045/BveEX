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
    public class VehicleVibrationManager : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleVibrationManager>();

            CarBodyTransformGetMethod = members.GetSourcePropertyGetterOf(nameof(CarBodyTransform));

            CarBodyDisplacementGetMethod = members.GetSourcePropertyGetterOf(nameof(CarBodyDisplacement));

            PositionerGetMethod = members.GetSourcePropertyGetterOf(nameof(Positioner));

            SpringHeightGetMethod = members.GetSourcePropertyGetterOf(nameof(SpringHeight));
            SpringHeightSetMethod = members.GetSourcePropertySetterOf(nameof(SpringHeight));

            ViewPointGetMethod = members.GetSourcePropertyGetterOf(nameof(ViewPoint));
            ViewPointSetMethod = members.GetSourcePropertySetterOf(nameof(ViewPoint));

            PassengerField = members.GetSourceFieldOf(nameof(Passenger));
            VerticalSpringsField = members.GetSourceFieldOf(nameof(VerticalSprings));

            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            UpdateCarBodyTransformMethod = members.GetSourceMethodOf(nameof(UpdateCarBodyTransform));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleVibrationManager"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleVibrationManager(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleVibrationManager"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleVibrationManager FromSource(object src) => src is null ? null : new VehicleVibrationManager(src);

        private static FastMethod CarBodyTransformGetMethod;
        /// <summary>
        /// 車体中心から運転士を取得します。
        /// </summary>
        public Transform CarBodyTransform => Transform.FromSource(CarBodyTransformGetMethod.Invoke(Src, null));

        private static FastMethod CarBodyDisplacementGetMethod;
        /// <summary>
        /// 車体中心から運転士を取得します。
        /// </summary>
        public SixDof CarBodyDisplacement => SixDof.FromSource(CarBodyDisplacementGetMethod.Invoke(Src, null));

        private static FastMethod PositionerGetMethod;
        /// <summary>
        /// 自列車をマップ上に配置するための機能を提供する <see cref="VehiclePositioner"/> を取得します。
        /// </summary>
        public VehiclePositioner Positioner => VehiclePositioner.FromSource(PositionerGetMethod.Invoke(Src, null));

        private static FastMethod SpringHeightGetMethod;
        private static FastMethod SpringHeightSetMethod;
        /// <summary>
        /// を取得・設定します。
        /// </summary>
        public double SpringHeight
        {
            get => SpringHeightGetMethod.Invoke(Src, null);
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
            set => ViewPointSetMethod.Invoke(Src, new object[] { value.Src });
        }

        private static FastField PassengerField;
        /// <summary>
        /// 自列車の乗客を取得・設定します。
        /// </summary>
        public Passenger Passenger
        {
            get => ClassWrappers.Passenger.FromSource(PassengerField.GetValue(Src));
            set => PassengerField.SetValue(Src, value.Src);
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
            get => WrappedArray<AirSpring>.FromSource(VerticalSpringsField.GetValue(Src));
            set => VerticalSpringsField.SetValue(Src, value.Src);
        }

        private static FastMethod TickMethod;
        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        public void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        private static FastMethod UpdateCarBodyTransformMethod;
        /// <summary>
        /// 車両
        /// </summary>
        public void UpdateCarBodyTransform() => UpdateCarBodyTransformMethod.Invoke(Src, null);
    }
}
