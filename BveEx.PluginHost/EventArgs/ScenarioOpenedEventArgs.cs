using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.PluginHost
{
    /// <summary>
    /// <see cref="IBveHacker.ScenarioOpened"/> イベントのデータを提供します。
    /// </summary>
    public class ScenarioOpenedEventArgs : EventArgs
    {
        /// <summary>
        /// 読込が開始されたシナリオの情報を格納する <see cref="BveTypes.ClassWrappers.ScenarioInfo"/> を取得します。
        /// </summary>
        public ScenarioInfo ScenarioInfo { get; }

        /// <summary>
        /// F5 キーなどによる再読込であるかどうかを取得します。
        /// </summary>
        public bool IsReload { get; }

        /// <summary>
        /// <see cref="ScenarioOpenedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="scenarioInfo">読込が開始されたシナリオの情報を格納する <see cref="BveTypes.ClassWrappers.ScenarioInfo"/>。</param>
        /// <param name="isReload">F5 キーなどによる再読込であるかどうか。</param>
        public ScenarioOpenedEventArgs(ScenarioInfo scenarioInfo, bool isReload)
        {
            ScenarioInfo = scenarioInfo;
            IsReload = isReload;
        }

        /// <summary>
        /// 再読込でないことを表す <see cref="ScenarioOpenedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="scenarioInfo">読込が開始されたシナリオの情報を格納する <see cref="BveTypes.ClassWrappers.ScenarioInfo"/>。</param>
        public ScenarioOpenedEventArgs(ScenarioInfo scenarioInfo) : this(scenarioInfo, false)
        {
        }
    }
}
