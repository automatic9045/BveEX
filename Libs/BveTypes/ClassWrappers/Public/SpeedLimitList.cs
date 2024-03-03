using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 速度制限のリストを表します。
    /// </summary>
    /// <remarks>
    /// 通常、要素は <see cref="ValueNode{T}"/> 型 (T は <see cref="double"/>) です。
    /// </remarks>
    public class SpeedLimitList : MapFunctionList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SpeedLimitList>();

            CurrentLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentLimit));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SpeedLimitList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SpeedLimitList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SpeedLimitList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new SpeedLimitList FromSource(object src) => src is null ? null : new SpeedLimitList((IList)src);

        private static FastMethod CurrentLimitGetMethod;
        /// <summary>
        /// 現在の制限速度 [m/s] を取得します。
        /// </summary>
        public double CurrentLimit => (double)CurrentLimitGetMethod.Invoke(Src, null);
    }
}
