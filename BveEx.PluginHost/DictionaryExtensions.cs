using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost
{
    /// <summary>
    /// <see cref="IDictionary{TKey, TValue}"/> の LINQ 操作機能を拡張します。
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 指定した値に関連付けられているキーを取得します。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> のキーの型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> の値の型。</typeparam>
        /// <param name="source">返されるキーが含まれる <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="key">このメソッドから制御が戻るときに、値が見つかった場合は、指定した値に関連付けられているキーが格納されます。それ以外の場合は <typeparamref name="TKey"/> 型に対する既定の値。 このパラメーターは初期化せずに渡されます。</param>
        /// <param name="comparer">値を比較する等値比較子。</param>
        /// <returns>指定した値を持つ要素が <paramref name="source"/> に格納されている場合は <see langword="true"/>。それ以外の場合は <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>、または <paramref name="comparer"/> が <see langword="null"/> です。</exception>
        public static bool TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, out TKey key, IEqualityComparer<TValue> comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            foreach (KeyValuePair<TKey, TValue> item in source)
            {
                if (comparer.Equals(item.Value, value))
                {
                    key = item.Key;
                    return true;
                }
            }

            key = default;
            return false;
        }

        /// <summary>
        /// 指定した値に関連付けられているキーを取得します。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> のキーの型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> の値の型。</typeparam>
        /// <param name="source">返されるキーが含まれる <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="key">このメソッドから制御が戻るときに、値が見つかった場合は、指定した値に関連付けられているキーが格納されます。それ以外の場合は <typeparamref name="TKey"/> 型に対する既定の値。このパラメーターは初期化せずに渡されます。</param>
        /// <returns>指定した値を持つ要素が <paramref name="source"/> に格納されている場合は <see langword="true"/>。それ以外の場合は <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <see langword="null"/> です。</exception>
        public static bool TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, out TKey key) => TryGetKey(source, value, out key, EqualityComparer<TValue>.Default);

        /// <summary>
        /// 指定した値に関連付けられているキーを取得します。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> のキーの型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> の値の型。</typeparam>
        /// <param name="source">返されるキーが含まれる <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="comparer">値を比較する等値比較子。</param>
        /// <returns>値が見つかった場合は、指定した値に関連付けられているキーが格納されます。それ以外の場合は <typeparamref name="TKey"/> 型に対する既定の値。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>、または <paramref name="comparer"/> が <see langword="null"/> です。</exception>
        public static TKey TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, IEqualityComparer<TValue> comparer)
            => TryGetKey(source, value, out TKey result, comparer) ? result : default;

        /// <summary>
        /// 指定した値に関連付けられているキーを取得します。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> のキーの型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> の値の型。</typeparam>
        /// <param name="source">返されるキーが含まれる <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="value">取得するキーの値。</param>
        /// <returns>値が見つかった場合は、指定した値に関連付けられているキーが格納されます。それ以外の場合は <typeparamref name="TKey"/> 型に対する既定の値。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <see langword="null"/> です。</exception>
        public static TKey TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value) => TryGetKey(source, value, EqualityComparer<TValue>.Default);

        /// <summary>
        /// <see cref="IDictionary{TKey, TValue}"/> に特定の値が格納されているかどうかを判断します。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> のキーの型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> の値の型。</typeparam>
        /// <param name="source">返されるキーが含まれる <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="comparer">値を比較する等値比較子。</param>
        /// <returns>指定した値を持つ要素が <paramref name="source"/> に格納されている場合は <see langword="true"/>。それ以外の場合は <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>、または <paramref name="comparer"/> が <see langword="null"/> です。</exception>
        public static bool ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, IEqualityComparer<TValue> comparer)
            => TryGetKey(source, value, out TKey _, comparer);

        /// <summary>
        /// <see cref="IDictionary{TKey, TValue}"/> に特定の値が格納されているかどうかを判断します。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> のキーの型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> の値の型。</typeparam>
        /// <param name="source">返されるキーが含まれる <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="comparer">値を比較する等値比較子。</param>
        /// <returns>指定した値を持つ要素が <paramref name="source"/> に格納されている場合は <see langword="true"/>。それ以外の場合は <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <see langword="null"/> です。</exception>
        public static bool ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value) => ContainsValue(source, value, EqualityComparer<TValue>.Default);
    }
}
