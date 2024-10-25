using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.PreTrainPatch
{
    /// <summary>
    /// 先行列車の走行位置の変換機能を提供します。
    /// </summary>
    public interface IPreTrainLocationConverter
    {
        /// <summary>
        /// 先行列車の走行位置を変換します。
        /// </summary>
        /// <param name="source">変換元の <see cref="PreTrainLocation"/>。</param>
        /// <returns>変換結果の <see cref="PreTrainLocation"/>。</returns>
        PreTrainLocation Convert(PreTrainLocation source);
    }
}
