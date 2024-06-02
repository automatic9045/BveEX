using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace AtsEx.Extensions.MapStatements
{
    /// <summary>
    /// マップステートメントの句を検索する際の絞り込み条件を表します。
    /// </summary>
    public struct ClauseFilter
    {
        /// <summary>
        /// 検索する句の名前を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 検索する句の種類を取得します。
        /// </summary>
        /// <remarks>
        /// 種類を限定しない場合は <see langword="null"/> を指定します。
        /// </remarks>
        public ClauseType? Type { get; }

        /// <summary>
        /// 検索する句の種類を指定して <see cref="ClauseFilter"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">検索する句の名前。</param>
        /// <param name="type">検索する句の種類。種類を限定しない場合は <see langword="null"/> を指定してください。</param>
        public ClauseFilter(string name, ClauseType? type)
        {
            Name = name.ToLowerInvariant();
            Type = type;
        }

        /// <summary>
        /// 検索する句の種類を限定しない <see cref="ClauseFilter"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">検索する句の名前。</param>
        public ClauseFilter(string name) : this(name, null)
        {
        }

        /// <summary>
        /// 指定した句がこの条件を満たすかどうかを判定します。
        /// </summary>
        /// <param name="clause">この条件を満たすかどうかを判定する句。</param>
        /// <returns><paramref name="clause"/> がこの条件を満たしている場合は <see langword="true"/>、満たしていない場合は <see langword="false"/>。</returns>
        /// <exception cref="NotSupportedException"><see cref="Type"/> がサポートされない値です。</exception>
        public bool Matches(MapStatementClause clause)
        {
            if (clause.Name.ToLowerInvariant() != Name) return false;

            switch (Type)
            {
                case null:
                    return true;

                case ClauseType.Element:
                    return clause.Args.Count == 0;

                case ClauseType.Function:
                    return 0 < clause.Args.Count;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
