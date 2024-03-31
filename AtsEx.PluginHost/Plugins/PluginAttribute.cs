using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// AtsEX プラグインの仕様を指定します。
    /// </summary>
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    public class PluginAttribute : PluginTypeAttribute
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
    {
        /// <summary>
        /// この AtsEX プラグインが必要とする AtsEX 本体の最低バージョンを取得します。
        /// </summary>
        public Version MinRequiredVersion { get; }

        /// <summary>
        /// <see cref="PluginAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="pluginType">AtsEX プラグインの種類。</param>
        /// <param name="minRequiredVersion">この AtsEX プラグインが必要とする AtsEX 本体の最低バージョンを表す文字列。
        /// テキストは <see cref="Version"/> クラスのコンストラクタがサポートするフォーマットに則っている必要があります。</param>
        public PluginAttribute(PluginType pluginType, string minRequiredVersion = null) : base(pluginType)
        {
            MinRequiredVersion = minRequiredVersion is null ? null : new Version(minRequiredVersion);
        }
    }
}
