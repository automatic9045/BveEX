using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace AtsEx.Extensions.MapStatements
{
    /// <summary>
    /// AtsEX 由来のステートメント (マップ構文) を表します。
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// このステートメントのソースを取得します。
        /// </summary>
        public MapStatement Source { get; }

        internal Statement(MapStatement source)
        {
            Source = source;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Source.Clauses.Count == 0) return "<Empty>";

            string position = $"{{{Source.Clauses[0].LineIndex},{Source.Clauses[0].CharIndex};{Source.Location}m}}";
            return $"{position} {string.Join(".", Source.Clauses)};";
        }

        private bool FilterMatches(IEnumerable<ClauseFilter> filters)
        {
            int i = 1;
            foreach (ClauseFilter filter in filters)
            {
                if (Source.Clauses.Count <= i) return false;

                bool matches;
                try
                {
                    matches = filter.Matches(Source.Clauses[i]);
                }
                catch (SyntaxException)
                {
                    throw new SyntaxException(this);
                }

                if (!matches) return false;

                i++;
            }

            return true;
        }

        /// <summary>
        /// 指定された条件に一致する AtsEX 公式のマップステートメントであるかどうかを判定します。
        /// </summary>
        /// <param name="filters">ステートメントの句の一覧 (前方一致)。
        /// 「AtsEx」マップ要素 (<c>AtsEx.Hoge['a'].Fuga('b');</c> の AtsEx) は除きます。</param>
        /// <returns>条件に一致した場合は <see langword="true"/>、一致しなかった場合は <see langword="false"/>。</returns>
        public bool IsOfficialStatement(params ClauseFilter[] filters)
        {
            if (0 < filters.Length && filters[0].Name.ToLowerInvariant() == "user")
            {
                throw new ArgumentException();
            }

            return FilterMatches(filters);
        }

        /// <summary>
        /// 指定された条件に一致する AtsEX ユーザーマップステートメントであるかどうかを判定します。
        /// </summary>
        /// <param name="userName">ステートメントのユーザー名。<c>AtsEx.User.FooBar.Hoge['a'].Fuga('b');</c> の FooBar が該当します。</param>
        /// <param name="filters">ステートメントの句の一覧 (前方一致)。
        /// ユーザー名以前のマップ要素 (<c>AtsEx.User.FooBar.Hoge['a'].Fuga('b');</c> の AtsEx、User、FooBar) は除きます。</param>
        /// <returns>条件に一致した場合は <see langword="true"/>、一致しなかった場合は <see langword="false"/>。</returns>
        public bool IsUserStatement(string userName, params ClauseFilter[] filters)
        {
            ClauseFilter[] userClauses = new ClauseFilter[]
            {
                new ClauseFilter("User", ClauseType.Element), new ClauseFilter(userName, ClauseType.Element),
            };

            return FilterMatches(userClauses.Concat(filters));
        }
    }
}
