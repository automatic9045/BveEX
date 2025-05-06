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
    /// 「運転指示」補助表示を表します。
    /// </summary>
    public class InstructionText : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<InstructionText>();

            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="InstructionText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected InstructionText(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="InstructionText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new InstructionText FromSource(object src) => src is null ? null : new InstructionText(src);

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public override void Dispose() => DisposeMethod.Invoke(Src, null);

        private static FastMethod DrawMethod;
        /// <inheritdoc/>
        public override void Draw() => DrawMethod.Invoke(Src, null);
    }
}
