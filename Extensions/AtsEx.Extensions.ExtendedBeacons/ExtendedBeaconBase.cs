using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;
using AtsEx.Scripting;

namespace AtsEx.Extensions.ExtendedBeacons
{
    public delegate void PassedEventHandler<TPassedEventArgs>(object sender, TPassedEventArgs e) where TPassedEventArgs : PassedEventArgs;

    /// <summary>
    /// すべての拡張地上子の基本クラスを表します。
    /// </summary>
    /// <typeparam name="TPassedEventArgs"><see cref="Passed"/> イベントで使用する <see cref="EventArgs"/> の型。</typeparam>
    [Obsolete]
    public abstract class ExtendedBeaconBase<TPassedEventArgs> : IExtendedBeacon, ICompilationErrorCheckable where TPassedEventArgs : PassedEventArgs
    {
        protected readonly INative Native;
        protected readonly IBveHacker BveHacker;

        protected readonly IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>> Script;

        /// <summary>
        /// 検知対象の列車がこの地上子上を通過した瞬間に発生します。
        /// </summary>
        public event PassedEventHandler<TPassedEventArgs> Passed;

        /// <summary>
        /// この地上子の設置されている距離程 [m] を取得します。
        /// </summary>
        public double Location => DefinedStatement.From;

        /// <summary>
        /// この地上子の名前を取得します。
        /// </summary>
        public Identifier BeaconName { get; }

        /// <summary>
        /// この地上子の定義に使用されている連続ストラクチャーを取得します。
        /// </summary>
        public IStatement DefinedStatement { get; }

        /// <summary>
        /// この地上子が列車の通過を検知する対象の軌道を指定する <see cref="PluginHost.ExtendedBeacons.ObservingTargetTrack"/> を取得します。
        /// </summary>
        public ObservingTargetTrack ObservingTargetTrack { get; }

        /// <summary>
        /// この地上子が通過を検知する対象の列車を指定する <see cref="PluginHost.ExtendedBeacons.ObservingTargetTrain"/> を取得します。
        /// </summary>
        public ObservingTargetTrain ObservingTargetTrain { get; }

        private protected ExtendedBeaconBase(INative native, IBveHacker bveHacker,
            IStatement definedStatement, Identifier beaconName, ObservingTargetTrack observingTargetTrack, ObservingTargetTrain observingTargetTrain,
            IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>> script)
        {
            Native = native;
            BveHacker = bveHacker;

            DefinedStatement = definedStatement ?? throw new ArgumentNullException(nameof(definedStatement));
            BeaconName = beaconName ?? throw new ArgumentNullException(nameof(beaconName));
            ObservingTargetTrack = observingTargetTrack;
            ObservingTargetTrain = observingTargetTrain;

            Script = script;
        }

        public void CheckCompilationErrors() => _ = Script.GetWithCheckErrors();

        /// <summary>
        /// 検知対象の列車が通過したことをこの <see cref="ExtendedBeaconBase{TPassedEventArgs}"/> オブジェクトに通知します。
        /// </summary>
        protected void NotifyPassed(TPassedEventArgs eventArgs)
        {
            Passed?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public int CompareTo(IExtendedBeacon other) => (int)(Location - other.Location);
    }

    /// <summary>
    /// Repeater 構文によって定義される拡張地上子を表します。
    /// </summary>
    [Obsolete]
    public interface IExtendedBeacon : IComparable<IExtendedBeacon>
    {
        /// <summary>
        /// 設置されている距離程 [m] を取得します。
        /// </summary>
        double Location { get; }

        /// <summary>
        /// この地上子の定義に使用された連続ストラクチャーを取得します。
        /// </summary>
        IStatement DefinedStatement { get; }
    }
}
