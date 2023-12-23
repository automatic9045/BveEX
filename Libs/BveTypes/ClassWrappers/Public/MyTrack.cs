using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自軌道を表します。
    /// </summary>
    public class MyTrack : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MyTrack>();

            CurvesGetMethod = members.GetSourcePropertyGetterOf(nameof(Curves));

            CurvePostsGetMethod = members.GetSourcePropertyGetterOf(nameof(CurvePosts));

            CantsGetMethod = members.GetSourcePropertyGetterOf(nameof(Cants));

            GetDirectionAtMethod = members.GetSourceMethodOf(nameof(GetDirectionAt));
            GetPositionMethod = members.GetSourceMethodOf(nameof(GetPosition));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MyTrack"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MyTrack(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MyTrack"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MyTrack FromSource(object src) => src is null ? null : new MyTrack(src);

        private static FastMethod CurvesGetMethod;
        /// <summary>
        /// 緩和曲線および円曲線のリストを取得します。
        /// </summary>
        public CurveList Curves => CurveList.FromSource(CurvesGetMethod.Invoke(Src, null));

        private static FastMethod CurvePostsGetMethod;
        /// <summary>
        /// 円曲線の開始距離程と半径のペアを表す <see cref="ValueNode{T}"/> (T は <see cref="double"/>) のリストを取得します。
        /// </summary>
        public MapObjectList CurvePosts => CurveList.FromSource(CurvePostsGetMethod.Invoke(Src, null));

        private static FastMethod CantsGetMethod;
        /// <summary>
        /// カントのリストを取得します。
        /// </summary>
        public CantList Cants => CantList.FromSource(CantsGetMethod.Invoke(Src, null));

        private static FastMethod GetDirectionAtMethod;
        /// <summary>
        /// 指定した距離程において自軌道が向いている方角を求めます。
        /// </summary>
        /// <param name="location">方角を求める距離程 [m]。</param>
        /// <returns>距離程 <paramref name="location"/> にて自軌道が向いている方角。</returns>
        public double GetDirectionAt(int location) => GetDirectionAtMethod.Invoke(Src, new object[] { location });

        private static FastMethod GetPositionMethod;
        /// <summary>
        /// 指定した距離程を基点とした、目標距離程における自軌道の位置ベクトル [m] の差を求めます。
        /// </summary>
        /// <param name="locationTo">目標距離程 [m]。</param>
        /// <param name="locationFrom">基点とする距離程 [m]。</param>
        /// <returns>距離程 <paramref name="locationFrom"/> を基点とした、距離程 <paramref name="locationTo"/> における自軌道の位置ベクトル。</returns>
        public Vector3 GetPosition(double locationTo, double locationFrom) => GetPositionMethod.Invoke(Src, new object[] { locationTo, locationFrom });
    }
}
