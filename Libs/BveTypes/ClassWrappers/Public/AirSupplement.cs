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
    /// 遅れ込め制御式電空協調制御を表します。
    /// </summary>
    public class AirSupplement : BrakeBlenderBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<AirSupplement>();

            ShoeFrictionGetMethod = members.GetSourcePropertyGetterOf(nameof(ShoeFriction));
            ShoeFrictionSetMethod = members.GetSourcePropertySetterOf(nameof(ShoeFriction));

            MotorCarRatioGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCarRatio));
            MotorCarRatioSetMethod = members.GetSourcePropertySetterOf(nameof(MotorCarRatio));

            PistonAreaGetMethod = members.GetSourcePropertyGetterOf(nameof(PistonArea));
            PistonAreaSetMethod = members.GetSourcePropertySetterOf(nameof(PistonArea));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AirSupplement"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected AirSupplement(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="AirSupplement"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static AirSupplement FromSource(object src) => src is null ? null : new AirSupplement(src);

        private static FastMethod ShoeFrictionGetMethod;
        private static FastMethod ShoeFrictionSetMethod;
        /// <summary>
        /// 電空演算に用いる制輪子の想定摩擦係数を取得・設定します。
        /// </summary>
        public double ShoeFriction
        {
            get => (double)ShoeFrictionGetMethod.Invoke(Src, null);
            set => ShoeFrictionSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MotorCarRatioGetMethod;
        private static FastMethod MotorCarRatioSetMethod;
        /// <summary>
        /// 編成の電動車率を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 必要に応じて <see cref="CarInfo.Count"/> プロパティも設定してください。
        /// </remarks>
        /// <seealso cref="CarInfo.Count"/>
        public double MotorCarRatio
        {
            get => (double)MotorCarRatioGetMethod.Invoke(Src, null);
            set => MotorCarRatioSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PistonAreaGetMethod;
        private static FastMethod PistonAreaSetMethod;
        /// <summary>
        /// てこ比を 1、機械的損失を 0 としたときの 1 両あたりのシリンダ受圧面積 [m^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 必要に応じて <see cref="BrakePiston.Area"/> プロパティも設定してください。
        /// </remarks>
        /// <seealso cref="BrakePiston.Area"/>
        public double PistonArea
        {
            get => (double)PistonAreaGetMethod.Invoke(Src, null);
            set => PistonAreaSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public override void Initialize()
            => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public override void Tick(double elapsedSeconds)
            => TickMethod.Invoke(Src, new object[] { elapsedSeconds });
    }
}
