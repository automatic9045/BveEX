using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Input.Native
{
    /// <summary>
    /// <see cref="INativeKeySet.AnyKeyPressed"/>、<see cref="INativeKeySet.AnyKeyReleased"/> イベントのデータを提供します。
    /// </summary>
    public class NativeKeyEventArgs : EventArgs
    {
        /// <summary>
        /// イベントの発生元であるキーの名前を取得します。
        /// </summary>
        public NativeAtsKeyName KeyName { get; }

        /// <summary>
        /// イベントの発生元であるキーを取得します。
        /// </summary>
        public KeyBase Key { get; }

        /// <summary>
        /// <see cref="NativeKeyEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="keyName">イベントの発生元であるキーの名前。</param>
        /// <param name="key">イベントの発生元であるキー。</param>
        public NativeKeyEventArgs(NativeAtsKeyName keyName, KeyBase key)
        {
            KeyName = keyName;
            Key = key;
        }
    }
}
