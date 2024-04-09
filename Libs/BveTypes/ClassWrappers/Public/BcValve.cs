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
    /// ブレーキシリンダ電磁弁を表します。
    /// </summary>
    public class BcValve : Valve
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BcValve>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BcValve"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BcValve(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BcValve"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new BcValve FromSource(object src) => src is null ? null : new BcValve(src);
    }
}
