using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// 双方向に型変換が可能なコンバーターを表します。
    /// </summary>
    /// <typeparam name="T1">変換元・先として使用する 1 つ目の型。</typeparam>
    /// <typeparam name="T2">変換元・先として使用する 2 つ目の型。</typeparam>
    public interface ITwoWayConverter<T1, T2>
    {
        /// <summary>
        /// <typeparamref name="T1"/> 型のオブジェクトを <typeparamref name="T2"/> 型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト。</param>
        /// <returns>変換されたオブジェクト。</returns>
        T2 Convert(T1 value);

        /// <summary>
        /// <typeparamref name="T2"/> 型のオブジェクトを <typeparamref name="T1"/> 型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト。</param>
        /// <returns>変換されたオブジェクト。</returns>
        T1 ConvertBack(T2 value);
    }
}
