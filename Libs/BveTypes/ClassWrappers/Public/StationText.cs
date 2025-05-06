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
    /// 「次駅情報」補助表示を表します。
    /// </summary>
    public class StationText : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<StationText>();

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StationText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StationText(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StationText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new StationText FromSource(object src) => src is null ? null : new StationText(src);

        private static FastMethod DrawMethod;
        /// <inheritdoc/>
        public override void Draw() => DrawMethod.Invoke(Src, null);

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public override void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
