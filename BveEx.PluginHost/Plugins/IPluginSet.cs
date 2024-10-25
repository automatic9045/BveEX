using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// 指定した種類の BveEX プラグインの一覧を取得します。
        /// </summary>
        ReadOnlyDictionary<string, PluginBase> this[PluginType pluginType] { get; }

        /// <summary>
        /// 全ての BveEX プラグインが読み込まれ、<see cref="Plugins"/> プロパティが取得可能になると発生します。
        /// </summary>
        event EventHandler AllPluginsLoaded;
    }
}
