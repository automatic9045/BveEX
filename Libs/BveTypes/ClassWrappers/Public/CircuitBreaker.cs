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
    /// 回路遮断器を表します。
    /// </summary>
    public class CircuitBreaker : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CircuitBreaker>();

            RegenerationLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(RegenerationLimit));
            RegenerationLimitSetMethod = members.GetSourcePropertySetterOf(nameof(RegenerationLimit));

            RegenerationStartLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(RegenerationStartLimit));
            RegenerationStartLimitSetMethod = members.GetSourcePropertySetterOf(nameof(RegenerationStartLimit));

            PerformanceGetMethod = members.GetSourcePropertyGetterOf(nameof(Performance));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CircuitBreaker"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CircuitBreaker(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CircuitBreaker"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static CircuitBreaker FromSource(object src) => src is null ? null : new CircuitBreaker(src);

        private static FastMethod RegenerationLimitGetMethod;
        private static FastMethod RegenerationLimitSetMethod;
        /// <summary>
        /// 電気ブレーキを遮断する走行速度 [m/s] を取得・設定します。
        /// </summary>
        public double RegenerationLimit
        {
            get => (double)RegenerationLimitGetMethod.Invoke(Src, null);
            set => RegenerationLimitSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RegenerationStartLimitGetMethod;
        private static FastMethod RegenerationStartLimitSetMethod;
        /// <summary>
        /// 電気ブレーキが開始可能な最低走行速度 [m/s] を取得・設定します。
        /// </summary>
        public double RegenerationStartLimit
        {
            get => (double)RegenerationStartLimitGetMethod.Invoke(Src, null);
            set => RegenerationStartLimitSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PerformanceGetMethod;
        /// <summary>
        /// 車両性能テーブルを取得します。
        /// </summary>
        public VehiclePerformance Performance => VehiclePerformance.FromSource(PerformanceGetMethod.Invoke(Src, null));

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize() => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドはオリジナルではないため、<see cref="ClassMemberSet.GetSourceMethodOf(string, Type[])"/> メソッドから参照することはできません。<br/>
        /// このメソッドのオリジナルバージョンは <see cref="Tick(double)"/> です。
        /// </remarks>
        /// <param name="elapsed">前フレームからの経過時間。</param>
        /// <seealso cref="Tick(double)"/>
        public void Tick(TimeSpan elapsed) => Tick(elapsed.TotalSeconds);
    }
}