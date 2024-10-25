using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// バージョン情報ダイアログから有効・無効が切替可能な BveEX 拡張機能を表します。
    /// </summary>
    public interface ITogglableExtension : IExtension
    {
        /// <summary>
        /// この拡張機能が有効かどうかを取得・設定します。
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
