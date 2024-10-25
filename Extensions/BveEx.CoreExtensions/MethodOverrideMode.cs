using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions
{
    /// <summary>
    /// メソッドの処理を上書きする方法を指定します。
    /// </summary>
    public enum MethodOverrideMode
    {
        /// <summary>
        /// 上書き定義した処理のみを実行し、オリジナルの処理をスキップすることを指定します。
        /// </summary>
        SkipOriginal = 0,

        /// <summary>
        /// 上書き定義した処理に続けて、オリジナルの処理も実行することを指定します。
        /// </summary>
        RunOriginal = 1,
    }
}
