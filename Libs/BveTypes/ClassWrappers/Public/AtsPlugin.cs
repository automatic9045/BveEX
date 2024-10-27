using System;
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

            PanelArrayGetMethod = members.GetSourcePropertyGetterOf(nameof(PanelArray));
            SoundArrayGetMethod = members.GetSourcePropertyGetterOf(nameof(SoundArray));

            IsPluginLoadedField = members.GetSourceFieldOf(nameof(IsPluginLoaded));
            HModuleField = members.GetSourceFieldOf(nameof(HModule));
            HandlesField = members.GetSourceFieldOf(nameof(Handles));
            _PanelArrayField = members.GetSourceFieldOf(nameof(_PanelArray));
            _SoundArrayField = members.GetSourceFieldOf(nameof(_SoundArray));
            _OldSoundArrayField = members.GetSourceFieldOf(nameof(_OldSoundArray));
            LocationManagerField = members.GetSourceFieldOf(nameof(LocationManager));
            StateStoreField = members.GetSourceFieldOf(nameof(StateStore));
            SectionManagerField = members.GetSourceFieldOf(nameof(SectionManager));
            DoorsField = members.GetSourceFieldOf(nameof(Doors));
            AtsHandlesField = members.GetSourceFieldOf(nameof(AtsHandles));

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
        public AtsPlugin(UserVehicleLocationManager locationManager, KeyProvider keyProvider, HandleSet _0, HandleSet _1, VehicleStateStore vehicleStateStore, SectionManager sectionManager, MapFunctionList beacons, DoorSet doors)
            : this(Constructor.Invoke(new object[] { locationManager?.Src, keyProvider?.Src, _0?.Src, _1?.Src, vehicleStateStore?.Src, sectionManager?.Src, beacons?.Src, doors?.Src }))
        {
        }

        private static FastMethod PanelArrayGetMethod;
        /// <summary>
        /// パネルに渡す値の配列を取得・設定します。
        /// </summary>
        public int[] PanelArray => PanelArrayGetMethod.Invoke(Src, null);

        private static FastMethod SoundArrayGetMethod;
        /// <summary>
        /// サウンドの再生状態を表す値の配列を取得・設定します。
        /// </summary>
        public int[] SoundArray => SoundArrayGetMethod.Invoke(Src, null);

        private static FastField IsPluginLoadedField;
        /// <summary>
        /// 自列車のハンドルのセットを取得・設定します。
        /// </summary>
        public bool IsPluginLoaded
        {
            get => IsPluginLoadedField.GetValue(Src);
            set => IsPluginLoadedField.SetValue(Src, value);
        }

        private static FastField HModuleField;
        /// <summary>
        /// ATS プラグインのモジュール (DLL) へのハンドルを取得・設定します。
        /// </summary>
        public IntPtr HModule
        {
            get => HModuleField.GetValue(Src);
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

        private static FastField _PanelArrayField;
        /// <summary>
        /// <see cref="PanelArray"/> プロパティのバッキングフィールドを取得・設定します。
        /// </summary>
#pragma warning disable IDE1006 // 命名スタイル
        public int[] _PanelArray
#pragma warning restore IDE1006 // 命名スタイル
        {
            get => _PanelArrayField.GetValue(Src);
            set => _PanelArrayField.SetValue(Src, value);
        }

        private static FastField _SoundArrayField;
        /// <summary>
        /// <see cref="SoundArray"/> プロパティのバッキングフィールドを取得・設定します。
        /// </summary>
#pragma warning disable IDE1006 // 命名スタイル
        public int[] _SoundArray
#pragma warning restore IDE1006 // 命名スタイル
        {
            get => _SoundArrayField.GetValue(Src);
            set => _SoundArrayField.SetValue(Src, value);
        }

        private static FastField _OldSoundArrayField;
        /// <summary>
        /// <see cref="OldSoundArray"/> プロパティのバッキングフィールドを取得・設定します。
        /// </summary>
#pragma warning disable IDE1006 // 命名スタイル
        public int[] _OldSoundArray
#pragma warning restore IDE1006 // 命名スタイル
        {
            get => _OldSoundArrayField.GetValue(Src);
            set => _OldSoundArrayField.SetValue(Src, value);
        }

        private static FastField LocationManagerField;
        /// <summary>
        /// 自列車の位置情報に関する処理を行う <see cref="UserVehicleLocationManager"/> を取得・設定します。
        /// </summary>
        public UserVehicleLocationManager LocationManager
        {
            get => UserVehicleLocationManager.FromSource(LocationManagerField.GetValue(Src));
            set => LocationManagerField.SetValue(Src, value?.Src);
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
            get => ClassWrappers.SectionManager.FromSource(SectionManagerField.GetValue(Src));
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
    }
}
