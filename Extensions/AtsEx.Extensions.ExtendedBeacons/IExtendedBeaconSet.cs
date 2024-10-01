using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.Extensions.ExtendedBeacons
{
    /// <summary>
    /// 拡張地上子の一覧を表します。
    /// </summary>
    [Obsolete]
    public interface IExtendedBeaconSet
    {
        /// <summary>
        /// 自列車の通過を検知する地上子の一覧を取得します。
        /// </summary>
        ReadOnlyDictionary<Identifier, ExtendedBeaconBase<PassedEventArgs>> Beacons { get; }

        /// <summary>
        /// 他列車の通過を検知する地上子の一覧を取得します。
        /// </summary>
        ReadOnlyDictionary<Identifier, ExtendedBeaconBase<TrainPassedEventArgs>> TrainObservingBeacons { get; }

        /// <summary>
        /// 先行列車の通過を検知する地上子の一覧を取得します。
        /// </summary>
        ReadOnlyDictionary<Identifier, ExtendedBeaconBase<PassedEventArgs>> PreTrainObservingBeacons { get; }
    }
}
