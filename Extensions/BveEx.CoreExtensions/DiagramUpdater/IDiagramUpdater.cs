using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.DiagramUpdater
{
    /// <summary>
    /// 時刻表、ダイヤグラムなどの行路に関わるオブジェクトの更新機能を提供します。
    /// </summary>
    public interface IDiagramUpdater : IExtension
    {
        /// <summary>
        /// 更新に使用する <see cref="Scenario"/> を明示的に指定して、時刻表と「時刻と位置」フォーム内のダイヤグラムの表示を最新の設定に更新します。
        /// </summary>
        /// <remarks>
        /// <see cref="IBveHacker.ScenarioCreated"/> イベント内で呼び出す場合など、<see cref="IBveHacker.Scenario"/> が利用できない場合にこのオーバーロードを使用してください。
        /// 通常は <see cref="UpdateDiagram()"/> オーバーロードを使用してください。
        /// </remarks>
        /// <seealso cref="UpdateDiagram()"/>
        /// <param name="scenario">更新に使用する <see cref="Scenario"/>。</param>
        void UpdateDiagram(Scenario scenario);

        /// <summary>
        /// <see cref="IBveHacker.Scenario"/> を使用して、時刻表と「時刻と位置」フォーム内のダイヤグラムの表示を最新の設定に更新します。
        /// </summary>
        /// <remarks>
        /// <see cref="IBveHacker.ScenarioCreated"/> イベント内で呼び出す場合など、
        /// <see cref="IBveHacker.Scenario"/> が利用できない場合は <see cref="UpdateDiagram(Scenario)"/> オーバーロードを使用してください。
        /// </remarks>
        /// <seealso cref="UpdateDiagram(Scenario)"/>
        void UpdateDiagram();
    }
}
