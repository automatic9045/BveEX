using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.MapStatements
{
    /// <summary>
    /// include ステートメントを使用して独自に定義する構文を表します。
    /// </summary>
    /// <remarks>
    /// 互換性のために残されている古い機能です。AtsEx.CoreExtensions のマップステートメント機能をご利用ください。<br/>
    /// <see cref="IStatement"/> と異なりシナリオの読込完了以前から取得できますが、マップ変数を埋め込んだり、定義した距離程と関連付けたりすることはできません。<br/>
    /// 何らかの目的でファイルを読み込む際のパス指定など、宣言的な用途にお使いください。
    /// </remarks>
    [Obsolete]
    public interface IHeader
    {
        /// <summary>
        /// このヘッダーの名前を表す識別子を取得します。
        /// </summary>
        /// <remarks>
        /// ヘッダーの宣言冒頭の、括弧に囲まれた部分のテキストが該当します (例: <c>include '&lt;AtsEx::SampleHeader&gt;Hello, AtsEX world!';</c> なら <c>AtsEx::SampleHeader</c>)。
        /// </remarks>
        Identifier Name { get; }

        /// <summary>
        /// このヘッダーの引数のテキストを取得します。
        /// </summary>
        /// <remarks>
        /// <see cref="Name"/> に続く部分のテキストが該当します (例: <c>include '&lt;AtsEx::SampleHeader&gt;Hello, AtsEX world!';</c> なら <c>Hello, AtsEX world!</c>)。
        /// </remarks>
        string Argument { get; }
    }
}
