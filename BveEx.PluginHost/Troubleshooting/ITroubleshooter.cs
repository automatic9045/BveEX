using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Troubleshooting
{
    /// <summary>
    /// トラブルシューティングプラグインを表します。
    /// </summary>
    public interface ITroubleshooter
    {
        /// <summary>
        /// 例外の解決を試みます。
        /// </summary>
        /// <param name="exception">解決を試みる例外。</param>
        /// <returns><paramref name="exception"/> の解決に成功したかどうか。</returns>
        bool Resolve(Exception exception);
    }
}
