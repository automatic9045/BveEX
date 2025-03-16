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
    /// 自列車の位置情報を表します。
    /// </summary>
    public class VehicleLocation : Tachogenerator
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleLocation>();

            Constructor = members.GetSourceConstructor();

            BlockIndexField = members.GetSourceFieldOf(nameof(BlockIndex));
            LocationInBlockField = members.GetSourceFieldOf(nameof(LocationInBlock));
            LimitLocationField = members.GetSourceFieldOf(nameof(LimitLocation));
            LocationField = members.GetSourceFieldOf(nameof(Location));

            FastEvent blockChangedEvent = members.GetSourceEventOf(nameof(BlockChanged));
            FastEvent locationChangedEvent = members.GetSourceEventOf(nameof(LocationChanged));

            BlockChangedEvent = new WrapperEvent<EventHandler<ValueEventArgs<int>>>(blockChangedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), ValueEventArgs<int>.FromSource(e)));
            LocationChangedEvent = new WrapperEvent<EventHandler<ValueEventArgs<double>>>(locationChangedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), ValueEventArgs<double>.FromSource(e)));

            OnBlockChangedMethod = members.GetSourceMethodOf(nameof(OnBlockChanged));
            OnLocationChangedMethod = members.GetSourceMethodOf(nameof(OnLocationChanged));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            SetLocationMethod = members.GetSourceMethodOf(nameof(SetLocation));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleLocation"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleLocation(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleLocation"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new VehicleLocation FromSource(object src) => src is null ? null : new VehicleLocation(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="VehicleLocation"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public VehicleLocation() : base(Constructor.Invoke(null))
        {
        }

        private static FastField BlockIndexField;
        /// <summary>
        /// 現在走行しているストラクチャーブロックのインデックスを取得・設定します。
        /// </summary>
        public int BlockIndex
        {
            get => (int)BlockIndexField.GetValue(Src);
            set => BlockIndexField.SetValue(Src, value);
        }

        private static FastField LocationInBlockField;
        /// <summary>
        /// 現在走行しているストラクチャーブロックの原点を基準とした走行位置 [m] を取得・設定します。
        /// </summary>
        public double LocationInBlock
        {
            get => (double)LocationInBlockField.GetValue(Src);
            set => LocationInBlockField.SetValue(Src, value);
        }

        private static FastField LimitLocationField;
        /// <summary>
        /// ストラクチャーが設置される限界の距離程 [m] を取得・設定します。通常は最後の駅の 10km 先の位置になります。
        /// </summary>
        public int LimitLocation
        {
            get => (int)LimitLocationField.GetValue(Src);
            set => LimitLocationField.SetValue(Src, value);
        }

        private static FastField LocationField;
        /// <summary>
        /// 現在の距離程 [m] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// このプロパティに値を直接設定すると、テレポート時の処理が行われません。通常は <see cref="SetLocation(double, bool)"/> メソッドを使用してください。
        /// </remarks>
        public double Location
        {
            get => (double)LocationField.GetValue(Src);
            set => LocationField.SetValue(Src, value);
        }

        private static WrapperEvent<EventHandler<ValueEventArgs<int>>> BlockChangedEvent;
        /// <summary>
        /// 走行しているストラクチャーブロックが変わったときに発生します。
        /// </summary>
        public event EventHandler<ValueEventArgs<int>> BlockChanged
        {
            add => BlockChangedEvent.Add(Src, value);
            remove => BlockChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="BlockChanged"/> イベントを実行します。
        /// </summary>
        public void BlockChanged_Invoke(ValueEventArgs<int> args) => BlockChangedEvent.Invoke(Src, args);

        private static WrapperEvent<EventHandler<ValueEventArgs<double>>> LocationChangedEvent;
        /// <summary>
        /// 走行している距離程が変わったときに発生します。
        /// </summary>
        public event EventHandler<ValueEventArgs<double>> LocationChanged
        {
            add => LocationChangedEvent.Add(Src, value);
            remove => LocationChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="LocationChanged"/> イベントを実行します。
        /// </summary>
        public void LocationChanged_Invoke(ValueEventArgs<double> args) => LocationChangedEvent.Invoke(Src, args);

        private static FastMethod OnBlockChangedMethod;
        /// <summary>
        /// 走行しているストラクチャーブロックが変わったことを通知します。
        /// </summary>
        public void OnBlockChanged(int delta) => OnBlockChangedMethod.Invoke(Src, new object[] { delta });

        private static FastMethod OnLocationChangedMethod;
        /// <summary>
        /// 走行している距離程が変わったことを通知します。
        /// </summary>
        public void OnLocationChanged(double delta) => OnLocationChangedMethod.Invoke(Src, new object[] { delta });

        private static FastMethod TickMethod;
        /// <summary>
        /// 毎フレーム呼び出され、速度および距離程の値を更新します。
        /// </summary>
        /// <param name="acceleration">加速度 [m/s^2]。</param>
        /// <param name="resistanceAcceleration">抵抗加速度 [m/s^2]。</param>
        /// <param name="gradient">勾配。</param>
        /// <param name="elapsedSeconds">前フレームからの経過時間 [s]。</param>
        public void Tick(double acceleration, double resistanceAcceleration, double gradient, double elapsedSeconds)
            => TickMethod.Invoke(Src, new object[] { acceleration, resistanceAcceleration, gradient, elapsedSeconds });

        private static FastMethod SetLocationMethod;
        /// <summary>
        /// 自車両の位置を設定します。
        /// </summary>
        /// <param name="location">設定する自車両の位置 [m]。</param>
        /// <param name="skipIfNoChange">指定された位置が現在と変わらない場合、処理をスキップするか。</param>
        /// <remarks>
        /// 自車両の位置を取得するには <see cref="Location"/> プロパティを使用してください。
        /// </remarks>
        /// <seealso cref="Location"/>
        public void SetLocation(double location, bool skipIfNoChange) => SetLocationMethod.Invoke(Src, new object[] { location, skipIfNoChange });
    }
}
