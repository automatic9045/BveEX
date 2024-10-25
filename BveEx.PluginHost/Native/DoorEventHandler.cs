using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.DoorOpened"/> イベント、<see cref="INative.DoorClosed"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベントデータを格納している <see cref="DoorEventArgs"/>。</param>
    public delegate void DoorEventHandler(DoorEventArgs e);
}
