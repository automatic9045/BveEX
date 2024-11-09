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
    /// 変位、速度、加速度から成る物理量を表します。
    /// </summary>
    /// <remarks>
    /// このクラスのオリジナル型は構造体であることに注意してください。
    /// </remarks>
    public class Physical : ClassWrapperBase
    {
        private static Type OriginalType;

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Physical>();

            OriginalType = members.OriginalType;

            DisplacementField = members.GetSourceFieldOf(nameof(Displacement));
            VelocityField = members.GetSourceFieldOf(nameof(Velocity));
            AccelerationField = members.GetSourceFieldOf(nameof(Acceleration));

            AddMethod = members.GetSourceMethodOf("Add");
            SubtractMethod = members.GetSourceMethodOf("Add");
            MultiplyMethod = members.GetSourceMethodOf("Add");
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Physical"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Physical(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Physical"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Physical FromSource(object src) => src is null ? null : new Physical(src);

        /// <summary>
        /// <see cref="Physical"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="displacement">変位。</param>
        /// <param name="velocity">速度。</param>
        /// <param name="acceleration">加速度。</param>
        public Physical(double displacement, double velocity, double acceleration)
            : this(Activator.CreateInstance(OriginalType, displacement, velocity, acceleration))
        {
        }

        /// <summary>
        /// 加速度が 0 の <see cref="Physical"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="displacement">変位。</param>
        /// <param name="velocity">速度。</param>
        public Physical(double displacement, double velocity)
            : this(Activator.CreateInstance(OriginalType, displacement, velocity))
        {
        }

        /// <summary>
        /// 速度、加速度が 0 の <see cref="Physical"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="displacement">変位。</param>
        public Physical(double displacement)
            : this(Activator.CreateInstance(OriginalType, displacement))
        {
        }

        private static FastField DisplacementField;
        /// <summary>
        /// 変位を取得・設定します。
        /// </summary>
        public double Displacement
        {
            get => (double)DisplacementField.GetValue(Src);
            set => DisplacementField.SetValue(Src, value);
        }

        private static FastField VelocityField;
        /// <summary>
        /// 速度を取得・設定します。
        /// </summary>
        public double Velocity
        {
            get => (double)VelocityField.GetValue(Src);
            set => VelocityField.SetValue(Src, value);
        }

        private static FastField AccelerationField;
        /// <summary>
        /// 加速度を取得・設定します。
        /// </summary>
        public double Acceleration
        {
            get => (double)AccelerationField.GetValue(Src);
            set => AccelerationField.SetValue(Src, value);
        }

        private static FastMethod AddMethod;
        private static Physical Add(Physical a, Physical b) => (Physical)AddMethod.Invoke(null, new object[] { a, b });

        /// <summary>
        /// 2 つの <see cref="Physical"/> を足し合わせます。
        /// </summary>
        /// <param name="a">左辺。</param>
        /// <param name="b">右辺。</param>
        /// <returns><paramref name="a"/> と <paramref name="b"/> を足し合わせた結果。</returns>
        public static Physical operator +(Physical a, Physical b) => Add(a, b);

        private static FastMethod SubtractMethod;
        private static Physical Subtract(Physical a, Physical b) => (Physical)SubtractMethod.Invoke(null, new object[] { a, b });

        /// <summary>
        /// 一方の <see cref="Physical"/> からもう一方の <see cref="Physical"/> を引きます。
        /// </summary>
        /// <param name="a">左辺。</param>
        /// <param name="b">右辺。</param>
        /// <returns><paramref name="a"/> から <paramref name="b"/> を引いた結果。</returns>
        public static Physical operator -(Physical a, Physical b) => Subtract(a, b);

        private static FastMethod MultiplyMethod;
        private static Physical Multiply(double a, Physical b) => (Physical)MultiplyMethod.Invoke(null, new object[] { a, b });

        /// <summary>
        /// <see cref="Physical"/> を実数倍します。
        /// </summary>
        /// <param name="a">左辺。</param>
        /// <param name="b">右辺。</param>
        /// <returns><paramref name="b"/> を <paramref name="a"/> 倍した結果。</returns>
        public static Physical operator *(double a, Physical b) => Multiply(a, b);
    }
}
