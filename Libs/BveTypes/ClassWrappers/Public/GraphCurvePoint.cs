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
    /// <see cref="GraphCurve"/> で線形補間処理の基となる点を表します。
    /// </summary>
    public class GraphCurvePoint : ClassWrapperBase
    {
        private static Type OriginalType;

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<GraphCurvePoint>();

            OriginalType = members.OriginalType;

            XGetMethod = members.GetSourcePropertyGetterOf(nameof(X));

            YGetMethod = members.GetSourcePropertyGetterOf(nameof(Y));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="GraphCurvePoint"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected GraphCurvePoint(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="GraphCurvePoint"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static GraphCurvePoint FromSource(object src) => src is null ? null : new GraphCurvePoint(src);

        /// <summary>
        /// 座標を指定して、<see cref="GraphCurvePoint"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="x">x 座標。</param>
        /// <param name="y">y 座標。</param>
        public GraphCurvePoint(double x, double y) : this(Activator.CreateInstance(OriginalType, x, y))
        {
        }

        private static FastMethod XGetMethod;
        /// <summary>
        /// x 座標を取得します。
        /// </summary>
        public double X => (double)XGetMethod.Invoke(Src, null);

        private static FastMethod YGetMethod;
        /// <summary>
        /// y 座標を取得します。
        /// </summary>
        public double Y => (double)YGetMethod.Invoke(Src, null);
    }
}
