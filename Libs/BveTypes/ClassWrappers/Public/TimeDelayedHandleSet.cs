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
    /// むだ時間フィルタを適用したハンドルのセットを表します。
    /// </summary>
    public class TimeDelayedHandleSet : HandleSet, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TimeDelayedHandleSet>();

            Constructor = members.GetSourceConstructor();

            DelaySetMethod = members.GetSourcePropertySetterOf(nameof(Delay));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            UpdateMethod = members.GetSourceMethodOf(nameof(Update));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TimeDelayedHandleSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TimeDelayedHandleSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TimeDelayedHandleSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new TimeDelayedHandleSet FromSource(object src) => src is null ? null : new TimeDelayedHandleSet(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="TimeDelayedHandleSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="baseHandles">入力ソースとなるハンドルのセット。</param>
        public TimeDelayedHandleSet(HandleSet baseHandles) : this(Constructor.Invoke(new object[] { baseHandles?.Src }))
        {
        }

        private static FastMethod DelaySetMethod;
        /// <summary>
        /// ハンドル出力の遅延時間 [s] を設定します。
        /// </summary>
        public double Delay
        {
            set => DelaySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize() => InitializeMethod.Invoke(Src, null);

        private static FastMethod UpdateMethod;
        /// <summary>
        /// ハンドル出力を最新の状態に更新します。
        /// </summary>
        public void Update() => UpdateMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public virtual void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドはオリジナルではないため、<see cref="ClassMemberSet.GetSourceMethodOf(string, Type[])"/> メソッドから参照することはできません。<br/>
        /// このメソッドのオリジナルバージョンは <see cref="Tick(double)"/> です。
        /// </remarks>
        /// <param name="elapsed">前フレームからの経過時間。</param>
        /// <seealso cref="Tick(double)"/>
        public virtual void Tick(TimeSpan elapsed) => Tick(elapsed.TotalSeconds);
    }
}
