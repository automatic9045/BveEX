using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の運転台の、すべての計器要素の基本クラスを表します。
    /// </summary>
    public class VehiclePanelElement : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehiclePanelElement>();

            Constructor = members.GetSourceConstructor();

            ColorGetMethod = members.GetSourcePropertyGetterOf(nameof(Color));
            ColorSetMethod = members.GetSourcePropertySetterOf(nameof(Color));

            LayerGetMethod = members.GetSourcePropertyGetterOf(nameof(Layer));
            LayerSetMethod = members.GetSourcePropertySetterOf(nameof(Layer));

            SubjectGetMethod = members.GetSourcePropertyGetterOf(nameof(Subject));
            SubjectSetMethod = members.GetSourcePropertySetterOf(nameof(Subject));

            SubjectIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(SubjectIndex));
            SubjectIndexSetMethod = members.GetSourcePropertySetterOf(nameof(SubjectIndex));

            ImageLocationSetMethod = members.GetSourcePropertySetterOf(nameof(ImageLocation));

            ImageRectangleGetMethod = members.GetSourcePropertyGetterOf(nameof(ImageRectangle));
            ImageRectangleSetMethod = members.GetSourcePropertySetterOf(nameof(ImageRectangle));

            BrightnessSetMethod = members.GetSourcePropertySetterOf(nameof(Brightness));

            DaytimeImageModelField = members.GetSourceFieldOf(nameof(DaytimeImageModel));
            DaytimeImagePathField = members.GetSourceFieldOf(nameof(DaytimeImagePath));
            NighttimeImageModelField = members.GetSourceFieldOf(nameof(NighttimeImageModel));
            NighttimeImagePathField = members.GetSourceFieldOf(nameof(NighttimeImagePath));
            MatrixField = members.GetSourceFieldOf(nameof(Matrix));

            CreateModelsMethod = members.GetSourceMethodOf(nameof(CreateModels));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehiclePanelElement"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehiclePanelElement(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。    
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehiclePanelElement"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehiclePanelElement FromSource(object src) => src is null ? null : new VehiclePanelElement(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="VehiclePanelElement"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public VehiclePanelElement() : this(Constructor.Invoke(null))
        {
        }

        private static FastMethod ColorGetMethod;
        private static FastMethod ColorSetMethod;
        /// <summary>
        /// 物体色を取得・設定します。
        /// </summary>
        public Color Color
        {
            get => ColorGetMethod.Invoke(Src, null);
            set => ColorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod LayerGetMethod;
        private static FastMethod LayerSetMethod;
        /// <summary>
        /// この計器要素の描画順序を取得・設定します。
        /// </summary>
        public double Layer
        {
            get => LayerGetMethod.Invoke(Src, null);
            set => LayerSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SubjectGetMethod;
        private static FastMethod SubjectSetMethod;
        /// <summary>
        /// 表示する状態量を取得・設定します。
        /// </summary>
        public double[] Subject
        {
            get => SubjectGetMethod.Invoke(Src, null);
            set => SubjectSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SubjectIndexGetMethod;
        private static FastMethod SubjectIndexSetMethod;
        /// <summary>
        /// 表示する状態量のインデックスを取得・設定します。
        /// </summary>
        public int SubjectIndex
        {
            get => SubjectIndexGetMethod.Invoke(Src, null);
            set => SubjectIndexSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ImageLocationSetMethod;
        /// <summary>
        /// この計器要素の画像の描画位置を <see cref="ImageRectangle"/> プロパティへ設定します。
        /// </summary>
        public Point ImageLocation
        {
            set => ImageLocationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ImageRectangleGetMethod;
        private static FastMethod ImageRectangleSetMethod;
        /// <summary>
        /// この計器要素の画像の描画位置・サイズを取得・設定します。
        /// </summary>
        public Rectangle ImageRectangle
        {
            get => ImageRectangleGetMethod.Invoke(Src, null);
            set => ImageRectangleSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BrightnessSetMethod;
        /// <summary>
        /// この計器要素の明るさを設定します。
        /// </summary>
        public double Brightness
        {
            set => BrightnessSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField DaytimeImageModelField;
        /// <summary>
        /// 昼間画像の 3D モデルを取得・設定します。
        /// </summary>
        public Model DaytimeImageModel
        {
            get => Model.FromSource(DaytimeImageModelField.GetValue(Src));
            set => DaytimeImageModelField.SetValue(Src, value?.Src);
        }

        private static FastField DaytimeImagePathField;
        /// <summary>
        /// 昼間画像のパスを取得・設定します。
        /// </summary>
        public string DaytimeImagePath
        {
            get => DaytimeImagePathField.GetValue(Src);
            set => DaytimeImagePathField.SetValue(Src, value);
        }

        private static FastField NighttimeImageModelField;
        /// <summary>
        /// 夜間画像の 3D モデルを取得・設定します。
        /// </summary>
        public Model NighttimeImageModel
        {
            get => Model.FromSource(NighttimeImageModelField.GetValue(Src));
            set => NighttimeImageModelField.SetValue(Src, value?.Src);
        }

        private static FastField NighttimeImagePathField;
        /// <summary>
        /// 夜間画像のパスを取得・設定します。
        /// </summary>
        public string NighttimeImagePath
        {
            get => NighttimeImagePathField.GetValue(Src);
            set => NighttimeImagePathField.SetValue(Src, value);
        }

        private static FastField MatrixField;
        /// <summary>
        /// この計器要素の画像を配置するための変換行列を取得・設定します。
        /// </summary>
        public Matrix Matrix
        {
            get => MatrixField.GetValue(Src);
            set => MatrixField.SetValue(Src, value);
        }

        private static FastMethod CreateModelsMethod;
        /// <summary>
        /// この計器要素の 3D モデルを作成し、描画位置を設定します。
        /// </summary>
        /// <remarks>
        /// 既定では <see cref="DaytimeImagePath"/>、<see cref="NighttimeImagePath"/>、<see cref="ImageRectangle"/> の値を参照し、
        /// 結果を <see cref="DaytimeImageModel"/>、<see cref="NighttimeImageModel"/>、<see cref="Matrix"/> へ代入します。
        /// </remarks>
        public void CreateModels() => CreateModelsMethod.Invoke(Src, null);

        private static FastMethod DrawMethod;
        /// <summary>
        /// この計器要素を描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        /// <param name="world">ワールド変換行列。</param>
        public void Draw(Direct3DProvider direct3DProvider, Matrix world)
            => DrawMethod.Invoke(Src, new object[] { direct3DProvider?.Src, world });

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
