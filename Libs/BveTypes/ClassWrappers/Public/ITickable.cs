using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// フレーム毎に実行可能であることを表します。
    /// </summary>
    public interface ITickable
    {
        /// <summary>
        /// 初期化します。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <param name="elapsedSeconds">前フレームからの経過時間 [s]。</param>
        void Tick(double elapsedSeconds);
    }
}
