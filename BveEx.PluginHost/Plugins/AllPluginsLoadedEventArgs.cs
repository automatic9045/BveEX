using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// AtsEX プラグインの読込完了を通知するイベントのデータを表します。
    /// </summary>
    public class AllPluginsLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// 読み込まれた AtsEX プラグインの一覧を取得します。
        /// </summary>
        public IPluginSet Plugins { get; }

        /// <summary>
        /// <see cref="AllPluginsLoadedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="plugins">読み込まれた AtsEX プラグインの一覧。</param>
        public AllPluginsLoadedEventArgs(IPluginSet plugins) : base()
        {
            Plugins = plugins;
        }
    }
}
