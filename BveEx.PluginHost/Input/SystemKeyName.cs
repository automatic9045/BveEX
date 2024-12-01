using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Input
{
    /// <summary>
    /// BVE のシステムキーの種類を指定します。
    /// </summary>
    public enum SystemKeyName
    {
        /// <summary>
        /// 視点を上へ移動します。
        /// </summary>
        ViewUp,

        /// <summary>
        /// 視点を下へ移動します。
        /// </summary>
        ViewDown,

        /// <summary>
        /// 視点を左へ移動します。
        /// </summary>
        ViewLeft,

        /// <summary>
        /// 視点を右へ移動します。
        /// </summary>
        ViewRight,

        /// <summary>
        /// 視点をデフォルトに戻します。
        /// </summary>
        ViewDefault,

        /// <summary>
        /// 視点をズームインします。
        /// </summary>
        ViewZoomIn,

        /// <summary>
        /// 視点をズームアウトします。
        /// </summary>
        ViewZoomOut,

        /// <summary>
        /// 視点を切り替えます。
        /// </summary>
        ViewChange,

        /// <summary>
        /// 時刻表の表示 / 非表示を切り替えます。
        /// </summary>
        TimeTable,

        /// <summary>
        /// シナリオを再読み込みします。
        /// </summary>
        ReloadScenario,

        /// <summary>
        /// 自列車をその場で停止させます。F7 キーに相当します。
        /// </summary>
        Stop,

        /// <summary>
        /// 早送りの有効 / 無効を切り替えます。F8 キーに相当します。
        /// </summary>
        FastForward,

        /// <summary>
        /// ポーズの有効 / 無効を切り替えます。
        /// </summary>
        Pause,
    }
}
