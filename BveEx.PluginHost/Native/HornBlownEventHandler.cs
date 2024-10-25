using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.HornBlown"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベントデータを格納している <see cref="HornBlownEventArgs"/>。</param>
    public delegate void HornBlownEventHandler(HornBlownEventArgs e);
}
