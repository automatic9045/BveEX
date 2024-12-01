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
    /// 車掌を表します。
    /// </summary>
    public class Conductor : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Conductor>();

            TimeManagerField = members.GetSourceFieldOf(nameof(TimeManager));
            LocationField = members.GetSourceFieldOf(nameof(Location));
            SoundsField = members.GetSourceFieldOf(nameof(Sounds));
            StationsField = members.GetSourceFieldOf(nameof(Stations));
            SectionManagerField = members.GetSourceFieldOf(nameof(SectionManager));
            DoorsField = members.GetSourceFieldOf(nameof(Doors));
            PassengerField = members.GetSourceFieldOf(nameof(Passenger));
            StoppedToDoorOpeningMillisecondsField = members.GetSourceFieldOf(nameof(StoppedToDoorOpeningMilliseconds));

            FixStopPositionRequestedEvent = members.GetSourceEventOf(nameof(FixStopPositionRequested));
            DepartureRequestedEvent = members.GetSourceEventOf(nameof(DepartureRequested));

            Constructor = members.GetSourceConstructor();

            OnJumpedMethod = members.GetSourceMethodOf(nameof(OnJumped));
            OnDoorStateChangedMethod = members.GetSourceMethodOf(nameof(OnDoorStateChanged));
            OnTickMethod = members.GetSourceMethodOf(nameof(OnTick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Conductor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Conductor(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Conductor"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Conductor FromSource(object src) => src is null ? null : new Conductor(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="Conductor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Conductor(TimeManager timeManager, VehicleLocation location, SoundSet sounds, MapObjectList stations, SectionManager sectionManager, DoorSet doors, Passenger passenger)
            : this(Constructor.Invoke(new object[] { timeManager, location, sounds, stations, sectionManager, doors, passenger }))
        {
        }

        private static FastField TimeManagerField;
        /// <summary>
        /// シナリオに関連付けられた <see cref="BveTypes.ClassWrappers.TimeManager"/> のインスタンスを取得・設定します。
        /// </summary>
        public TimeManager TimeManager
        {
            get => ClassWrappers.TimeManager.FromSource(TimeManagerField.GetValue(Src));
            set => TimeManagerField.SetValue(Src, value?.Src);
        }

        private static FastField LocationField;
        /// <summary>
        /// シナリオに関連付けられた <see cref="VehicleLocation"/> のインスタンスを取得・設定します。
        /// </summary>
        public VehicleLocation Location
        {
            get => VehicleLocation.FromSource(LocationField.GetValue(Src));
            set => LocationField.SetValue(Src, value?.Src);
        }

        private static FastField SoundsField;
        /// <summary>
        /// シナリオに関連付けられた <see cref="BveTypes.ClassWrappers.SoundSet"/> のインスタンスを取得・設定します。
        /// </summary>
        public SoundSet Sounds
        {
            get => SoundSet.FromSource(SoundsField.GetValue(Src));
            set => SoundsField.SetValue(Src, value?.Src);
        }

        private static FastField StationsField;
        /// <summary>
        /// 停車場のリストを取得・設定します。
        /// </summary>
        public MapObjectList Stations
        {
            get => MapObjectList.FromSource(StationsField.GetValue(Src));
            set => StationsField.SetValue(Src, value?.Src);
        }

        private static FastField SectionManagerField;
        /// <summary>
        /// シナリオに関連付けられた <see cref="BveTypes.ClassWrappers.SectionManager"/> のインスタンスを取得・設定します。
        /// </summary>
        public SectionManager SectionManager
        {
            get => ClassWrappers.SectionManager.FromSource(SectionManagerField.GetValue(Src));
            set => SectionManagerField.SetValue(Src, value?.Src);
        }

        private static FastField DoorsField;
        /// <summary>
        /// シナリオに関連付けられた <see cref="DoorSet"/> のインスタンスを取得・設定します。
        /// </summary>
        public DoorSet Doors
        {
            get => DoorSet.FromSource(DoorsField.GetValue(Src));
            set => DoorsField.SetValue(Src, value?.Src);
        }

        private static FastField PassengerField;
        /// <summary>
        /// シナリオに関連付けられた <see cref="BveTypes.ClassWrappers.Passenger"/> のインスタンスを取得・設定します。
        /// </summary>
        public Passenger Passenger
        {
            get => ClassWrappers.Passenger.FromSource(PassengerField.GetValue(Src));
            set => PassengerField.SetValue(Src, value?.Src);
        }

        private static FastField StoppedToDoorOpeningMillisecondsField;
        /// <summary>
        /// 停車場に停車してからドアを開くまでの時間 [ms] を取得・設定します。
        /// </summary>
        public int StoppedToDoorOpeningMilliseconds
        {
            get => (int)StoppedToDoorOpeningMillisecondsField.GetValue(Src);
            set => StoppedToDoorOpeningMillisecondsField.SetValue(Src, value);
        }

        /// <summary>
        /// 停車場に停車してからドアを開くまでの時間を取得・設定します。
        /// </summary>
        public TimeSpan StoppedToDoorOpening
        {
            get => TimeSpan.FromMilliseconds(StoppedToDoorOpeningMilliseconds);
            set => StoppedToDoorOpeningMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastEvent FixStopPositionRequestedEvent;
        /// <summary>
        /// 停止位置の修正が要求されたときに発生します。
        /// </summary>
        public event EventHandler FixStopPositionRequested
        {
            add => FixStopPositionRequestedEvent.Add(Src, value);
            remove => FixStopPositionRequestedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="FixStopPositionRequested"/> イベントを実行します。
        /// </summary>
        public void FixStopPositionRequested_Invoke() => FixStopPositionRequestedEvent.Invoke(Src, new object[] { (object)Src, EventArgs.Empty });

        private static FastEvent DepartureRequestedEvent;
        /// <summary>
        /// 車側灯の滅灯を確認したとき、つまり全てのドアが閉まりきったときに発生します。
        /// </summary>
        public event EventHandler DepartureRequested
        {
            add => DepartureRequestedEvent.Add(Src, value);
            remove => DepartureRequestedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="DepartureRequested"/> イベントを実行します。
        /// </summary>
        public void DepartureRequested_Invoke() => DepartureRequestedEvent.Invoke(Src, new object[] { (object)Src, EventArgs.Empty });

        private static FastMethod OnJumpedMethod;
        /// <summary>
        /// このインスタンスに自列車がテレポートしたことを通知します。
        /// </summary>
        /// <param name="nextStationIndex">ジャンプ先の距離程における次駅のインデックス。</param>
        /// <param name="isDoorClosed">ドアが閉まっているかどうか。</param>
        public void OnJumped(int nextStationIndex, bool isDoorClosed)
            => OnJumpedMethod.Invoke(Src, new object[] { nextStationIndex, isDoorClosed });

        private static FastMethod OnDoorStateChangedMethod;
        /// <summary>
        /// このインスタンスにドアの状態が変更されたことを通知します。
        /// </summary>
        /// <param name="sender">通知元のインスタンス。</param>
        /// <param name="e">イベントデータを格納する <see cref="EventArgs"/>。</param>
        public void OnDoorStateChanged(object sender, EventArgs e)
            => OnDoorStateChangedMethod.Invoke(Src, new object[] { sender, e });

        private static FastMethod OnTickMethod;
        /// <summary>
        /// このインスタンスに 1 フレームが経過したことを通知します。
        /// </summary>
        /// <param name="sender">通知元のインスタンス。</param>
        /// <param name="e">イベントデータを格納する <see cref="EventArgs"/>。</param>
        public void OnTick(object sender, EventArgs e)
            => OnTickMethod.Invoke(Src, new object[] { sender, e });
    }
}
