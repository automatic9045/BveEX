using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Plugins.Extensions;

using BveEx.Extensions.Native.Input;

namespace BveEx.Extensions.Native
{
    /// <summary>
    /// 通常の ATS プラグインの互換機能を提供します。
    /// </summary>
    public interface INative : IExtension
    {
        /// <summary>
        /// <see cref="INative"/> の各機能が現在使用可能かどうかを取得します。
        /// </summary>
        /// <remarks>
        /// シナリオの実行中以外は使用できません。
        /// </remarks>
        bool IsAvailable { get; }


        /// <summary>
        /// ATS プラグインによって制御可能な、運転台パネルの状態量 (例えば「ats12」など、subjectKey が「ats」から始まる状態量) の一覧を取得します。
        /// </summary>
        IList<int> AtsPanelArray { get; }

        /// <summary>
        /// ATS サウンドの一覧を取得します。
        /// </summary>
        IList<int> AtsSoundArray { get; }

        /// <summary>
        /// BVE が ATS プラグイン向けに提供するキーの入力情報を取得します。
        /// </summary>
        AtsKeySet AtsKeys { get; }

        /// <summary>
        /// BVE が ATS プラグイン向けに提供する車両の性能に関する情報を取得します。
        /// </summary>
        VehicleSpec VehicleSpec { get; }

        /// <summary>
        /// BVE が ATS プラグイン向けに提供する車両の状態に関する情報を取得します。
        /// このプロパティの値はフレーム毎に更新されます。
        /// </summary>
        VehicleState VehicleState { get; }


        /// <summary>
        /// <see cref="INative"/> の各機能が使用可能になったときに呼び出されます。
        /// </summary>
        /// <remarks>
        /// <see cref="AtsPlugin"/> のコンストラクタを実行した直後に呼び出されます。
        /// </remarks>
        event EventHandler Opened;

        /// <summary>
        /// シナリオが終了され、<see cref="INative"/> の各機能が使用不可能になる直前に呼び出されます。
        /// </summary>
        event EventHandler Closed;


        /// <summary>
        /// <see cref="VehicleSpec"/> プロパティへ値が設定された時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの SetVehicleSpec(ATS_VEHICLESPEC vehicleSpec) 関数に相当します。
        /// </remarks>
        event EventHandler VehicleSpecLoaded;

        /// <summary>
        /// シナリオ開始時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの Initialize(int brake) 関数に相当します。
        /// </remarks>
        event EventHandler<StartedEventArgs> Started;

        /// <summary>
        /// 警笛が吹鳴された時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの HornBlow(int hornType) 関数に相当します。
        /// </remarks>
        event EventHandler<HornBlownEventArgs> HornBlown;

        /// <summary>
        /// 客室ドアが開いた時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの DoorOpen() 関数に相当します。
        /// </remarks>
        event EventHandler DoorOpened;

        /// <summary>
        /// 客室ドアが閉まった時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの DoorClose() 関数に相当します。
        /// </remarks>
        event EventHandler DoorClosed;

        /// <summary>
        /// 現在の閉そくの信号インデックスが変化した時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの SetSignal(int signal) 関数に相当します。
        /// </remarks>
        event EventHandler<SignalUpdatedEventArgs> SignalUpdated;

        /// <summary>
        /// 地上子上を通過した時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 通常の ATS プラグインの SendBeaconData(ATS_BEACONDATA beaconData) 関数に相当します。
        /// </remarks>
        event EventHandler<BeaconPassedEventArgs> BeaconPassed;
    }
}
