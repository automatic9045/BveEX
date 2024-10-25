using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.ConductorPatch
{
    /// <summary>
    /// 車掌動作を上書きするパッチを表します。
    /// </summary>
    public class ConductorPatch
    {
        /// <summary>
        /// 新しい車掌動作を定義した <see cref="ConductorBase"/> を取得します。
        /// </summary>
        public ConductorBase Conductor { get; }

        /// <summary>
        /// 車掌動作の上書き宣言の優先度を取得します。
        /// </summary>
        public DeclarationPriority Priority { get; }


        internal ConductorPatch(ConductorBase conductor, DeclarationPriority priority)
        {
            Conductor = conductor;
            Priority = priority;
        }
    }
}
