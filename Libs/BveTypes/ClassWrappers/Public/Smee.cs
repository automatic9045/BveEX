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
    /// 電磁直通空気ブレーキを表します。
    /// </summary>
    public class Smee : BrakeControllerBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Smee>();

            SapGetMethod = members.GetSourcePropertyGetterOf(nameof(Sap));
            BpGetMethod = members.GetSourcePropertyGetterOf(nameof(Bp));

            BpInitialPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(BpInitialPressure));
            BpInitialPressureSetMethod = members.GetSourcePropertySetterOf(nameof(BpInitialPressure));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Smee"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Smee(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Smee"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Smee FromSource(object src) => src is null ? null : new Smee(src);

        private static FastMethod SapGetMethod;
        /// <summary>
        /// 直通管の圧力調整弁を取得します。
        /// </summary>
        public Valve Sap => Valve.FromSource(SapGetMethod.Invoke(Src, null));

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
    }
}
