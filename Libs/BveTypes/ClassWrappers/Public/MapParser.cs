using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using Irony.Parsing;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// バージョン 2.0 以降のマップの構文解析を行います。
    /// </summary>
    public class MapParser : MapParserBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapParser>();

            IncludeMethod = members.GetSourceMethodOf(nameof(Include));
            SetLocationMethod = members.GetSourceMethodOf(nameof(SetLocation));
            GetValueMethod = members.GetSourceMethodOf(nameof(GetValue));
            SetUserVariableMethod = members.GetSourceMethodOf(nameof(SetUserVariable));
            GetUserVariableMethod = members.GetSourceMethodOf(nameof(GetUserVariable));
            GetSystemVariableMethod = members.GetSourceMethodOf(nameof(GetSystemVariable));
            InvokeFunctionMethod = members.GetSourceMethodOf(nameof(InvokeFunction));
            CalculateSignMethod = members.GetSourceMethodOf(nameof(CalculateSign));
            CalculateOperatorMethod = members.GetSourceMethodOf(nameof(CalculateOperator));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapParser"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapParser(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapParser"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapParser FromSource(object src) => src is null ? null : new MapParser(src);

        private static FastMethod IncludeMethod;
        /// <summary>
        /// 他のマップファイルをインクルードします。
        /// </summary>
        /// <param name="node">インクルード文 (例: <c>include 'SubMap.txt';</c>) を表す構文木。</param>
        public void Include(ParseTreeNode node) => IncludeMethod.Invoke(Src, new object[] { node });

        private static FastMethod SetLocationMethod;
        /// <summary>
        /// 距離程 [m] を設定します。
        /// </summary>
        /// <param name="node">距離程を設定する構文 (例: <c>1200;</c>) を表す構文木。</param>
        public void SetLocation(ParseTreeNode node) => SetLocationMethod.Invoke(Src, new object[] { node });

        private static FastMethod GetValueMethod;
        /// <summary>
        /// 与えられた構文を解釈し、その実行結果を返します。
        /// </summary>
        /// <param name="node">構文木。</param>
        /// <returns>構文の実行結果。</returns>
        public object GetValue(ParseTreeNode node) => GetValueMethod.Invoke(Src, new object[] { node });

        private static FastMethod SetUserVariableMethod;
        /// <summary>
        /// 変数に値を代入します。
        /// </summary>
        /// <param name="node">変数への代入 (例: <c>$foo = 123;</c>) を表す構文木。</param>
        public void SetUserVariable(ParseTreeNode node) => SetUserVariableMethod.Invoke(Src, new object[] { node });

        private static FastMethod GetUserVariableMethod;
        /// <summary>
        /// 変数の値を取得します。
        /// </summary>
        /// <param name="node">変数の取得 (例: <c>Curve.SetGauge($foo);</c> の <c>$foo</c>) を表す構文木。</param>
        public object GetUserVariable(ParseTreeNode node) => GetUserVariableMethod.Invoke(Src, new object[] { node });

        private static FastMethod GetSystemVariableMethod;
        /// <summary>
        /// システム変数 (<c>distance</c> など) の値を取得します。
        /// </summary>
        /// <param name="clauses">構文の句の一覧。</param>
        /// <param name="node">システム変数の取得 (例: <c>Beacon.Put(1, 0, distance);</c> の <c>distance</c>) を表す構文木。</param>
        public object GetSystemVariable(WrappedList<MapStatementClause> clauses, ParseTreeNode node) => GetSystemVariableMethod.Invoke(Src, new object[] { clauses.Src, node });

        private static FastMethod InvokeFunctionMethod;
        /// <summary>
        /// 関数を実行します。
        /// </summary>
        /// <param name="clauses">構文の句の一覧。</param>
        /// <param name="node">関数 (例: <c>abs(1.23);</c>) を表す構文木。</param>
        public object InvokeFunction(WrappedList<MapStatementClause> clauses, ParseTreeNode node) => InvokeFunctionMethod.Invoke(Src, new object[] { clauses.Src, node });

        private static FastMethod CalculateSignMethod;
        /// <summary>
        /// 符号付きの値を解釈します。
        /// </summary>
        /// <param name="node">符号付きの値 (例: <c>-$foo</c>) を表す構文木。</param>
        public object CalculateSign(ParseTreeNode node) => CalculateSignMethod.Invoke(Src, new object[] { node });

        private static FastMethod CalculateOperatorMethod;
        /// <summary>
        /// 数式を計算します。
        /// </summary>
        /// <param name="node">数式 (例: <c>(12 + 34 * 56 - 78) / 90</c>) を表す構文木。</param>
        public object CalculateOperator(ParseTreeNode node) => CalculateOperatorMethod.Invoke(Src, new object[] { node });
    }
}
