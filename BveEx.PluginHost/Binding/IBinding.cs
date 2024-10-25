using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Binding
{
    /// <summary>
    /// ターゲットと任意のデータソースを接続するデータバインディングを表します。
    /// </summary>
    /// <typeparam name="T">バインドするデータの型。</typeparam>
    public interface IBinding<T>
    {
        /// <summary>
        /// バインドソースとして使用するオブジェクトを取得・設定します。
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// バインド方向のモードを取得・設定します。
        /// </summary>
        BindingMode Mode { get; set; }
    }
}
