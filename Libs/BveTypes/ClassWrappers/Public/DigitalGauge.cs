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
    /// 自列車の運転台の円形デジタルゲージ計器要素を表します。
    /// </summary>
    public class DigitalGauge : CircularGauge
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<DigitalGauge>();

            Constructor = members.GetSourceConstructor();

            StepGetMethod = members.GetSourcePropertyGetterOf(nameof(Step));
            StepSetMethod = members.GetSourcePropertySetterOf(nameof(Step));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="DigitalGauge"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected DigitalGauge(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="DigitalGauge"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new DigitalGauge FromSource(object src) => src is null ? null : new DigitalGauge(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="DigitalGauge"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DigitalGauge() : this(Constructor.Invoke(null))
        {
        }

        private static FastMethod StepGetMethod;
        private static FastMethod StepSetMethod;
        /// <summary>
        /// ゲージの分解能を取得・設定します。
        /// </summary>
        public double Step
        {
            get => (double)StepGetMethod.Invoke(Src, null);
            set => StepSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
