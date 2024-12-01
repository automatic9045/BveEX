using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

namespace BveEx.Extensions
{
    /// <summary>
    /// 行列の変換機能を提供します。
    /// </summary>
    public interface IMatrixConverter
    {
        /// <summary>
        /// 行列を変換します。
        /// </summary>
        /// <param name="source">変換元の行列。</param>
        /// <returns>変換結果の行列。</returns>
        Matrix Convert(Matrix source);
    }
}
