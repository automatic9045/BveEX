using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// <see cref="IBveHacker.PreviewScenarioCreated"/>、<see cref="IBveHacker.ScenarioCreated"/> イベントのデータを提供します。
    /// </summary>
    public class ScenarioCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// 生成された <see cref="BveTypes.ClassWrappers.Scenario"/> クラスのインスタンスを取得します。
        /// </summary>
        public Scenario Scenario { get; }

        /// <summary>
        /// <see cref="ScenarioCreatedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="scenario">生成された <see cref="BveTypes.ClassWrappers.Scenario"/> クラスのインスタンス。</param>
        public ScenarioCreatedEventArgs(Scenario scenario) : base()
        {
            Scenario = scenario;
        }
    }
}
