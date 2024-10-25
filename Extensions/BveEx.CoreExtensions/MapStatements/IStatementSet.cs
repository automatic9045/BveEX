using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.MapStatements
{
    /// <summary>
    /// BveEX マップステートメントのセットを表します。
    /// </summary>
    public interface IStatementSet : IExtension, IEnumerable<Statement>
    {
        /// <summary>
        /// BveEX マップステートメントが読み込まれた瞬間に発生します。
        /// </summary>
        event EventHandler<StatementLoadedEventArgs> StatementLoaded;

        /// <summary>
        /// 指定された条件に一致する BveEX 公式のマップステートメントを検索し、その冒頭 1 つ目を取得します。
        /// </summary>
        /// <param name="clauses">検索するステートメントの句の一覧 (前方一致)。
        /// 「BveEx」マップ要素 (<c>BveEx.Hoge['a'].Fuga('b');</c> の BveEx) は除きます。</param>
        /// <returns>条件に一致するステートメントの冒頭 1 つ目。見つからなかった場合は <see langword="null"/> を返します。</returns>
        Statement FindOfficialStatement(params ClauseFilter[] clauses);

        /// <summary>
        /// 指定された条件に一致する BveEX 公式のマップステートメントを検索し、その一覧を取得します。
        /// </summary>
        /// <param name="clauses">検索するステートメントの句の一覧 (前方一致)。
        /// 「BveEx」マップ要素 (<c>BveEx.Hoge['a'].Fuga('b');</c> の BveEx) は除きます。</param>
        /// <returns>条件に一致するステートメントの一覧。</returns>
        IEnumerable<Statement> FindOfficialStatements(params ClauseFilter[] clauses);

        /// <summary>
        /// 指定された条件に一致する BveEX ユーザーマップステートメントを検索し、その冒頭 1 つ目を取得します。
        /// </summary>
        /// <param name="userName">ステートメントのユーザー名。<c>BveEx.User.FooBar.Hoge['a'].Fuga('b');</c> の FooBar が該当します。</param>
        /// <param name="clauses">検索するステートメントの句の一覧 (前方一致)。
        /// ユーザー名以前のマップ要素 (<c>BveEx.User.FooBar.Hoge['a'].Fuga('b');</c> の BveEx、User、FooBar) は除きます。</param>
        /// <returns>条件に一致するステートメントの冒頭 1 つ目。見つからなかった場合は <see langword="null"/> を返します。</returns>
        Statement FindUserStatement(string userName, params ClauseFilter[] clauses);

        /// <summary>
        /// 指定された条件に一致する BveEX ユーザーマップステートメントを検索し、その一覧を取得します。
        /// </summary>
        /// <param name="userName">ステートメントのユーザー名。<c>BveEx.User.FooBar.Hoge['a'].Fuga('b');</c> の FooBar が該当します。</param>
        /// <param name="clauses">検索するステートメントの句の一覧 (前方一致)。
        /// ユーザー名以前のマップ要素 (<c>BveEx.User.FooBar.Hoge['a'].Fuga('b');</c> の BveEx、User、FooBar) は除きます。</param>
        /// <returns>条件に一致するステートメントの一覧。</returns>
        IEnumerable<Statement> FindUserStatements(string userName, params ClauseFilter[] clauses);
    }
}
