using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 締切電磁弁式電空協調制御を表します。
    /// </summary>
    public class LockoutValve : BrakeBlenderBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<LockoutValve>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="LockoutValve"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected LockoutValve(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="LockoutValve"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static LockoutValve FromSource(object src) => src is null ? null : new LockoutValve(src);
    }
}
