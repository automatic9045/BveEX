using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.MapStatements
{
    /// <summary>
    /// <see cref="IStatementSet.StatementLoaded"/> イベントのデータを提供します。
    /// </summary>
    public class StatementLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// 読み込まれた AtsEX マップステートメント を取得します。
        /// </summary>
        public Statement Statement { get; }

        /// <summary>
        /// <see cref="StatementLoadedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="statement">読み込まれた AtsEX マップステートメント。</param>
        public StatementLoadedEventArgs(Statement statement)
        {
            Statement = statement;
        }
    }
}
