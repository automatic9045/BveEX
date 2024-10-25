using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// AtsEX プラグインの読込完了を通知するイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベントデータを格納している <see cref="AllPluginsLoadedEventArgs"/>。</param>
    public delegate void AllPluginsLoadedEventHandler(AllPluginsLoadedEventArgs e);
}
