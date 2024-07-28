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
    /// マップ上に設置可能な固定音源のリストを表します。
    /// </summary>
    public class Sound3DObjectList : MapFunctionList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Sound3DObjectList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Sound3DObjectList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Sound3DObjectList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Sound3DObjectList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new Sound3DObjectList FromSource(object src) => src is null ? null : new Sound3DObjectList((IList)src);
    }
}
