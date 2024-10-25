using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

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
        /// ステートメントの読込に使用された <see cref="BveTypes.ClassWrappers.MapLoader"/> を取得します。
        /// </summary>
        public MapLoader MapLoader { get; }

        /// <summary>
        /// <see cref="StatementLoadedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="statement">読み込まれた AtsEX マップステートメント。</param>
        /// <param name="mapLoader">ステートメントの読込に使用された <see cref="BveTypes.ClassWrappers.MapLoader"/>。</param>
        public StatementLoadedEventArgs(Statement statement, MapLoader mapLoader)
        {
            Statement = statement;
            MapLoader = mapLoader;
        }
    }
}
