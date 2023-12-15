using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SlimDX.Direct3D9;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// Direct3D9 から表示可能な 3D モデルを表します。
    /// </summary>
    public class Model : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Model>();

            FromXFileMethod = members.GetSourceMethodOf(nameof(FromXFile));

            MeshGetMethod = members.GetSourcePropertyGetterOf(nameof(Mesh));
            MeshSetMethod = members.GetSourcePropertySetterOf(nameof(Mesh));

            MaterialsGetMethod = members.GetSourcePropertyGetterOf(nameof(Materials));
            MaterialsSetMethod = members.GetSourcePropertySetterOf(nameof(Materials));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Model"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Model(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Model"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Model FromSource(object src) => src is null ? null : new Model(src);

        private static FastMethod FromXFileMethod;
        /// <summary>
        /// 指定した X ファイルからモデルを読み込みます。
        /// </summary>
        /// <param name="filePath">X ファイルのパス。</param>
        /// <returns>読み込まれたモデルを表す <see cref="Model"/>。</returns>
        public static Model FromXFile(string filePath)
            => FromSource(FromXFileMethod.Invoke(null, new object[] { filePath }));

        private static FastMethod MeshGetMethod;
        private static FastMethod MeshSetMethod;
        /// <summary>
        /// モデルを構成する <see cref="Mesh"/> を取得・設定します。
        /// </summary>
        public Mesh Mesh
        {
            get => MeshGetMethod.Invoke(Src, null);
            set => MeshSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MaterialsGetMethod;
        private static FastMethod MaterialsSetMethod;
        private static readonly Func<object, MaterialInfo[]> MaterialsParserToWrapper = src =>
        {
            Array srcArray = src as Array;
            MaterialInfo[] result = new MaterialInfo[srcArray.Length];
            for (int i = 0; i < srcArray.Length; i++)
            {
                object srcArrayItem = srcArray.GetValue(i);
                result[i] = srcArrayItem is null ? null : MaterialInfo.FromSource(srcArrayItem);
            }

            return result;
        };
        private static readonly Func<MaterialInfo[], object> MaterialsParserToSource = wrapper =>
        {
            object[] result = new object[wrapper.Length];
            for (int i = 0; i < wrapper.Length; i++)
            {
                result[i] = wrapper[i]?.Src;
            }

            return result;
        };
        /// <summary>
        /// モデルのマテリアル情報の一覧を取得・設定します。
        /// </summary>
        public MaterialInfo[] Materials
        {
            get => MaterialsParserToWrapper(MaterialsGetMethod.Invoke(Src, null));
            set => MaterialsSetMethod.Invoke(Src, new object[] { MaterialsParserToSource(value) });
        }

        private static FastMethod DrawMethod;
        /// <summary>
        /// モデルを描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        /// <param name="skipZWrite">深度バッファーへの書き込みをスキップするか。</param>
        public void Draw(Direct3DProvider direct3DProvider, bool skipZWrite)
            => DrawMethod.Invoke(Src, new object[] { direct3DProvider.Src, skipZWrite });
    }
}
