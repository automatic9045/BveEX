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
    /// 自軌道のカーブを表します。
    /// </summary>
    /// <remarks>
    /// 緩和曲線、円曲線のどちらもこのクラスで定義します。緩和曲線の場合は 1m ごとに曲率が変化するため、曲率の変化する 1m ごとに定義します。
    /// </remarks>
    public class Curve : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Curve>();

            CurvatureGetMethod = members.GetSourcePropertyGetterOf(nameof(Curvature));
            CurvatureSetMethod = members.GetSourcePropertySetterOf(nameof(Curvature));

            DirectionGetMethod = members.GetSourcePropertyGetterOf(nameof(Direction));
            DirectionSetMethod = members.GetSourcePropertySetterOf(nameof(Direction));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Curve"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Curve(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Curve"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Curve FromSource(object src) => src is null ? null : new Curve(src);

        private static FastMethod CurvatureGetMethod;
        private static FastMethod CurvatureSetMethod;
        /// <summary>
        /// このカーブの曲率 [/m]、すなわち半径 [m] の逆数を取得・設定します。
        /// </summary>
        public double Curvature
        {
            get => (double)CurvatureGetMethod.Invoke(Src, null);
            set => CurvatureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DirectionGetMethod;
        private static FastMethod DirectionSetMethod;
        /// <summary>
        /// このカーブの始点における方角を取得・設定します。
        /// </summary>
        public double Direction
        {
            get => (double)DirectionGetMethod.Invoke(Src, null);
            set => DirectionSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
