﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ATS プラグインを表します。
    /// </summary>
    public class AtsPlugin : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<AtsPlugin>();

            Constructor = members.GetSourceConstructor();

            IsPluginLoadedField = members.GetSourceFieldOf(nameof(IsPluginLoaded));
            HModuleField = members.GetSourceFieldOf(nameof(HModule));
            HandlesField = members.GetSourceFieldOf(nameof(Handles));
            PanelArrayField = members.GetSourceFieldOf(nameof(PanelArray));
            SoundArrayField = members.GetSourceFieldOf(nameof(SoundArray));
            OldSoundArrayField = members.GetSourceFieldOf(nameof(OldSoundArray));
            LocationField = members.GetSourceFieldOf(nameof(Location));
            StateStoreField = members.GetSourceFieldOf(nameof(StateStore));
            SectionManagerField = members.GetSourceFieldOf(nameof(SectionManager));
            DoorsField = members.GetSourceFieldOf(nameof(Doors));
            AtsHandlesField = members.GetSourceFieldOf(nameof(AtsHandles));

            FastEvent loopSoundRequestedEvent = members.GetSourceEventOf(nameof(LoopSoundRequested));
            FastEvent playSoundRequestedEvent = members.GetSourceEventOf(nameof(PlaySoundRequested));

            LoopSoundRequestedEvent = new WrapperEvent<EventHandler<AtsSoundEventArgs>>(loopSoundRequestedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), AtsSoundEventArgs.FromSource(e)));
            PlaySoundRequestedEvent = new WrapperEvent<EventHandler<AtsSoundEventArgs>>(playSoundRequestedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), AtsSoundEventArgs.FromSource(e)));

            OnSetBeaconDataMethod = members.GetSourceMethodOf(nameof(OnSetBeaconData));
            OnKeyDownMethod = members.GetSourceMethodOf(nameof(OnKeyDown));
            OnKeyUpMethod = members.GetSourceMethodOf(nameof(OnKeyUp));
            OnDoorStateChangedMethod = members.GetSourceMethodOf(nameof(OnDoorStateChanged));
            OnSetSignalMethod = members.GetSourceMethodOf(nameof(OnSetSignal));
            OnSetReverserMethod = members.GetSourceMethodOf(nameof(OnSetReverser));
            OnSetBrakeMethod = members.GetSourceMethodOf(nameof(OnSetBrake));
            OnSetPowerMethod = members.GetSourceMethodOf(nameof(OnSetPower));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
            LoadLibraryMethod = members.GetSourceMethodOf(nameof(LoadLibrary));
            OnSetVehicleSpecMethod = members.GetSourceMethodOf(nameof(OnSetVehicleSpec));
            OnInitializeMethod = members.GetSourceMethodOf(nameof(OnInitialize));
            OnElapseMethod = members.GetSourceMethodOf(nameof(OnElapse));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AtsPlugin"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected AtsPlugin(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="AtsPlugin"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static AtsPlugin FromSource(object src) => src is null ? null : new AtsPlugin(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="AtsPlugin"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">自列車の位置情報。</param>
        /// <param name="inputManager">キー入力に関する情報。</param>
        /// <param name="handles">自列車のノッチ情報。</param>
        /// <param name="atsHandles">ATS による指示を適用した自列車のノッチ情報。</param>
        /// <param name="vehicleStateStore">自列車の状態に関する情報。</param>
        /// <param name="sectionManager">閉塞の制御に関する情報。</param>
        /// <param name="beacons">地上子の一覧。</param>
        /// <param name="doors">自列車のドアの一覧。</param>
        public AtsPlugin(VehicleLocation location, InputManager inputManager, HandleSet handles, HandleSet atsHandles, VehicleStateStore vehicleStateStore, SectionManager sectionManager, MapFunctionList beacons, DoorSet doors)
            : this(Constructor.Invoke(new object[] { location?.Src, inputManager?.Src, handles?.Src, atsHandles?.Src, vehicleStateStore?.Src, sectionManager?.Src, beacons?.Src, doors?.Src }))
        {
        }

        private static FastField IsPluginLoadedField;
        /// <summary>
        /// 何らかの ATS プラグインが読み込まれているかどうかを取得・設定します。
        /// </summary>
        public bool IsPluginLoaded
        {
            get => (bool)IsPluginLoadedField.GetValue(Src);
            set => IsPluginLoadedField.SetValue(Src, value);
        }

        private static FastField HModuleField;
        /// <summary>
        /// ATS プラグインのモジュール (DLL) へのハンドルを取得・設定します。
        /// </summary>
        public IntPtr HModule
        {
            get => (IntPtr)HModuleField.GetValue(Src);
            set => HModuleField.SetValue(Src, value);
        }

        private static FastField HandlesField;
        /// <summary>
        /// 自列車のノッチ情報を取得・設定します。
        /// </summary>
        public HandleSet Handles
        {
            get => HandleSet.FromSource(HandlesField.GetValue(Src));
            set => HandlesField.SetValue(Src, value?.Src);
        }

        private static FastField PanelArrayField;
        /// <summary>
        /// パネルに渡す値の配列を取得・設定します。
        /// </summary>
        public int[] PanelArray
        {
            get => PanelArrayField.GetValue(Src) as int[];
            set => PanelArrayField.SetValue(Src, value);
        }

        private static FastField SoundArrayField;
        /// <summary>
        /// サウンドの再生状態を表す値の配列を取得・設定します。
        /// </summary>
        public int[] SoundArray
        {
            get => SoundArrayField.GetValue(Src) as int[];
            set => SoundArrayField.SetValue(Src, value);
        }

        private static FastField OldSoundArrayField;
        /// <summary>
        /// 1 フレーム前におけるサウンドの再生状態を表す値の配列を取得・設定します。
        /// </summary>
        public int[] OldSoundArray
        {
            get => OldSoundArrayField.GetValue(Src) as int[];
            set => OldSoundArrayField.SetValue(Src, value);
        }

        private static FastField LocationField;
        /// <summary>
        /// 自列車の位置情報を取得・設定します。
        /// </summary>
        public VehicleLocation Location
        {
            get => VehicleLocation.FromSource(LocationField.GetValue(Src));
            set => LocationField.SetValue(Src, value?.Src);
        }

        private static FastField StateStoreField;
        /// <summary>
        /// 自列車の状態に関する情報を提供する <see cref="VehicleStateStore"/> を取得・設定します。
        /// </summary>
        public VehicleStateStore StateStore
        {
            get => VehicleStateStore.FromSource(StateStoreField.GetValue(Src));
            set => StateStoreField.SetValue(Src, value?.Src);
        }

        private static FastField SectionManagerField;
        /// <summary>
        /// 閉塞を制御するための機能を提供する <see cref="ClassWrappers.SectionManager"/> を取得・設定します。
        /// </summary>
        public SectionManager SectionManager
        {
            get => SectionManager.FromSource(SectionManagerField.GetValue(Src));
            set => SectionManagerField.SetValue(Src, value?.Src);
        }

        private static FastField DoorsField;
        /// <summary>
        /// 自列車のドアのセットを取得・設定します。
        /// </summary>
        public DoorSet Doors
        {
            get => DoorSet.FromSource(DoorsField.GetValue(Src));
            set => DoorsField.SetValue(Src, value?.Src);
        }

        private static FastField AtsHandlesField;
        /// <summary>
        /// ATS による指示を適用した自列車のノッチ情報を取得・設定します。
        /// </summary>
        public HandleSet AtsHandles
        {
            get => HandleSet.FromSource(AtsHandlesField.GetValue(Src));
            set => AtsHandlesField.SetValue(Src, value?.Src);
        }

        private static WrapperEvent<EventHandler<AtsSoundEventArgs>> LoopSoundRequestedEvent;
        /// <summary>
        /// ATS サウンドのループ再生が要求されたときに発生します。
        /// </summary>
        public event EventHandler<AtsSoundEventArgs> LoopSoundRequested
        {
            add => LoopSoundRequestedEvent.Add(Src, value);
            remove => LoopSoundRequestedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="LoopSoundRequested"/> イベントを実行します。
        /// </summary>
        public void LoopSoundRequested_Invoke(AtsSoundEventArgs args) => LoopSoundRequestedEvent.Invoke(Src, args);

        private static WrapperEvent<EventHandler<AtsSoundEventArgs>> PlaySoundRequestedEvent;
        /// <summary>
        /// ATS サウンドの再生が要求されたときに発生します。
        /// </summary>
        public event EventHandler<AtsSoundEventArgs> PlaySoundRequested
        {
            add => PlaySoundRequestedEvent.Add(Src, value);
            remove => PlaySoundRequestedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="PlaySoundRequested"/> イベントを実行します。
        /// </summary>
        public void PlaySoundRequested_Invoke(AtsSoundEventArgs args) => PlaySoundRequestedEvent.Invoke(Src, args);

        private static FastMethod OnSetBeaconDataMethod;
        /// <summary>
        /// プラグインに地上子を越えたことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnSetBeaconData(object sender, ObjectPassedEventArgs e) => OnSetBeaconDataMethod.Invoke(Src, new object[] { sender, e?.Src });

        private static FastMethod OnKeyDownMethod;
        /// <summary>
        /// プラグインに ATS キー、または警笛キーが押されたことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnKeyDown(object sender, InputEventArgs e) => OnKeyDownMethod.Invoke(Src, new object[] { sender, e });

        private static FastMethod OnKeyUpMethod;
        /// <summary>
        /// プラグインに ATS キーが離されたことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnKeyUp(object sender, InputEventArgs e) => OnKeyUpMethod.Invoke(Src, new object[] { sender, e });

        private static FastMethod OnDoorStateChangedMethod;
        /// <summary>
        /// プラグインに客室ドアの状態が変化したことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnDoorStateChanged(object sender, EventArgs e) => OnDoorStateChangedMethod.Invoke(Src, new object[] { sender, e });

        private static FastMethod OnSetSignalMethod;
        /// <summary>
        /// プラグインに現在の閉塞の信号が変化したことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnSetSignal(object sender, EventArgs e) => OnSetSignalMethod.Invoke(Src, new object[] { sender, e });

        private static FastMethod OnSetReverserMethod;
        /// <summary>
        /// プラグインにレバーサーが扱われたことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnSetReverser(object sender, EventArgs e) => OnSetReverserMethod.Invoke(Src, new object[] { sender, e });

        private static FastMethod OnSetBrakeMethod;
        /// <summary>
        /// プラグインにブレーキが扱われたことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnSetBrake(object sender, ValueEventArgs<int> e) => OnSetBrakeMethod.Invoke(Src, new object[] { sender, e?.Src });

        private static FastMethod OnSetPowerMethod;
        /// <summary>
        /// プラグインに主ハンドルが扱われたことを通知します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベント データを格納しているオブジェクト。</param>
        public void OnSetPower(object sender, ValueEventArgs<int> e) => OnSetPowerMethod.Invoke(Src, new object[] { sender, e?.Src });

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);

        private static FastMethod LoadLibraryMethod;
        /// <summary>
        /// ATS プラグインのライブラリ (DLL) を読み込みます。
        /// </summary>
        /// <param name="path"></param>
        public void LoadLibrary(string path) => LoadLibraryMethod.Invoke(Src, new object[] { path });

        private static FastMethod OnSetVehicleSpecMethod;
        /// <summary>
        /// プラグインに車両スペックを設定します。
        /// </summary>
        /// <param name="notchInfo">ノッチの情報。</param>
        /// <param name="carCount">編成両数。</param>
        public void OnSetVehicleSpec(NotchInfo notchInfo, int carCount) => OnSetVehicleSpecMethod.Invoke(Src, new object[] { notchInfo?.Src, carCount });

        private static FastMethod OnInitializeMethod;
        /// <summary>
        /// プラグインにゲームが開始されたことを通知します。
        /// </summary>
        /// <param name="brakePosition">ゲーム開始時のブレーキ弁の状態。</param>
        public void OnInitialize(BrakePosition brakePosition) => OnInitializeMethod.Invoke(Src, new object[] { brakePosition });

        private static FastMethod OnElapseMethod;
        /// <summary>
        /// プラグインに 1 フレームが経過したことを通知します。
        /// </summary>
        /// <param name="time">現在時刻。</param>
        public void OnElapse(int time) => OnElapseMethod.Invoke(Src, new object[] { time });


        /// <summary>
        /// <see cref="PlaySoundRequested"/>、<see cref="LoopSoundRequested"/> イベントのデータを提供します。
        /// </summary>
        public class AtsSoundEventArgs : ClassWrapperBase
        {
            [InitializeClassWrapper]
            private static void Initialize(BveTypeSet bveTypes)
            {
                ClassMemberSet members = bveTypes.GetClassInfoOf<AtsSoundEventArgs>();

                Constructor = members.GetSourceConstructor();

                SoundIndexField = members.GetSourceFieldOf(nameof(SoundIndex));
                VolumeField = members.GetSourceFieldOf(nameof(Volume));
            }

            /// <summary>
            /// オリジナル オブジェクトから <see cref="AtsSoundEventArgs"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="src">ラップするオリジナル オブジェクト。</param>
            protected AtsSoundEventArgs(object src) : base(src)
            {
            }

            private static FastConstructor Constructor;
            /// <summary>
            /// <see cref="ObjectPassedEventArgs"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="soundIndex">通過したマップオブジェクト。</param>
            /// <param name="volume">下げる音量の符号付き大きさ [B]。0 または負の値で指定してください。</param>
            public AtsSoundEventArgs(int soundIndex, int volume)
                : this(Constructor.Invoke(new object[] { soundIndex, volume }))
            {
            }

            /// <summary>
            /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
            /// </summary>
            /// <param name="src">ラップするオリジナル オブジェクト。</param>
            /// <returns>オリジナル オブジェクトをラップした <see cref="AtsSoundEventArgs"/> クラスのインスタンス。</returns>
            [CreateClassWrapperFromSource]
            public static AtsSoundEventArgs FromSource(object src) => src is null ? null : new AtsSoundEventArgs(src);

            private static FastField SoundIndexField;
            /// <summary>
            /// 操作対象となる ATS サウンドのインデックスを取得・設定します。
            /// </summary>
            public int SoundIndex
            {
                get => (int)SoundIndexField.GetValue(Src);
                set => SoundIndexField.SetValue(Src, value);
            }

            private static FastField VolumeField;
            /// <summary>
            /// 下げる音量の符号付き大きさ [B] を取得・設定します。
            /// </summary>
            /// <remarks>
            /// 0 または負の値で指定してください。
            /// </remarks>
            public int Volume
            {
                get => (int)VolumeField.GetValue(Src);
                set => VolumeField.SetValue(Src, value);
            }
        }
    }
}
