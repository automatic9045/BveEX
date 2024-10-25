using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// マッププラグインの <see cref="PluginBase.Tick(TimeSpan)"/> メソッドの実行結果を表します。
    /// </summary>
    public class MapPluginTickResult : TickResult
    {
        /// <summary>
        /// <see cref="MapPluginTickResult"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MapPluginTickResult()
        {
        }
    }
}
