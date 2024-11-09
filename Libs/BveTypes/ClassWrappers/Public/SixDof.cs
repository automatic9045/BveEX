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
    /// 6DoF (3 次元における剛体の運動の自由度) を表します。
    /// </summary>
    /// <remarks>
    /// このクラスのオリジナル型は構造体であることに注意してください。
    /// </remarks>
    public class SixDof : ClassWrapperBase
    {
        private static Type OriginalType;

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SixDof>();

            OriginalType = members.OriginalType;

            XField = members.GetSourceFieldOf(nameof(X));
            YField = members.GetSourceFieldOf(nameof(Y));
            ZField = members.GetSourceFieldOf(nameof(Z));
            RotationXField = members.GetSourceFieldOf(nameof(RotationX));
            RotationYField = members.GetSourceFieldOf(nameof(RotationY));
            RotationZField = members.GetSourceFieldOf(nameof(RotationZ));

            AddMethod = members.GetSourceMethodOf(nameof(Add));
            MultiplyMethod = members.GetSourceMethodOf(nameof(Multiply));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SixDof"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SixDof(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SixDof"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static SixDof FromSource(object src) => src is null ? null : new SixDof(src);

        /// <summary>
        /// <see cref="SixDof"/> 同士の加算を行います。
        /// </summary>
        /// <param name="x">左辺。</param>
        /// <param name="y">右辺。</param>
        /// <returns><paramref name="x"/> と <paramref name="y"/> の和。</returns>
        public static SixDof operator +(SixDof x, SixDof y) => Add(x, y);

        /// <summary>
        /// <see cref="SixDof"/> を実数倍します。
        /// </summary>
        /// <param name="x">かける数。</param>
        /// <param name="y">元となる <see cref="SixDof"/>。</param>
        /// <returns><paramref name="y"/> を <paramref name="x"/> 倍した値。</returns>
        public static SixDof operator *(double x, SixDof y) => Multiply(x, y);

        /// <summary>
        /// 座標、回転を指定して <see cref="SixDof"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="x">X 座標。</param>
        /// <param name="y">Y 座標。</param>
        /// <param name="z">Z 座標。</param>
        /// <param name="rotationX">X 軸まわりの回転 (ピッチ)。</param>
        /// <param name="rotationY">Y 軸まわりの回転 (ヨー)。</param>
        /// <param name="rotationZ">Z 軸まわりの回転 (ロール)。</param>
        public SixDof(double x, double y, double z, double rotationX, double rotationY, double rotationZ)
            : this(Activator.CreateInstance(OriginalType, x, y, z, rotationX, rotationY, rotationZ))
        {
        }

        private static FastField XField;
        /// <summary>
        /// X 座標を取得・設定します。
        /// </summary>
        public double X
        {
            get => (double)XField.GetValue(Src);
            set => XField.SetValue(Src, value);
        }

        private static FastField YField;
        /// <summary>
        /// Y 座標を取得・設定します。
        /// </summary>
        public double Y
        {
            get => (double)YField.GetValue(Src);
            set => YField.SetValue(Src, value);
        }

        private static FastField ZField;
        /// <summary>
        /// Z 座標を取得・設定します。
        /// </summary>
        public double Z
        {
            get => (double)ZField.GetValue(Src);
            set => ZField.SetValue(Src, value);
        }

        private static FastField RotationXField;
        /// <summary>
        /// X 軸まわりの回転 (ピッチ) を取得・設定します。
        /// </summary>
        public double RotationX
        {
            get => (double)RotationXField.GetValue(Src);
            set => RotationXField.SetValue(Src, value);
        }

        private static FastField RotationYField;
        /// <summary>
        /// Y 軸まわりの回転 (ヨー) を取得・設定します。
        /// </summary>
        public double RotationY
        {
            get => (double)RotationYField.GetValue(Src);
            set => RotationYField.SetValue(Src, value);
        }

        private static FastField RotationZField;
        /// <summary>
        /// Z 軸まわりの回転 (ロール) を取得・設定します。
        /// </summary>
        public double RotationZ
        {
            get => (double)RotationZField.GetValue(Src);
            set => RotationZField.SetValue(Src, value);
        }

        private static FastMethod AddMethod;
        private static SixDof Add(SixDof x, SixDof y) => SixDof.FromSource(AddMethod.Invoke(null, new object[] { x?.Src, y?.Src }));

        private static FastMethod MultiplyMethod;
        private static SixDof Multiply(double x, SixDof y) => SixDof.FromSource(MultiplyMethod.Invoke(null, new object[] { x, y?.Src }));
    }
}
