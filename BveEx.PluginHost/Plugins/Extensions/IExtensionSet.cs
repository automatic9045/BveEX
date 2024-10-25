using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// BveEX 拡張機能の一覧を表します。
    /// </summary>
    public interface IExtensionSet : IEnumerable<PluginBase>
    {
        /// <summary>
        /// 指定した型の BveEX 拡張機能を取得します。
        /// </summary>
        /// <typeparam name="TExtension">BveEX 拡張機能の型。</typeparam>
        /// <returns><typeparamref name="TExtension"/> 型の BveEX 拡張機能。</returns>
        TExtension GetExtension<TExtension>() where TExtension : IExtension;

        /// <summary>
        /// 全ての BveEX 拡張機能が読み込まれ、<see cref="Extensions"/> プロパティが取得可能になると発生します。
        /// </summary>
        /// <remarks>
        /// BveEX 拡張機能以外の BveEX プラグインの場合 (<see cref="PluginBase.PluginType"/> が <see cref="PluginType.Extension"/> 以外の場合)、<see cref="PluginBase.Extensions"/> プロパティは初めから取得可能です。
        /// </remarks>
        event EventHandler AllExtensionsLoaded;
    }
}
