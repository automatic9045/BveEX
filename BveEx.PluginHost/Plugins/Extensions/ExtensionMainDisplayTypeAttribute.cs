using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Plugins.Extensions
{
    /// <summary>
    /// <see cref="IExtensionSet.GetExtension{TExtension}"/> メソッドからこの AtsEX 拡張機能のメインクラスを取得する時に使用する型を指定します。
    /// </summary>
    /// <seealso cref="HideExtensionMainAttribute"/>
    public class ExtensionMainDisplayTypeAttribute : Attribute
    {
        /// <summary>
        /// <see cref="IExtensionSet.GetExtension{TExtension}"/> メソッドからこの AtsEX 拡張機能のメインクラスを取得する時に使用する型を取得します。
        /// </summary>
        public Type DisplayType { get; }

        /// <summary>
        /// <see cref="ExtensionMainDisplayTypeAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="displayType"><see cref="IExtensionSet.GetExtension{TExtension}"/> メソッドからこの AtsEX 拡張機能のメインクラスを取得する時に使用する型。</param>
        public ExtensionMainDisplayTypeAttribute(Type displayType)
        {
            if (displayType is null) throw new ArgumentNullException(nameof(displayType));

            DisplayType = displayType;
        }
    }
}
