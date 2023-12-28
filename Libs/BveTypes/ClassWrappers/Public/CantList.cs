using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// <see cref="Cant"/> のリストを表します。
    /// </summary>
    public class CantList : InterpolatableMapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CantList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CantList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CantList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CantList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new CantList FromSource(object src) => src is null ? null : new CantList((IList)src);
    }
}
