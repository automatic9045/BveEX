using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// AtsEX 拡張機能の読込完了を通知するイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベントデータを格納している <see cref="AllExtensionsLoadedEventArgs"/>。</param>
    public delegate void AllExtensionsLoadedEventHandler(AllExtensionsLoadedEventArgs e);
}
