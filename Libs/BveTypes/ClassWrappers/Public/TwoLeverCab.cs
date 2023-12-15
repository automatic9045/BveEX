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
    /// ツーハンドル式の運転台を表します。
    /// </summary>
    public class TwoLeverCab : CabBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TwoLeverCab>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TwoLeverCab"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TwoLeverCab(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TwoLeverCab"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new TwoLeverCab FromSource(object src) => src is null ? null : new TwoLeverCab(src);
    }
}
