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
    /// むだ時間フィルタを適用し、定速制御のモードを反映したハンドルのセットを表します。
    /// </summary>
    public class CscFilteredTimeDelayedHandleSet : TimeDelayedHandleSet
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CscFilteredTimeDelayedHandleSet>();

            Constructor = members.GetSourceConstructor();

            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CscFilteredTimeDelayedHandleSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CscFilteredTimeDelayedHandleSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CscFilteredTimeDelayedHandleSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new CscFilteredTimeDelayedHandleSet FromSource(object src) => src is null ? null : new CscFilteredTimeDelayedHandleSet(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="CscFilteredTimeDelayedHandleSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="baseHandles">入力ソースとなるハンドルのセット。</param>
        public CscFilteredTimeDelayedHandleSet(HandleSet baseHandles) : this(Constructor.Invoke(new object[] { baseHandles?.Src }))
        {
        }

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public override void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドはオリジナルではないため、<see cref="ClassMemberSet.GetSourceMethodOf(string, Type[])"/> メソッドから参照することはできません。<br/>
        /// このメソッドのオリジナルバージョンは <see cref="Tick(double)"/> です。
        /// </remarks>
        /// <param name="elapsed">前フレームからの経過時間。</param>
        /// <seealso cref="Tick(double)"/>
        public override void Tick(TimeSpan elapsed) => Tick(elapsed.TotalSeconds);
    }
}
