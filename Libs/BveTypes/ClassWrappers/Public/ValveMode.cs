using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 弁の給排気のモードを指定します。
    /// </summary>
    public enum ValveMode
    {
        /// <summary>
        /// 急ゆるめ。
        /// </summary>
        RapidRelease = -3,

        /// <summary>
        /// 全ゆるめ。
        /// </summary>
        Release = -2,

        /// <summary>
        /// ゆるめ。
        /// </summary>
        PartialRelease = -1,

        /// <summary>
        /// 重なり。
        /// </summary>
        Lap = 0,

        /// <summary>
        /// 作用。
        /// </summary>
        Apply = 1,
    }
}
