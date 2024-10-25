using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.HornBlown"/> イベントのデータを表します。
    /// </summary>
    public class HornBlownEventArgs : EventArgs
    {
        /// <summary>
        /// 警笛の種類を取得します。
        /// </summary>
        public HornType HornType { get; }

        /// <summary>
        /// <see cref="HornBlownEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="hornType">警笛の種類。</param>
        public HornBlownEventArgs(HornType hornType)
        {
            HornType = hornType;
        }
    }
}
