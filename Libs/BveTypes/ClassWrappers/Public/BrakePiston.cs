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
    /// 基礎ブレーキ装置のピストンを表します。
    /// </summary>
    public class BrakePiston : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakePiston>();

            StrokeGetMethod = members.GetSourcePropertyGetterOf(nameof(Stroke));
            StrokeSetMethod = members.GetSourcePropertySetterOf(nameof(Stroke));

            ForceGetMethod = members.GetSourcePropertyGetterOf(nameof(Force));

            AreaGetMethod = members.GetSourcePropertyGetterOf(nameof(Area));
            AreaSetMethod = members.GetSourcePropertySetterOf(nameof(Area));

            ApplyPressureField = members.GetSourceFieldOf(nameof(ApplyPressure));
            ReleasePressureField = members.GetSourceFieldOf(nameof(ReleasePressure));
            SpeedField = members.GetSourceFieldOf(nameof(Speed));

            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BrakePiston"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakePiston(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BrakePiston"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BrakePiston FromSource(object src) => src is null ? null : new BrakePiston(src);

        private static FastMethod StrokeGetMethod;
        private static FastMethod StrokeSetMethod;
        /// <summary>
        /// ピストンの最大位置 [-] を取得・設定します。
        /// </summary>
        public double Stroke
        {
            get => (double)StrokeGetMethod.Invoke(Src, null);
            set => StrokeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ForceGetMethod;
        /// <summary>
        /// 1 両当たりのピストンの押し付け力 [N] を取得します。
        /// </summary>
        public ValueContainer Force => ValueContainer.FromSource(ForceGetMethod.Invoke(Src, null));

        private static FastMethod AreaGetMethod;
        private static FastMethod AreaSetMethod;
        /// <summary>
        /// てこ比を 1、機械的損失を 0 としたときの 1 両あたりのシリンダ受圧面積 [m^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 必要に応じて <see cref="AirSupplement.PistonArea"/> プロパティも設定してください。
        /// </remarks>
        /// <seealso cref="AirSupplement.PistonArea"/>
        public double Area
        {
            get => (double)AreaGetMethod.Invoke(Src, null);
            set => AreaSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField ApplyPressureField;
        /// <summary>
        /// ピストンが伸びる最低圧 [Pa] を取得・設定します。
        /// </summary>
        public double ApplyPressure
        {
            get => (double)ApplyPressureField.GetValue(Src);
            set => ApplyPressureField.SetValue(Src, value);
        }

        private static FastField ReleasePressureField;
        /// <summary>
        /// ピストンが縮む最高圧 [Pa] を取得・設定します。
        /// </summary>
        public double ReleasePressure
        {
            get => (double)ReleasePressureField.GetValue(Src);
            set => ReleasePressureField.SetValue(Src, value);
        }

        private static FastField SpeedField;
        /// <summary>
        /// ピストンの伸縮速度 [Pa^(1/2)/s] を取得・設定します。
        /// </summary>
        public double Speed
        {
            get => (double)SpeedField.GetValue(Src);
            set => SpeedField.SetValue(Src, value);
        }

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

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize()
            => InitializeMethod.Invoke(Src, null);
    }
}
