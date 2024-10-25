using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// AtsEX 拡張機能の一覧を表します。
    /// </summary>
    public interface IExtensionSet : IEnumerable<PluginBase>
    {
        /// <summary>
        /// 指定した型の AtsEX 拡張機能を取得します。
        /// </summary>
        /// <typeparam name="TExtension">AtsEX 拡張機能の型。</typeparam>
        /// <returns><typeparamref name="TExtension"/> 型の AtsEX 拡張機能。</returns>
        TExtension GetExtension<TExtension>() where TExtension : IExtension;

        /// <summary>
        /// 全ての AtsEX 拡張機能が読み込まれ、<see cref="Extensions"/> プロパティが取得可能になると発生します。
        /// </summary>
        /// <remarks>
        /// AtsEX 拡張機能以外の AtsEX プラグインの場合 (<see cref="PluginBase.PluginType"/> が <see cref="PluginType.Extension"/> 以外の場合)、<see cref="PluginBase.Extensions"/> プロパティは初めから取得可能です。
        /// </remarks>
        event EventHandler AllExtensionsLoaded;
    }
}
