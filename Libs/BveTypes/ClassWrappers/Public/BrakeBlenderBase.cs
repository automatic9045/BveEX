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
    /// 電空協調制御の基本クラスを表します。
    /// </summary>
    public class BrakeBlenderBase : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakeBlenderBase>();

            InitialPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(InitialPressure));
            InitialPressureSetMethod = members.GetSourcePropertySetterOf(nameof(InitialPressure));

            MaximumPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(MaximumPressure));
            MaximumPressureSetMethod = members.GetSourcePropertySetterOf(nameof(MaximumPressure));

            SapBcRatioGetMethod = members.GetSourcePropertyGetterOf(nameof(SapBcRatio));
            SapBcRatioSetMethod = members.GetSourcePropertySetterOf(nameof(SapBcRatio));

            SapBcOffsetGetMethod = members.GetSourcePropertyGetterOf(nameof(SapBcOffset));
            SapBcOffsetSetMethod = members.GetSourcePropertySetterOf(nameof(SapBcOffset));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BrakeBlenderBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakeBlenderBase(object src) : base(src)
        {
        }

        private static FastMethod InitialPressureGetMethod;
        private static FastMethod InitialPressureSetMethod;
        /// <summary>
        /// 初込め圧力 [Pa] を取得・設定します。
        /// </summary>
        public double InitialPressure
        {
            get => (double)InitialPressureGetMethod.Invoke(Src, null);
            set => InitialPressureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MaximumPressureGetMethod;
        private static FastMethod MaximumPressureSetMethod;
        /// <summary>
        /// 電気ブレーキ (限流値) が最大となるブレーキシリンダー圧力指令 (空車時) (ブレーキ方式が電磁直通空気ブレーキの場合は直通管圧力) [Pa] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 締切電磁弁式の場合、ブレーキシリンダー圧力指令がこの値を超えると遅れ込め圧力が立ち上がります。
        /// </remarks>
        public double MaximumPressure
        {
            get => (double)MaximumPressureGetMethod.Invoke(Src, null);
            set => MaximumPressureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SapBcRatioGetMethod;
        private static FastMethod SapBcRatioSetMethod;
        /// <summary>
        /// 圧力指令変化に対するブレーキシリンダー圧力変化の比を取得・設定します。
        /// </summary>
        public double SapBcRatio
        {
            get => (double)SapBcRatioGetMethod.Invoke(Src, null);
            set => SapBcRatioSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SapBcOffsetGetMethod;
        private static FastMethod SapBcOffsetSetMethod;
        /// <summary>
        /// ブレーキシリンダー圧力が上昇するための最低圧力指令 [Pa] を取得・設定します。
        /// </summary>
        public double SapBcOffset
        {
            get => (double)SapBcOffsetGetMethod.Invoke(Src, null);
            set => SapBcOffsetSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public virtual void Initialize()
            => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public virtual void Tick(double elapsedSeconds)
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
