using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自軌道のカーブのリストを表します。
    /// </summary>
    public class CurveList : MapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CurveList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CurveList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CurveList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CurveList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new CurveList FromSource(object src) => src is null ? null : new CurveList((IList)src);
    }
}
