using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// 拡張機能の <see cref="PluginBase.Tick(TimeSpan)"/> メソッドの実行結果を表します。
    /// </summary>
    public class ExtensionTickResult : TickResult
    {
        /// <summary>
        /// <see cref="ExtensionTickResult"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ExtensionTickResult()
        {
        }
    }
}
