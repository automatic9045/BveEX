using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 互換性のために残されている旧名のクラスです。<see cref="BrakeBlenderBase"/> を使用してください。
    /// </summary>
    public abstract class ElectroPneumaticBlendedBrakingControlBase : BrakeBlenderBase
    {
        /// <summary>
        /// オリジナル オブジェクトから <see cref="ElectroPneumaticBlendedBrakingControlBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ElectroPneumaticBlendedBrakingControlBase(object src) : base(src)
        {
        }
    }
}
