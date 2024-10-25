using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Binding
{
    /// <summary>
    /// ターゲットとデータソースとのバインディングのデータ同期の方向を指定します。
    /// </summary>
    public enum BindingMode
    {
        /// <summary>
        /// 「データソース→ターゲット」の方向のみデータを同期することを指定します。
        /// </summary>
        OneWay,

        /// <summary>
        /// 「ターゲット→データソース」の方向のみデータを同期することを指定します。
        /// </summary>
        OneWayToSource,

        /// <summary>
        /// ターゲット―データソース間で双方向にデータを同期することを指定します。
        /// </summary>
        TwoWay,
    }
}
