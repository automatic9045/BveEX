using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// AtsEX 拡張機能の読込完了を通知するイベントのデータを表します。
    /// </summary>
    public class AllExtensionsLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// 読み込まれた AtsEX 拡張機能の一覧を取得します。
        /// </summary>
        public IExtensionSet Extensions { get; }

        /// <summary>
        /// <see cref="AllExtensionsLoadedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="extensions">読み込まれた AtsEX 拡張機能の一覧。</param>
        public AllExtensionsLoadedEventArgs(IExtensionSet extensions) : base()
        {
            Extensions = extensions;
        }
    }
}
