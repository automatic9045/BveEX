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
    /// 単独で設置された <see cref="Structure"/> のリストを表します。
    /// </summary>
    public class SingleStructureList : MapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SingleStructureList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SingleStructureList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SingleStructureList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SingleStructureList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new SingleStructureList FromSource(object src) => src is null ? null : new SingleStructureList((IList)src);
    }
}
