using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.ConductorPatch
{
    /// <summary>
    /// 車掌動作の上書き宣言の優先度を指定します。
    /// </summary>
    public enum DeclarationPriority
    {
        /// <summary>
        /// 既定値として宣言することを指定します。他に宣言が存在する場合は無条件に上書きされます。
        /// </summary>
        AsDefaultValue = 0,

        /// <summary>
        /// 通常の優先度で宣言することを指定します。既になされた優先度 <see cref="Sequentially"/> の宣言を上書きしますが、後に他の優先度 <see cref="Sequentially"/> の宣言がなされた場合は上書きされます。
        /// </summary>
        Sequentially,

        /// <summary>
        /// 最上級の優先度で宣言することを指定します。全ての他の宣言を無条件に上書きします。
        /// </summary>
        TopMost,
    }
}
