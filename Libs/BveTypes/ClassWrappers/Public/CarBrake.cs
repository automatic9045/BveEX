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
    /// 車両のブレーキを表します。
    /// </summary>
    public class CarBrake : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CarBrake>();

            BcValveGetMethod = members.GetSourcePropertyGetterOf(nameof(BcValve));
            BasicBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(BasicBrake));
            BrakeReAdhesionGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeReAdhesion));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CarBrake"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CarBrake(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CarBrake"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static CarBrake FromSource(object src) => src is null ? null : new CarBrake(src);

        private static FastMethod BcValveGetMethod;
        /// <summary>
        /// ブレーキシリンダ電磁弁を表す <see cref="ClassWrappers.BcValve"/> を取得します。
        /// </summary>
        public BcValve BcValve => ClassWrappers.BcValve.FromSource(BcValveGetMethod.Invoke(Src, null));

        private static FastMethod BasicBrakeGetMethod;
        /// <summary>
        /// 基礎ブレーキ装置を表す <see cref="ClassWrappers.BasicBrake"/> を取得します。
        /// </summary>
        public BasicBrake BasicBrake => ClassWrappers.BasicBrake.FromSource(BasicBrakeGetMethod.Invoke(Src, null));

        private static FastMethod BrakeReAdhesionGetMethod;
        /// <summary>
        /// 基礎ブレーキ装置の滑走再粘着制御機構を取得します。
        /// </summary>
        public ReAdhesionControl BrakeReAdhesion => ReAdhesionControl.FromSource(BrakeReAdhesionGetMethod.Invoke(Src, null));

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize()
            => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public void Tick(double elapsedSeconds)
            => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

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
