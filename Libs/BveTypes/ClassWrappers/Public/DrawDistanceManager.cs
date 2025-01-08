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
    /// ストラクチャーの描画範囲を算出するための機能を提供します。
    /// </summary>
    public class DrawDistanceManager : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<DrawDistanceManager>();

            DrawDistanceGetMethod = members.GetSourcePropertyGetterOf(nameof(DrawDistance));

            DrawDistanceObjectsGetMethod = members.GetSourcePropertyGetterOf(nameof(DrawDistanceObjects));

            FacingDirectionField = members.GetSourceFieldOf(nameof(FacingDirection));
            FrontDrawDistanceField = members.GetSourceFieldOf(nameof(FrontDrawDistance));
            BackDrawDistanceField = members.GetSourceFieldOf(nameof(BackDrawDistance));

            UpdateEachDirectionDrawDistanceMethod = members.GetSourceMethodOf(nameof(UpdateEachDirectionDrawDistance));

            FacingDirectionChangedEvent = members.GetSourceEventOf(nameof(FacingDirectionChanged));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="DrawDistanceManager"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected DrawDistanceManager(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="DrawDistanceManager"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static DrawDistanceManager FromSource(object src) => src is null ? null : new DrawDistanceManager(src);

        private static FastField FacingDirectionField;
        /// <summary>
        /// カメラの向いている方向を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 前方を向いている場合は 0、後方を向いている場合は 2、その中間付近を向いている場合は 1 となります。
        /// </remarks>
        public int FacingDirection
        {
            get => (int)FacingDirectionField.GetValue(Src);
            set => FacingDirectionField.SetValue(Src, value);
        }

        private static FastField FrontDrawDistanceField;
        /// <summary>
        /// 前方の描画距離 [m] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 前方を向いている場合は <see cref="DrawDistance"/> プロパティの値、後方を向いている場合は必要最低限の距離となります。
        /// </remarks>
        public double FrontDrawDistance
        {
            get => (double)FrontDrawDistanceField.GetValue(Src);
            set => FrontDrawDistanceField.SetValue(Src, value);
        }

        private static FastField BackDrawDistanceField;
        /// <summary>
        /// 後方の描画距離 [m] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 前方を向いている場合は必要最低限の距離、後方を向いている場合は <see cref="DrawDistance"/> プロパティの値となります。
        /// </remarks>
        public double BackDrawDistance
        {
            get => (double)BackDrawDistanceField.GetValue(Src);
            set => BackDrawDistanceField.SetValue(Src, value);
        }

        private static FastMethod DrawDistanceGetMethod;
        /// <summary>
        /// マップファイル内での最長描画距離指定とユーザー設定から算出された描画距離 [m] を取得します。
        /// </summary>
        public double DrawDistance => (double)DrawDistanceGetMethod.Invoke(Src, null);

        private static FastMethod DrawDistanceObjectsGetMethod;
        /// <summary>
        /// マップファイルで DrawDistance.Change ステートメントにより設置された、最長描画距離を指定するためのオブジェクトを取得します。
        /// </summary>
        public MapFunctionList DrawDistanceObjects => MapFunctionList.FromSource(DrawDistanceObjectsGetMethod.Invoke(Src, null));

        private static FastEvent FacingDirectionChangedEvent;
        /// <summary>
        /// カメラの向いている方向が変わり、<see cref="FacingDirection"/> の値が変更されたときに発生します。
        /// </summary>
        public event EventHandler FacingDirectionChanged
        {
            add => FacingDirectionChangedEvent.Add(Src, value);
            remove => FacingDirectionChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="FacingDirectionChanged"/> イベントを実行します。
        /// </summary>
        public void FacingDirectionChanged_Invoke() => FacingDirectionChangedEvent.Invoke(Src, new object[] { (object)Src, EventArgs.Empty });

        private static FastMethod UpdateEachDirectionDrawDistanceMethod;
        /// <summary>
        /// 前後各方向の描画ブロック数を、現在カメラが向いている方向に合わせて設定します。
        /// </summary>
        public void UpdateEachDirectionDrawDistance()
            => UpdateEachDirectionDrawDistanceMethod.Invoke(Src, null);
    }
}
