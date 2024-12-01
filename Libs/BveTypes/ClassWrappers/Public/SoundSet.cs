using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// シナリオで使用するサウンドのセットを表します。
    /// </summary>
    public class SoundSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SoundSet>();

            ItemsGetMethod = members.GetSourcePropertyGetterOf(nameof(Items));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SoundSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SoundSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SoundSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static SoundSet FromSource(object src) => src is null ? null : new SoundSet(src);

        private static FastMethod ItemsGetMethod;
        /// <summary>
        /// 要素の一覧を取得します。
        /// </summary>
        public WrappedSortedList<string, Sound> Items => new WrappedSortedList<string, Sound>(ItemsGetMethod.Invoke(Src, null) as IDictionary);
    }
}
