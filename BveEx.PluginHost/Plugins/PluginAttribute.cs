using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// BveEX プラグインの仕様を指定します。
    /// </summary>
    public class PluginAttribute : Attribute
    {
        /// <summary>
        /// この BveEX プラグインの種類を取得します。
        /// </summary>
        public PluginType PluginType { get; }

        /// <summary>
        /// この BveEX プラグインが必要とする BveEX 本体の最低バージョンを取得します。
        /// </summary>
        public Version MinRequiredVersion { get; }

        /// <summary>
        /// <see cref="PluginAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="pluginType">BveEX プラグインの種類。</param>
        /// <param name="minRequiredVersion">この BveEX プラグインが必要とする BveEX 本体の最低バージョンを表す文字列。
        /// テキストは <see cref="Version"/> クラスのコンストラクタがサポートするフォーマットに則っている必要があります。</param>
        public PluginAttribute(PluginType pluginType, string minRequiredVersion = null)
        {
            PluginType = pluginType;
            MinRequiredVersion = minRequiredVersion is null ? null : new Version(minRequiredVersion);
        }
    }
}
