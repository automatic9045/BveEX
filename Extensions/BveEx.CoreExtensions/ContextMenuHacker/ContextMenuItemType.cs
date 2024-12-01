using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.ContextMenuHacker
{
    /// <summary>
    /// 右クリックメニューの項目の種類を指定します。
    /// </summary>
    public enum ContextMenuItemType
    {
        /// <summary>
        /// BveEX 本体、または拡張機能によるものであることを指定します。
        /// </summary>
        CoreAndExtensions,

        /// <summary>
        /// プラグイン (拡張機能を除く) によるものであることを指定します。
        /// </summary>
        Plugins,
    }
}
