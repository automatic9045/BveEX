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
    /// 「曲線・勾配」補助表示を表します。
    /// </summary>
    public class GeometryText : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<GeometryText>();

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="GeometryText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected GeometryText(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="GeometryText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new GeometryText FromSource(object src) => src is null ? null : new GeometryText(src);

        private static FastMethod DrawMethod;
        /// <inheritdoc/>
        public override void Draw() => DrawMethod.Invoke(Src, null);
    }
}
