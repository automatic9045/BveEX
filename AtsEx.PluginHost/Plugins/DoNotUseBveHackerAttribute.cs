using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// AtsEX 独自の特殊機能拡張 (<see cref="IBveHacker"/>、マッププラグインなど) を利用しないプラグインであることを指定します。
    /// </summary>
    /// <remarks>
    /// 互換性のために残されている旧名のクラスです。<see cref="PluginAttribute"/> を使用してください。
    /// </remarks>
    [Obsolete]
    public class DoNotUseBveHackerAttribute : Attribute
    {
        /// <summary>
        /// <see cref="DoNotUseBveHackerAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DoNotUseBveHackerAttribute()
        {
        }
    }
}
