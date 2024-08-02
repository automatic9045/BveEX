using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ブレーキハンドルの位置を指定します。
    /// </summary>
    public enum BrakePosition
    {
        /// <summary>
        /// 緩解位置。
        /// </summary>
        Release = -2,

        /// <summary>
        /// 常用ブレーキ位置 (B67)。
        /// </summary>
        Service = -1,

        /// <summary>
        /// 非常ブレーキ位置。
        /// </summary>
        Emergency = 0,
    }
}
