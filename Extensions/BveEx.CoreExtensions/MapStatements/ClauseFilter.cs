using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.Extensions.MapStatements
{
    /// <summary>
    /// マップステートメントの句を検索する際の絞り込み条件を表します。
    /// </summary>
    public struct ClauseFilter
    {
        private readonly static int[] ZeroArray = new int[] { 0 };

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
        /// 句のキーの長さの条件を取得します。
        /// </summary>
        /// <remarks>
        /// キーの長さがこの配列のいずれかの要素に一致することを要求します。
        /// <see cref="Name"/>、<see cref="Type"/> には合致するが、この条件に合致しない句に対して <see cref="Matches(MapStatementClause)"/> メソッドを呼び出すと、<see cref="SyntaxException"/> をスローします。<br/>
        /// 長さを限定しない場合は <see langword="null"/> を指定します。
        /// </remarks>
        public int[] KeyCount { get; }

        /// <summary>
        /// 句の引数の長さの条件を取得します。
        /// </summary>
        /// <remarks>
        /// 引数の長さがこの配列のいずれかの要素に一致することを要求します。
        /// <see cref="Name"/>、<see cref="Type"/> には合致するが、この条件に合致しない句に対して <see cref="Matches(MapStatementClause)"/> メソッドを呼び出すと、<see cref="SyntaxException"/> をスローします。<br/>
        /// <see cref="Type"/> が <see cref="ClauseType.Function"/> である場合は、例外的に引数が <see langword="null"/> 1 個のみである関数に対しても要素「0」が一致します。<br/>
        /// 長さを限定しない場合は <see langword="null"/> を指定します。
        /// </remarks>
        public int[] ArgCount { get; }

        /// <summary>
        /// マップ要素を表す <see cref="ClauseFilter"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">検索する句の名前。</param>
        /// <param name="keyCount">
        /// マップ要素のキーの長さの条件。キーの長さがこの配列のいずれかの要素に一致することを要求します。
        /// <paramref name="name"/> には合致するが、この条件に合致しない句に対して <see cref="Matches(MapStatementClause)"/> メソッドを呼び出すと、<see cref="SyntaxException"/> をスローします。
        /// 長さを限定しない場合は <see langword="null"/> を指定してください。
        /// </param>
        /// <returns>マップ要素を表す <see cref="ClauseFilter"/>。</returns>
        public static ClauseFilter Element(string name, params int[] keyCount) => new ClauseFilter(name, ClauseType.Element, keyCount, ZeroArray);

        /// <summary>
        /// 関数を表す <see cref="ClauseFilter"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">検索する句の名前。</param>
        /// <param name="argCount">
        /// 関数の引数の長さの条件。引数の長さがこの配列のいずれかの要素に一致することを要求します。
        /// <paramref name="name"/> には合致するが、この条件に合致しない句に対して <see cref="Matches(MapStatementClause)"/> メソッドを呼び出すと、<see cref="SyntaxException"/> をスローします。
        /// なお、引数が <see langword="null"/> 1 個のみである関数に対しては、要素「0」も一致します。
        /// 長さを限定しない場合は <see langword="null"/> を指定してください。
        /// </param>
        /// <returns>マップ要素を表す <see cref="ClauseFilter"/>。</returns>
        public static ClauseFilter Function(string name, params int[] argCount) => new ClauseFilter(name, ClauseType.Function, ZeroArray, argCount);

        /// <summary>
        /// 検索する句の種類、キー・引数の長さを指定して <see cref="ClauseFilter"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">検索する句の名前。</param>
        /// <param name="type">検索する句の種類。種類を限定しない場合は <see langword="null"/> を指定してください。</param>
        /// <param name="keyCount">
        /// 句のキーの長さの条件。
        /// <paramref name="name"/>、<paramref name="type"/> には合致するが、この条件に合致しない句に対して <see cref="Matches(MapStatementClause)"/> メソッドを呼び出すと、<see cref="SyntaxException"/> をスローします。
        /// 長さを限定しない場合は <see langword="null"/> を指定してください。
        /// </param>
        /// <param name="argCount">
        /// 句の引数の長さの条件。
        /// <paramref name="name"/>、<paramref name="type"/> には合致するが、この条件に合致しない句に対して <see cref="Matches(MapStatementClause)"/> メソッドを呼び出すと、<see cref="SyntaxException"/> をスローします。
        /// 長さを限定しない場合は <see langword="null"/> を指定してください。
        /// </param>
        /// <remarks>
        /// 通常は <see cref="Element(string, int[])"/> または <see cref="Function(string, int[])"/> メソッドを使用してください。
        /// </remarks>
        public ClauseFilter(string name, ClauseType? type, int[] keyCount, int[] argCount)
        {
            Name = name.ToLowerInvariant();
            Type = type;
            KeyCount = keyCount;
            ArgCount = argCount;
        }

        /// <summary>
        /// 検索する句の種類を指定して <see cref="ClauseFilter"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">検索する句の名前。</param>
        /// <param name="type">検索する句の種類。種類を限定しない場合は <see langword="null"/> を指定してください。</param>
        public ClauseFilter(string name, ClauseType? type) : this(name, type, null, null)
        {
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
        /// <exception cref="SyntaxException"><see cref="KeyCount"/>、<see cref="ArgCount"/> のいずれかの条件に合致しません。</exception>
        /// <exception cref="NotSupportedException"><see cref="Type"/> がサポートされない値です。</exception>
        public bool Matches(MapStatementClause clause)
        {
            if (clause.Name.ToLowerInvariant() != Name) return false;

            switch (Type)
            {
                case null:
                    ValidateCount(clause);
                    return true;

                case ClauseType.Element:
                    if (clause.Args.Count != 0) return false;

                    ValidateCount(clause);
                    return true;

                case ClauseType.Function:
                    if (clause.Args.Count == 0) return false;

                    ValidateCount(clause, true);
                    return true;

                default:
                    throw new NotSupportedException();
            }
        }

        private void ValidateCount(MapStatementClause clause, bool isFunction = false)
        {
            if (!(KeyCount is null) && !KeyCount.Contains(clause.Keys.Count))
            {
                throw new SyntaxException();
            }

            if (!(ArgCount is null) && !ArgCount.Contains(clause.Args.Count))
            {
                if (!(isFunction && ArgCount.Contains(0) && clause.Args.Count == 1 && clause.Args[0] is null))
                {
                    throw new SyntaxException();
                }
            }
        }
    }
}
