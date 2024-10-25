using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.Started"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベントデータを格納している <see cref="StartedEventArgs"/>。</param>
    public delegate void StartedEventHandler(StartedEventArgs e);
}
