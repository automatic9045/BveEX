using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車をマップ上に配置するための機能を提供します。
    /// </summary>
    public class VehiclePositioner : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehiclePositioner>();

            FrontBogieGetMethod = members.GetSourcePropertyGetterOf(nameof(FrontBogie));

            RearBogieGetMethod = members.GetSourcePropertyGetterOf(nameof(RearBogie));

            HalfOfCarLengthGetMethod = members.GetSourcePropertyGetterOf(nameof(HalfOfCarLength));
            HalfOfCarLengthSetMethod = members.GetSourcePropertySetterOf(nameof(HalfOfCarLength));

            HalfOfBogieDistanceGetMethod = members.GetSourcePropertyGetterOf(nameof(HalfOfBogieDistance));
            HalfOfBogieDistanceSetMethod = members.GetSourcePropertySetterOf(nameof(HalfOfBogieDistance));

            PositionInBlockField = members.GetSourceFieldOf(nameof(PositionInBlock));
            BlockToCarCenterTransformField = members.GetSourceFieldOf(nameof(BlockToCarCenterTransform));

            OnLocationChangedMethod = members.GetSourceMethodOf(nameof(OnLocationChanged));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehiclePositioner"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehiclePositioner(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehiclePositioner"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehiclePositioner FromSource(object src) => src is null ? null : new VehiclePositioner(src);

        private static FastMethod FrontBogieGetMethod;
        /// <summary>
        /// 前台車の車輪 (軌道接触部) に関する情報を取得します。
        /// </summary>
        public VehicleBogieWheel FrontBogie => VehicleBogieWheel.FromSource(FrontBogieGetMethod.Invoke(Src, null));

        private static FastMethod RearBogieGetMethod;
        /// <summary>
        /// 後台車の車輪 (軌道接触部) に関する情報を取得します。
        /// </summary>
        public VehicleBogieWheel RearBogie => VehicleBogieWheel.FromSource(RearBogieGetMethod.Invoke(Src, null));

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

        private static FastField PositionInBlockField;
        /// <summary>
        /// 現在自列車が走行しているマップ ブロックの原点に対する、車両中心の位置ベクトル [m] を取得・設定します。
        /// </summary>
        public Vector3 PositionInBlock
        {
            get => (Vector3)PositionInBlockField.GetValue(Src);
            set => PositionInBlockField.SetValue(Src, value);
        }

        private static FastField BlockToCarCenterTransformField;
        /// <summary>
        /// ワールド座標系を車両中心原点・車両前方を基準とした座標系に変換する行列を格納している <see cref="Transform"/> を取得・設定します。
        /// </summary>
        public Transform BlockToCarCenterTransform
        {
            get => Transform.FromSource(BlockToCarCenterTransformField.GetValue(Src));
            set => BlockToCarCenterTransformField.SetValue(Src, value?.Src);
        }

        private static FastMethod OnLocationChangedMethod;
        private void OnLocationChanged(object sender, ValueEventArgs<double> e) => OnLocationChangedMethod.Invoke(Src, new object[] { sender, e?.Src });

        /// <summary>
        /// 自列車が移動したときに呼び出されます。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベントデータを格納している <see cref="ValueEventArgs{T}"/> (T は <see cref="double"/>)。<see cref="ValueEventArgs{T}.Value"/> は自列車の走行距離程の変位 [m] です。</param>
        public void OnLocationChanged(VehiclePositioner sender, ValueEventArgs<double> e) => OnLocationChanged(sender?.Src, e);
    }
}
