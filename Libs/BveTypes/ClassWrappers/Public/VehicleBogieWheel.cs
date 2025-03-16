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
    /// 台車の車輪 (軌道接触部) を表します。
    /// </summary>
    public class VehicleBogieWheel : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleBogieWheel>();

            VehicleLocationField = members.GetSourceFieldOf(nameof(VehicleLocation));
            TotalInertiaField = members.GetSourceFieldOf(nameof(MyTrack));
            LocationInCarField = members.GetSourceFieldOf(nameof(LocationInCar));
            XField = members.GetSourceFieldOf(nameof(X));
            YField = members.GetSourceFieldOf(nameof(Y));
            RotationZField = members.GetSourceFieldOf(nameof(RotationZ));
            PositionInBlockField = members.GetSourceFieldOf(nameof(PositionInBlock));

            OnLocationChangedMethod = members.GetSourceMethodOf(nameof(OnLocationChanged));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleBogieWheel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleBogieWheel(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleBogieWheel"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleBogieWheel FromSource(object src) => src is null ? null : new VehicleBogieWheel(src);

        private static FastField VehicleLocationField;
        /// <summary>
        /// 自列車の位置情報を取得・設定します。
        /// </summary>
        public VehicleLocation VehicleLocation
        {
            get => VehicleLocation.FromSource(VehicleLocationField.GetValue(Src));
            set => VehicleLocationField.SetValue(Src, value?.Src);
        }

        private static FastField TotalInertiaField;
        /// <summary>
        /// 自軌道を取得・設定します。
        /// </summary>
        public MyTrack MyTrack
        {
            get => MyTrack.FromSource(TotalInertiaField.GetValue(Src));
            set => TotalInertiaField.SetValue(Src, value?.Src);
        }

        private static FastField LocationInCarField;
        /// <summary>
        /// 自列車の進行方向を Z 軸正方向とした左手系における、この車輪の Z 座標 [m] を取得・設定します。
        /// </summary>
        public double LocationInCar
        {
            get => (double)LocationInCarField.GetValue(Src);
            set => LocationInCarField.SetValue(Src, value);
        }

        private static FastField XField;
        /// <summary>
        /// 自列車の進行方向を Z 軸正方向とした左手系における、この車輪の運動の X 成分 [m] を取得・設定します。
        /// </summary>
        public Physical X
        {
            get => Physical.FromSource(XField.GetValue(Src));
            set => XField.SetValue(Src, value?.Src);
        }

        private static FastField YField;
        /// <summary>
        /// 自列車の進行方向を Z 軸正方向とした左手系における、この車輪の運動の Y 成分 [m] を取得・設定します。
        /// </summary>
        public Physical Y
        {
            get => Physical.FromSource(YField.GetValue(Src));
            set => YField.SetValue(Src, value?.Src);
        }

        private static FastField RotationZField;
        /// <summary>
        /// 自列車の進行方向を Z 軸正方向とした左手系における、この車輪の Z 軸まわりの回転運動 [rad] を取得・設定します。
        /// </summary>
        public Physical RotationZ
        {
            get => Physical.FromSource(RotationZField.GetValue(Src));
            set => RotationZField.SetValue(Src, value?.Src);
        }

        private static FastField PositionInBlockField;
        /// <summary>
        /// 現在自列車が走行しているマップ ブロックの原点に対する、この車輪の位置ベクトル [m] を取得・設定します。
        /// </summary>
        public Vector3 PositionInBlock
        {
            get => (Vector3)PositionInBlockField.GetValue(Src);
            set => PositionInBlockField.SetValue(Src, value);
        }

        private static FastMethod OnLocationChangedMethod;
        private void OnLocationChanged(object sender, ValueEventArgs<double> e) => OnLocationChangedMethod.Invoke(Src, new object[] { sender, e?.Src });

        /// <summary>
        /// 自列車が移動したときに呼び出されます。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベントデータを格納している <see cref="ValueEventArgs{T}"/> (T は <see cref="double"/>)。<see cref="ValueEventArgs{T}.Value"/> は自列車の走行距離程の変位 [m] です。</param>
        public void OnLocationChanged(VehicleBogieWheel sender, ValueEventArgs<double> e) => OnLocationChanged(sender?.Src, e);
    }
}
