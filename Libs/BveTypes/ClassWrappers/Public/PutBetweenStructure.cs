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
    /// Structure[].PutBetween ステートメントで設置されたストラクチャーを表します。
    /// </summary>
    /// <seealso cref="StructureSet"/>
    public class PutBetweenStructure : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<PutBetweenStructure>();

            Constructor = members.GetSourceConstructor();

            ModelGetMethod = members.GetSourcePropertyGetterOf(nameof(Model));
            ModelSetMethod = members.GetSourcePropertySetterOf(nameof(Model));

            TrackKey1GetMethod = members.GetSourcePropertyGetterOf(nameof(TrackKey1));
            TrackKey1SetMethod = members.GetSourcePropertySetterOf(nameof(TrackKey1));

            TrackKey2GetMethod = members.GetSourcePropertyGetterOf(nameof(TrackKey2));
            TrackKey2SetMethod = members.GetSourcePropertySetterOf(nameof(TrackKey2));

            TransformOnlyXGetMethod = members.GetSourcePropertyGetterOf(nameof(TransformOnlyX));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="PutBetweenStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected PutBetweenStructure(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="PutBetweenStructure"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static PutBetweenStructure FromSource(object src) => src is null ? null : new PutBetweenStructure(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// 距離程、設置位置の計算の基となる 2 軌道、モデル、変形方向を指定して <see cref="PutBetweenStructure"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey1">一方の軌道の軌道名。</param>
        /// <param name="trackKey2">他方の軌道の軌道名。</param>
        /// <param name="model">ストラクチャーの 3D モデルを表す <see cref="ClassWrappers.Model"/>。</param>
        /// <param name="transformOnlyX">X 方向のみに変形するか。</param>
        public PutBetweenStructure(double location, string trackKey1, string trackKey2, Model model, bool transformOnlyX)
            : this(Constructor.Invoke(new object[] { location, trackKey1, trackKey2, model, transformOnlyX }))
        {
        }

        private static FastMethod ModelGetMethod;
        private static FastMethod ModelSetMethod;
        /// <summary>
        /// ストラクチャーの 3D モデルを表す <see cref="ClassWrappers.Model"/> を取得・設定します。
        /// </summary>
        public Model Model
        {
            get => Model.FromSource(ModelGetMethod.Invoke(Src, null));
            set => ModelSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod TrackKey1GetMethod;
        private static FastMethod TrackKey1SetMethod;
        /// <summary>
        /// 一方の軌道の軌道名を取得・設定します。
        /// </summary>
        public string TrackKey1
        {
            get => TrackKey1GetMethod.Invoke(Src, null) as string;
            set => TrackKey1SetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TrackKey2GetMethod;
        private static FastMethod TrackKey2SetMethod;
        /// <summary>
        /// 他方の軌道の軌道名を取得・設定します。
        /// </summary>
        public string TrackKey2
        {
            get => TrackKey2GetMethod.Invoke(Src, null) as string;
            set => TrackKey2SetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TransformOnlyXGetMethod;
        /// <summary>
        /// X 方向のみに変形するかを取得・設定します。
        /// </summary>
        public bool TransformOnlyX => (bool)TransformOnlyXGetMethod.Invoke(Src, null);
    }
}
