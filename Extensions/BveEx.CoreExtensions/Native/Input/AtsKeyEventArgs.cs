using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Input;

namespace BveEx.Extensions.Native.Input
{
    /// <summary>
    /// <see cref="AtsKeySet.AnyKeyPressed"/>、<see cref="AtsKeySet.AnyKeyReleased"/> イベントのデータを提供します。
    /// </summary>
    public class AtsKeyEventArgs : EventArgs
    {
        /// <summary>
        /// イベントの発生元であるキーの名前を取得します。
        /// </summary>
        public AtsKeyName KeyName { get; }

        /// <summary>
        /// イベントの発生元であるキーを取得します。
        /// </summary>
        public AtsKey Key { get; }

        /// <summary>
        /// <see cref="AtsKeyEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="keyName">イベントの発生元であるキーの名前。</param>
        /// <param name="key">イベントの発生元であるキー。</param>
        public AtsKeyEventArgs(AtsKeyName keyName, AtsKey key)
        {
            KeyName = keyName;
            Key = key;
        }
    }
}
