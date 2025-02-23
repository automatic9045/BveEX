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
    /// BVE によって読み込まれた全ての 3D モデルを描画するための機能を提供します。
    /// </summary>
    public class ObjectDrawer : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ObjectDrawer>();

            StructureDrawerField = members.GetSourceFieldOf(nameof(StructureDrawer));
            DrawDistanceManagerField = members.GetSourceFieldOf(nameof(DrawDistanceManager));

            BuildMethod = members.GetSourceMethodOf(nameof(Build));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ObjectDrawer"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ObjectDrawer(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ObjectDrawer"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ObjectDrawer FromSource(object src) => src is null ? null : new ObjectDrawer(src);

        private static FastField StructureDrawerField;
        /// <summary>
        /// ストラクチャーを描画するための機能を提供する <see cref="ClassWrappers.StructureDrawer"/> を取得します。
        /// </summary>
        public StructureDrawer StructureDrawer => StructureDrawer.FromSource(StructureDrawerField.GetValue(Src));

        private static FastField DrawDistanceManagerField;
        /// <summary>
        /// ストラクチャーの描画範囲を算出するための機能を提供する <see cref="ClassWrappers.DrawDistanceManager"/> を取得します。
        /// </summary>
        public DrawDistanceManager DrawDistanceManager => DrawDistanceManager.FromSource(DrawDistanceManagerField.GetValue(Src));

        private static FastMethod BuildMethod;
        /// <summary>
        /// 風景を構築します。
        /// </summary>
        /// <param name="map">路線データ。</param>
        public void Build(Map map) => BuildMethod.Invoke(Src, new object[] { map?.Src });

        /// <summary>
        /// 互換性のために残されている古いメソッドです。<see cref="Build(Map)"/> メソッドを使用してください。
        /// </summary>
        /// <param name="map">処理対象となるマップ。</param>
        /// <seealso cref="Build(Map)"/>
        [Obsolete]
        public void SetMap(Map map) => Build(map);

        private static FastMethod DrawMethod;
        /// <summary>
        /// 3D モデル群を描画します。
        /// </summary>
        public void Draw() => DrawMethod.Invoke(Src, null);
    }
}
