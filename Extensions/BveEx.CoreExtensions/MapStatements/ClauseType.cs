using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.MapStatements
{
    /// <summary>
    /// マップステートメントの句の種類を指定します。
    /// </summary>
    public enum ClauseType
    {
        /// <summary>
        /// マップ要素。<c>Hoge['a'].Fuga.Piyo('b');</c> の Hoge、Fuga が該当します。
        /// </summary>
        Element,

        /// <summary>
        /// 関数。<c>Hoge['a'].Fuga.Piyo('b');</c> の Piyo が該当します。
        /// </summary>
        Function,
    }
}
