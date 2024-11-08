using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.Direct3D9;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 設置位置の情報を伴う 3D モデルを表します。
    /// </summary>
    public class LocatableModel : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<LocatableModel>();

            Constructor = members.GetSourceConstructor();

            ModelField = members.GetSourceFieldOf(nameof(Model));
            MatrixField = members.GetSourceFieldOf(nameof(Matrix));
            VerticesField = members.GetSourceFieldOf(nameof(Vertices));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="LocatableModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected LocatableModel(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="LocatableModel"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static LocatableModel FromSource(object src) => src is null ? null : new LocatableModel(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="LocatableModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="model">ソースとなる 3D モデル。</param>
        /// <param name="matrix">この 3D モデルを設置するための行列。</param>
        /// <param name="vertices">頂点の位置ベクトルの一覧。この値は Structure.PutBetween ステートメントによるストラクチャーを描画する際に使用されます。</param>
        public LocatableModel(Model model, Matrix matrix, Vector3[] vertices) : base(Constructor.Invoke(new object[] { model.Src, matrix, vertices }))
        {
        }

        private static FastField ModelField;
        /// <summary>
        /// ソースとなる 3D モデルを取得・設定します。
        /// </summary>
        public Model Model
        {
            get => ClassWrappers.Model.FromSource(ModelField.GetValue(Src));
            set => ModelField.SetValue(Src, value.Src);
        }

        private static FastField MatrixField;
        /// <summary>
        /// この 3D モデルを設置するための行列を取得・設定します。
        /// </summary>
        public Matrix Matrix
        {
            get => MatrixField.GetValue(Src);
            set => MatrixField.SetValue(Src, value);
        }

        private static FastField VerticesField;
        /// <summary>
        /// 頂点の位置ベクトルの一覧を取得・設定します。
        /// </summary>
        /// <remarks>
        /// この値は Structure.PutBetween ステートメントによるストラクチャーを描画する際に使用されます。
        /// </remarks>
        public Vector3[] Vertices
        {
            get => VerticesField.GetValue(Src);
            set => VerticesField.SetValue(Src, value);
        }

        private static FastMethod DrawMethod;
        /// <summary>
        /// モデルを描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        /// <param name="baseTransform"><see cref="Matrix"/> に追加で掛ける、ベースの行列。</param>
        /// <param name="skipZWrite">深度バッファーへの書き込みをスキップするか。</param>
        public void Draw(Direct3DProvider direct3DProvider, Matrix baseTransform, bool skipZWrite)
            => DrawMethod.Invoke(Src, new object[] { direct3DProvider?.Src, baseTransform, skipZWrite });
    }
}
