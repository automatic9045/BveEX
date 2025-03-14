using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ストラクチャーを描画するための機能を提供します。
    /// </summary>
    public class StructureDrawer : ClassWrapperBase
    {
        private static BveTypeSet BveTypes;

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            BveTypes = bveTypes;

            ClassMemberSet members = bveTypes.GetClassInfoOf<StructureDrawer>();

            BlocksGetMethod = members.GetSourcePropertyGetterOf(nameof(Blocks));

            MinDrawBlockIndexField = members.GetSourceFieldOf(nameof(MinDrawBlockIndex));
            MaxDrawBlockIndexPlus1Field = members.GetSourceFieldOf(nameof(MaxDrawBlockIndexPlus1));

            OnDrawLocationRangeUpdatedMethod = members.GetSourceMethodOf(nameof(OnDrawLocationRangeUpdated));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            BuildBlocksMethod = members.GetSourceMethodOf(nameof(BuildBlocks));
            BuildBlockMethod = members.GetSourceMethodOf(nameof(BuildBlock));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StructureDrawer"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StructureDrawer(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StructureDrawer"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static StructureDrawer FromSource(object src) => src is null ? null : new StructureDrawer(src);

        private static FastMethod BlocksGetMethod;
        /// <summary>
        /// 設置距離程 25m おきにストラクチャーをグルーピングしたブロックの一覧を取得します。
        /// </summary>
        public WrappedArray<StructureBlock> Blocks
        {
            get
            {
                Array arraySrc = BlocksGetMethod.Invoke(Src, null) as Array;
                return WrappedArray<StructureBlock>.FromSource(arraySrc);
            }
        }

        private static FastField MinDrawBlockIndexField;
        /// <summary>
        /// 描画範囲内にあるブロックのインデックスの下限を取得します。
        /// </summary>
        public int MinDrawBlockIndex
        {
            get => (int)MinDrawBlockIndexField.GetValue(Src);
            set => MinDrawBlockIndexField.SetValue(Src, value);
        }

        private static FastField MaxDrawBlockIndexPlus1Field;
        /// <summary>
        /// 描画範囲内にあるブロックのインデックスの上限に 1 を足した値を取得します。
        /// </summary>
        public int MaxDrawBlockIndexPlus1
        {
            get => (int)MaxDrawBlockIndexPlus1Field.GetValue(Src);
            set => MaxDrawBlockIndexPlus1Field.SetValue(Src, value);
        }

        private static FastMethod OnDrawLocationRangeUpdatedMethod;
        /// <summary>
        /// 描画範囲の距離程の始・終点が変化したときに呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDrawLocationRangeUpdated(object sender, EventArgs e)
            => OnDrawLocationRangeUpdatedMethod.Invoke(Src, new object[] { sender is ClassWrapperBase classWrapper ? classWrapper.Src : sender, e });

        private static FastMethod DrawMethod;
        /// <summary>
        /// 描画範囲内のストラクチャーを描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        /// <param name="view">ビュー変換行列。</param>
        public void Draw(Direct3DProvider direct3DProvider, Matrix view)
            => DrawMethod.Invoke(Src, new object[] { direct3DProvider?.Src, view });

        private static FastMethod BuildBlocksMethod;
        /// <summary>
        /// 読み込んだ路線データを基に、全てのストラクチャーブロックを構築します。
        /// </summary>
        /// <param name="map">路線データ。</param>
        public void BuildBlocks(Map map) => BuildBlocksMethod.Invoke(Src, new object[] { map?.Src });

        private static FastMethod BuildBlockMethod;
        /// <summary>
        /// 指定したインデックスのストラクチャーブロックを構築します。
        /// </summary>
        /// <param name="blockIndex">構築するストラクチャーブロックのインデックス。</param>
        /// <param name="map">路線データ。</param>
        public void BuildBlock(int blockIndex, Map map) => BuildBlockMethod.Invoke(Src, new object[] { blockIndex, map?.Src });
    }
}
