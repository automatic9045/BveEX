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
    /// 「内部計算値」補助表示を表します。
    /// </summary>
    public class DetailText : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<DetailText>();

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="DetailText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected DetailText(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="DetailText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new DetailText FromSource(object src) => src is null ? null : new DetailText(src);

        private static FastMethod DrawMethod;
        /// <inheritdoc/>
        public override void Draw() => DrawMethod.Invoke(Src, null);
    }
}
