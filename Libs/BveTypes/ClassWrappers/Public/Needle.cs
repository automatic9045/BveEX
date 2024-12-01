using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の運転台の指針計器要素を表します。
    /// </summary>
    public class Needle : CircularGauge
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Needle>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(TimeManager) });

            OriginSetMethod = members.GetSourcePropertySetterOf(nameof(Origin));

            NaturalFrequencyGetMethod = members.GetSourcePropertyGetterOf(nameof(NaturalFrequency));
            NaturalFrequencySetMethod = members.GetSourcePropertySetterOf(nameof(NaturalFrequency));

            DampingRatioGetMethod = members.GetSourcePropertyGetterOf(nameof(DampingRatio));
            DampingRatioSetMethod = members.GetSourcePropertySetterOf(nameof(DampingRatio));

            IsStopPinEnabledGetMethod = members.GetSourcePropertyGetterOf(nameof(IsStopPinEnabled));
            IsStopPinEnabledSetMethod = members.GetSourcePropertySetterOf(nameof(IsStopPinEnabled));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Needle"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Needle(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Needle"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new Needle FromSource(object src) => src is null ? null : new Needle(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="Needle"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="timeManager">針の振動の計算に用いる <see cref="TimeManager"/>。</param>
        public Needle(TimeManager timeManager) : this(Constructor.Invoke(new object[] { timeManager?.Src }))
        {
        }

        private static FastMethod OriginSetMethod;
        /// <summary>
        /// 針の画像における回転中心を設定します。
        /// </summary>
        public Point Origin
        {
            set => OriginSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod NaturalFrequencyGetMethod;
        private static FastMethod NaturalFrequencySetMethod;
        /// <summary>
        /// 針の固有角振動数 [Hz] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 94.24778 より大きい値を設定することはできません。
        /// </remarks>
        public double NaturalFrequency
        {
            get => (double)NaturalFrequencyGetMethod.Invoke(Src, null);
            set => NaturalFrequencySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DampingRatioGetMethod;
        private static FastMethod DampingRatioSetMethod;
        /// <summary>
        /// 針の減衰比を取得・設定します。
        /// </summary>
        public double DampingRatio
        {
            get => (double)DampingRatioGetMethod.Invoke(Src, null);
            set => DampingRatioSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod IsStopPinEnabledGetMethod;
        private static FastMethod IsStopPinEnabledSetMethod;
        /// <summary>
        /// 針の可動範囲を <see cref="CircularGauge.Min"/> と <see cref="CircularGauge.Max"/> の間に制限するかどうかを取得・設定します。
        /// </summary>
        public bool IsStopPinEnabled
        {
            get => (bool)IsStopPinEnabledGetMethod.Invoke(Src, null);
            set => IsStopPinEnabledSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
