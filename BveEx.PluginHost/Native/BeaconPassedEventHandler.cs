using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.BeaconPassed"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベントデータを格納している <see cref="BeaconPassedEventArgs"/>。</param>
    public delegate void BeaconPassedEventHandler(BeaconPassedEventArgs e);
}
