﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins.Extensions;

using BveEx.Extensions.MapStatements.Parsing;

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
        /// マップの読込が完了したときに発生します。
        /// </summary>
        event EventHandler LoadingCompleted;

        /// <summary>
        /// 独自の文法をサポートする構文解析器を登録します。
        /// </summary>
        /// <remarks>
        /// 独自の文法を追加すると、将来のアップデートで BVE 本体や BveEX 公式の文法と衝突する危険性があります。
        /// この機能は可能な限り使わず、単純なマップステートメントを使用されることを強く推奨します。
        /// </remarks>
        /// <param name="parser">登録する構文解析器。</param>
        void RegisterParser(RawParserBase parser);

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
