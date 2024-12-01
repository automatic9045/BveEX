using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// スクリプト言語を指定します。
    /// </summary>
    public enum ScriptLanguage
    {
        /// <summary>
        /// C# スクリプトを指定します。
        /// </summary>
        CSharpScript = 0,

        /// <summary>
        /// IronPython 2 を指定します。
        /// </summary>
        IronPython2,
    }
}
