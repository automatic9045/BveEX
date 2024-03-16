using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 互換性のために残されている旧名のクラスです。<see cref="CarBrake"/> を使用してください。
    /// </summary>
    [Obsolete]
    public class CarBc : CarBrake
    {
        /// <summary>
        /// オリジナル オブジェクトから <see cref="CarBc"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CarBc(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CarBc"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new CarBc FromSource(object src) => src is null ? null : new CarBc(src);
    }
}
