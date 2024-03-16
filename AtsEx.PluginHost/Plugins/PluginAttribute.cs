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
        /// この AtsEX プラグインが AtsEX 独自の特殊機能拡張 (<see cref="IBveHacker"/>、マッププラグインなど) を利用するかどうかを取得します。
        /// </summary>
        /// <remarks>
        /// このプロパティの値が <see langword="true"/> の場合、<see cref="PluginBase.BveHacker"/> が取得できなくなる代わりに、
        /// BVE のバージョンの問題で AtsEX の特殊機能拡張の読込に失敗した場合でもシナリオを開始できるようになります。<br/>
        /// マッププラグインではこの属性を指定することはできません。
        /// </remarks>
        public bool UseBveHacker { get; }

        /// <summary>
        /// <see cref="PluginAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="pluginType">AtsEX プラグインの種類。</param>
        /// <param name="minRequiredVersion">この AtsEX プラグインが必要とする AtsEX 本体の最低バージョンを表す文字列。
        /// テキストは <see cref="Version"/> クラスのコンストラクタがサポートするフォーマットに則っている必要があります。</param>
        /// <param name="useBveHacker">この AtsEX プラグインが AtsEX 独自の特殊機能拡張 (<see cref="IBveHacker"/>、マッププラグインなど) を利用するかどうか。</param>
        public PluginAttribute(PluginType pluginType, string minRequiredVersion = null, bool useBveHacker = true) : base(pluginType)
        {
            MinRequiredVersion = minRequiredVersion is null ? null : new Version(minRequiredVersion);
            UseBveHacker = useBveHacker;
        }
    }
}
