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
    /// 自列車のジャーク制御機構を表します。
    /// </summary>
    public class JerkRegulator : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<JerkRegulator>();

            MotorStateGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorState));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            CalculateRawCurrentMethod = members.GetSourceMethodOf(nameof(CalculateRawCurrent));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="JerkRegulator"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected JerkRegulator(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="JerkRegulator"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static JerkRegulator FromSource(object src) => src is null ? null : new JerkRegulator(src);

        private static FastMethod MotorStateGetMethod;
        /// <summary>
        /// 自列車のモーターの状態を取得します。
        /// </summary>
        public VehicleMotorState MotorState => VehicleMotorState.FromSource(MotorStateGetMethod.Invoke(Src, null));

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize()
            => InitializeMethod.Invoke(Src, null);

        private static FastMethod CalculateRawCurrentMethod;
        /// <summary>
        /// ジャーク制御介入前の電流値 [A] を計算します。
        /// </summary>
        /// <returns>ジャーク制御介入前の電流値 [A]。</returns>
        public double CalculateRawCurrent()
            => CalculateRawCurrentMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <summary>
        /// 毎フレーム呼び出され、<see cref="MotorState"/> の値を更新します。
        /// </summary>
        /// <param name="elapsedSeconds">前フレームからの経過時間 [s]。</param>
        public void Tick(double elapsedSeconds)
            => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出され、<see cref="MotorState"/> の値を更新します。
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
