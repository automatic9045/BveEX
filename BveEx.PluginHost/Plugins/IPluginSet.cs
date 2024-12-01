using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// BveEX プラグインの一覧を表します。
    /// </summary>
    public interface IPluginSet : IEnumerable<KeyValuePair<string, PluginBase>>
    {
        /// <summary>
        /// マッププラグインの一覧を、キーが識別子、値がプラグインのインスタンスであるディクショナリとして取得します。
        /// </summary>
        IReadOnlyDictionary<string, PluginBase> MapPlugins { get; }

        /// <summary>
        /// 車両プラグインの一覧を、キーが識別子、値がプラグインのインスタンスであるディクショナリとして取得します。
        /// </summary>
        IReadOnlyDictionary<string, PluginBase> VehiclePlugins { get;  }

        /// <summary>
        /// 全ての BveEX プラグインが読み込まれ、<see cref="Plugins"/> プロパティが取得可能になると発生します。
        /// </summary>
        event EventHandler AllPluginsLoaded;
    }
}
