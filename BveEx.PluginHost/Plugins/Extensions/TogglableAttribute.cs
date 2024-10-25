using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// バージョン情報ダイアログから有効・無効が切替可能な AtsEX 拡張機能であることを指定します。
    /// </summary>

    public class TogglableAttribute : Attribute
    {
        /// <summary>
        /// <see cref="TogglableAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public TogglableAttribute()
        {
        }
    }
}
