using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の運転台の、円形の計器要素の基本クラスを表します。
    /// </summary>
    public class CircularGauge : VehiclePanelElement
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CircularGauge>();

            Constructor = members.GetSourceConstructor();

            RadiusGetMethod = members.GetSourcePropertyGetterOf(nameof(Radius));
            RadiusSetMethod = members.GetSourcePropertySetterOf(nameof(Radius));

            InitialAngleGetMethod = members.GetSourcePropertyGetterOf(nameof(InitialAngle));
            InitialAngleSetMethod = members.GetSourcePropertySetterOf(nameof(InitialAngle));

            LastAngleGetMethod = members.GetSourcePropertyGetterOf(nameof(LastAngle));
            LastAngleSetMethod = members.GetSourcePropertySetterOf(nameof(LastAngle));

            MaxGetMethod = members.GetSourcePropertyGetterOf(nameof(Max));
            MaxSetMethod = members.GetSourcePropertySetterOf(nameof(Max));

            MinGetMethod = members.GetSourcePropertyGetterOf(nameof(Min));
            MinSetMethod = members.GetSourcePropertySetterOf(nameof(Min));

            TiltGetMethod = members.GetSourcePropertyGetterOf(nameof(Tilt));
            TiltSetMethod = members.GetSourcePropertySetterOf(nameof(Tilt));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CircularGauge"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CircularGauge(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CircularGauge"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new CircularGauge FromSource(object src) => src is null ? null : new CircularGauge(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="CircularGauge"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public CircularGauge() : this(Constructor.Invoke(null))
        {
        }

        private static FastMethod RadiusGetMethod;
        private static FastMethod RadiusSetMethod;
        /// <summary>
        /// 針やゲージの半径を取得・設定します。
        /// </summary>
        public double Radius
        {
            get => (double)RadiusGetMethod.Invoke(Src, null);
            set => RadiusSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod InitialAngleGetMethod;
        private static FastMethod InitialAngleSetMethod;
        /// <summary>
        /// 最小値を指す針やゲージの角度 [rad] を取得・設定します。
        /// </summary>
        public double InitialAngle
        {
            get => (double)InitialAngleGetMethod.Invoke(Src, null);
            set => InitialAngleSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod LastAngleGetMethod;
        private static FastMethod LastAngleSetMethod;
        /// <summary>
        /// 最大値を指す針やゲージの角度 [rad] を取得・設定します。
        /// </summary>
        public double LastAngle
        {
            get => (double)LastAngleGetMethod.Invoke(Src, null);
            set => LastAngleSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MaxGetMethod;
        private static FastMethod MaxSetMethod;
        /// <summary>
        /// この描画要素が表せる最大値を取得・設定します。
        /// </summary>
        public double Max
        {
            get => (double)MaxGetMethod.Invoke(Src, null);
            set => MaxSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MinGetMethod;
        private static FastMethod MinSetMethod;
        /// <summary>
        /// この描画要素が表せる最小値を取得・設定します。
        /// </summary>
        public double Min
        {
            get => (double)MinGetMethod.Invoke(Src, null);
            set => MinSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TiltGetMethod;
        private static FastMethod TiltSetMethod;
        /// <summary>
        /// 垂直面に対する表示面の角度 [rad] を取得・設定します。
        /// </summary>
        public Vector2 Tilt
        {
            get => (Vector2)TiltGetMethod.Invoke(Src, null);
            set => TiltSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
