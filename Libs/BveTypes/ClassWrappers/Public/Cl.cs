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
    /// 自動空気ブレーキを表します。
    /// </summary>
    public class Cl : BrakeControllerBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Cl>();

            ErGetMethod = members.GetSourcePropertyGetterOf(nameof(Er));
            BpGetMethod = members.GetSourcePropertyGetterOf(nameof(Bp));

            BpInitialPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(BpInitialPressure));
            BpInitialPressureSetMethod = members.GetSourcePropertySetterOf(nameof(BpInitialPressure));

            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Cl"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Cl(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Cl"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Cl FromSource(object src) => src is null ? null : new Cl(src);

        private static FastMethod ErGetMethod;
        /// <summary>
        /// 釣り合い空気溜めの圧力調整弁を取得します。
        /// </summary>
        public BpValve Er => BpValve.FromSource(ErGetMethod.Invoke(Src, null));

        private static FastMethod BpGetMethod;
        /// <summary>
        /// ブレーキ管の圧力調整弁を取得します。
        /// </summary>
        public BpValve Bp => BpValve.FromSource(BpGetMethod.Invoke(Src, null));

        private static FastMethod BpInitialPressureGetMethod;
        private static FastMethod BpInitialPressureSetMethod;
        /// <summary>
        /// ブレーキ緩解時のブレーキ管圧力 [Pa] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 電磁直通空気ブレーキおよび自動空気ブレーキの場合に限り認識されます。
        /// </remarks>
        public double BpInitialPressure
        {
            get => (double)BpInitialPressureGetMethod.Invoke(Src, null);
            set => BpInitialPressureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public override void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public override void Initialize() => InitializeMethod.Invoke(Src, null);
    }
}
