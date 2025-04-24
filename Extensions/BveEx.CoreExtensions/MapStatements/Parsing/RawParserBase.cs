using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Irony.Parsing;

namespace BveEx.Extensions.MapStatements.Parsing
{
    /// <summary>
    /// 追加文法を独自に定義し、それに則ってマップ構文を解析します。
    /// </summary>
    public abstract class RawParserBase
    {
        /// <summary>
        /// 文法を定義します。
        /// </summary>
        /// <param name="grammar">マップ構文の文法。</param>
        /// <param name="terminals">既定の文法にて定義されている代表的な <see cref="BnfTerm"/> のセット。</param>
        public virtual void ConstructGrammar(Grammar grammar, TerminalSet terminals)
        {
        }

        /// <summary>
        /// 式の解析を試行します。
        /// </summary>
        /// <param name="parser">構文解析器。</param>
        /// <param name="node">解析対象となる式。</param>
        /// <returns>
        /// 式の解析が完了したかどうか。<see langword="false"/> を返すと、他の構文解析器へ処理がフォールバックされます。
        /// どの構文解析器でも解析ができなかった場合は、BVE の既定の解析処理を使用します。
        /// </returns>
        public virtual bool TryParseNode(MapParser parser, ParseTreeNode node)
        {
            return false;
        }

        /// <summary>
        /// システム変数の取得を試行します。
        /// </summary>
        /// <param name="parser">構文解析器。</param>
        /// <param name="key">変数の名前。</param>
        /// <param name="result">変数の値。</param>
        /// <returns>
        /// システム変数の取得が完了したかどうか。<see langword="false"/> を返すと、他の構文解析器へ処理がフォールバックされます。
        /// どの構文解析器でも解析ができなかった場合は、BVE の既定の解析処理を使用します。
        /// </returns>
        public virtual bool TryGetSystemVariable(MapParser parser, string key, out object result)
        {
            result = default;
            return false;
        }

        /// <summary>
        /// 符号演算を試行します。
        /// </summary>
        /// <param name="value">演算前の値。</param>
        /// <param name="operatorName">符号の名前。</param>
        /// <param name="result">演算後の値。</param>
        /// <returns>
        /// 符号演算が完了したかどうか。<see langword="false"/> を返すと、他の構文解析器へ処理がフォールバックされます。
        /// どの構文解析器でも解析ができなかった場合は、BVE の既定の解析処理を使用します。
        /// </returns>
        public virtual bool TryCalculateSign(object value, string operatorName, out object result)
        {
            result = default;
            return false;
        }

        /// <summary>
        /// 二項演算を試行します。
        /// </summary>
        /// <param name="left">左辺の値。</param>
        /// <param name="right">右辺の値。</param>
        /// <param name="operatorName">演算子の名前。</param>
        /// <param name="result">演算後の値。</param>
        /// <returns>
        /// 二項演算が完了したかどうか。<see langword="false"/> を返すと、他の構文解析器へ処理がフォールバックされます。
        /// どの構文解析器でも解析ができなかった場合は、BVE の既定の解析処理を使用します。
        /// </returns>
        public virtual bool TryCalculateOperator(object left, object right, string operatorName, out object result)
        {
            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="string"/> 型、<see cref="double"/> 型、<see cref="int"/> 型のオブジェクトを真偽値に変換します。
        /// </summary>
        /// <param name="value">変換元のオブジェクト。</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><paramref name="value"/> が <see cref="string"/> 型、<see cref="double"/> 型、<see cref="int"/> 型以外です。</exception>
        protected bool ToBool(object value)
        {
            switch (value)
            {
                case string stringValue:
                    return !(stringValue is null);

                case double doubleValue:
                    return doubleValue != 0;

                case int intValue:
                    return intValue != 0;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
