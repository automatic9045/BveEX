using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 定速制御の状態を指定します。
    /// </summary>
    public enum ConstantSpeedMode
    {
        /// <summary>
        /// 現在の状態を維持します。
        /// </summary>
        Continue = 0,
        /// <summary>
        /// 定速制御を起動します。
        /// </summary>
        Enable = 1,
        /// <summary>
        /// 定速制御を停止します。
        /// </summary>
        Disable = 2,
    }
}
