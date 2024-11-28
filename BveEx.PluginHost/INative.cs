using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Input.Native;
using BveEx.PluginHost.Native;
using BveEx.PluginHost.Panels.Native;
using BveEx.PluginHost.Sound.Native;

namespace BveEx.PluginHost
{
    /// <summary>
    /// BVE が標準で提供する ATS プラグイン向けの機能をラップします。
    /// </summary>
    public interface INative
    {
        /// <summary>
        /// BveEX プラグインから、ATS プラグインによって制御可能な運転台パネルの状態量 (例えば「ats12」など、subjectKey が「ats」から始まる状態量) を操作するための機能を提供する
        /// <see cref="IAtsPanelValueSet"/> を取得します。
        /// </summary>
        IAtsPanelValueSet AtsPanelValues { get; }

        /// <summary>
        /// BveEX プラグインから ATS サウンドを再生するための機能を提供する <see cref="IAtsSoundSet"/> を取得します。
        /// </summary>
        IAtsSoundSet AtsSounds { get; }


        /// <summary>
        /// BVE が ATS プラグイン向けに提供するキーの入力情報を取得します。
        /// </summary>
        INativeKeySet NativeKeys { get; }


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
        /// シナリオ開始時に発生します。ネイティブ ATS プラグインの Initialize(int brake) に当たります。
        /// </summary>
        event StartedEventHandler Started;

        /// <summary>
        /// 警笛が吹鳴された時に発生します。ネイティブ ATS プラグインの HornBlow(int hornType) に当たります。
        /// </summary>
        event HornBlownEventHandler HornBlown;

        /// <summary>
        /// 客室ドアが開いた時に発生します。ネイティブ ATS プラグインの DoorOpen() に当たります。
        /// </summary>
        event DoorEventHandler DoorOpened;

        /// <summary>
        /// 客室ドアが閉まった時に発生します。ネイティブ ATS プラグインの DoorClose() に当たります。
        /// </summary>
        event DoorEventHandler DoorClosed;

        /// <summary>
        /// 現在の閉そくの信号インデックスが変化した時に発生します。ネイティブ ATS プラグインの SetSignal(int signal) に当たります。
        /// </summary>
        event SignalUpdatedEventHandler SignalUpdated;

        /// <summary>
        /// 地上子上を通過した時に発生します。ネイティブ ATS プラグインの SendBeaconData(ATS_BEACONDATA beaconData) に当たります。
        /// </summary>
        event BeaconPassedEventHandler BeaconPassed;
    }
}
