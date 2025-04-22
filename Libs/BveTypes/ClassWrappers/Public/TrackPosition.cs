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
    /// 他軌道の位置を表します。
    /// </summary>
    public class TrackPosition : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TrackPosition>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(double), typeof(double), typeof(double) });

            DisplacementGetMethod = members.GetSourcePropertyGetterOf(nameof(Displacement));
            DisplacementSetMethod = members.GetSourcePropertySetterOf(nameof(Displacement));

            RadiusGetMethod = members.GetSourcePropertyGetterOf(nameof(Radius));
            RadiusSetMethod = members.GetSourcePropertySetterOf(nameof(Radius));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TrackPosition"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TrackPosition(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TrackPosition"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static TrackPosition FromSource(object src) => src is null ? null : new TrackPosition(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="TrackPosition"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="displacement">自軌道からの変位 [m]。</param>
        /// <param name="radius">自軌道に対する曲線相対半径 [m]。</param>
        public TrackPosition(double location, double displacement, double radius)
            : this(Constructor.Invoke(new object[] { location, displacement, radius }))
        {
        }

        private static FastMethod DisplacementGetMethod;
        private static FastMethod DisplacementSetMethod;
        /// <summary>
        /// 自軌道からの変位 [m] を取得・設定します。
        /// </summary>
        public double Displacement
        {
            get => (double)DisplacementGetMethod.Invoke(Src, null);
            set => DisplacementSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RadiusGetMethod;
        private static FastMethod RadiusSetMethod;
        /// <summary>
        /// 自軌道に対する曲線相対半径 [m] を取得・設定します。
        /// </summary>
        public double Radius
        {
            get => (double)RadiusGetMethod.Invoke(Src, null);
            set => RadiusSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
