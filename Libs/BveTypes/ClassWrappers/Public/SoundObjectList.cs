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
    /// サウンド マップ オブジェクトのリストを表します。
    /// </summary>
    public class SoundObjectList : MapFunctionList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SoundObjectList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SoundObjectList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SoundObjectList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SoundObjectList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new SoundObjectList FromSource(object src) => src is null ? null : new SoundObjectList((IList)src);
    }
}
