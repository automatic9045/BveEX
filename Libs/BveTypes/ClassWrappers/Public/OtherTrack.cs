using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using SlimDX;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 他軌道を表します。
    /// </summary>
    public class OtherTrack : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<OtherTrack>();

            XField = members.GetSourceFieldOf(nameof(X));
            YField = members.GetSourceFieldOf(nameof(Y));
            CantsField = members.GetSourceFieldOf(nameof(Cants));

            GetPositionMethod = members.GetSourceMethodOf(nameof(GetPosition));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="OtherTrack"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected OtherTrack(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="OtherTrack"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static OtherTrack FromSource(object src) => src is null ? null : new OtherTrack(src);

        private static FastField XField;
        /// <summary>
        /// 自軌道に対する X 方向の相対位置を取得・設定します。
        /// </summary>
        public TrackPositionList X
        {
            get => TrackPositionList.FromSource(XField.GetValue(Src));
            set => XField.SetValue(Src, value?.Src);
        }

        private static FastField YField;
        /// <summary>
        /// 自軌道に対する Y 方向の相対位置を取得・設定します。
        /// </summary>
        public TrackPositionList Y
        {
            get => TrackPositionList.FromSource(YField.GetValue(Src));
            set => YField.SetValue(Src, value?.Src);
        }

        private static FastField CantsField;
        /// <summary>
        /// カントの一覧を取得・設定します。
        /// </summary>
        public CantList Cants
        {
            get => CantList.FromSource(CantsField.GetValue(Src));
            set => CantsField.SetValue(Src, value?.Src);
        }

        private static FastMethod GetPositionMethod;
        /// <summary>
        /// 指定した距離程における、自軌道を基準としたこの他軌道の位置ベクトルを計算します。
        /// </summary>
        /// <param name="location">距離程 [m]。</param>
        /// <returns>距離程 <paramref name="location"/> における、この他軌道の位置ベクトル。</returns>
        public Vector3 GetPosition(double location) => (Vector3)GetPositionMethod.Invoke(Src, new object[] { location });
    }
}
