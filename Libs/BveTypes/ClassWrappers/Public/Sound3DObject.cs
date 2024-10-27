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
    /// マップ上に設置可能な固定音源を表します。
    /// </summary>
    public class Sound3DObject : LocatableMapObject
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Sound3DObject>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(double), typeof(double), typeof(int), typeof(Sound) });

            SourceGetMethod = members.GetSourcePropertyGetterOf(nameof(Source));
            SourceSetMethod = members.GetSourcePropertySetterOf(nameof(Source));

            DistanceToSourceFrontGetMethod = members.GetSourcePropertyGetterOf(nameof(DistanceToSourceFront));
            DistanceToSourceFrontSetMethod = members.GetSourcePropertySetterOf(nameof(DistanceToSourceFront));

            FunctionGetMethod = members.GetSourcePropertyGetterOf(nameof(Function));
            FunctionSetMethod = members.GetSourcePropertySetterOf(nameof(Function));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Sound3DObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Sound3DObject(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Sound3DObject"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Sound3DObject FromSource(object src) => src is null ? null : new Sound3DObject(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="Sound3DObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey">設置先の軌道名。</param>
        /// <param name="x">軌道からの x 座標 [m]。</param>
        /// <param name="y">軌道からの y 座標 [m]。</param>
        /// <param name="tiltOptions">傾斜オプション。</param>
        /// <param name="source">再生するサウンド。</param>
        public Sound3DObject(double location, string trackKey, double x, double y, int tiltOptions, Sound source)
            : this(Constructor.Invoke(new object[] { location, trackKey, x, y, tiltOptions, source }))
        {
        }

        /// <summary>
        /// <see cref="Sound3DObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey">設置先の軌道名。</param>
        /// <param name="x">軌道からの x 座標 [m]。</param>
        /// <param name="y">軌道からの y 座標 [m]。</param>
        /// <param name="tiltOptions">傾斜オプション。</param>
        /// <param name="source">再生するサウンド。</param>
        public Sound3DObject(double location, string trackKey, double x, double y, TiltOptions tiltOptions, Sound source)
            : this(location, trackKey, x, y, (int)tiltOptions, source)
        {
        }

        private static FastMethod SourceGetMethod;
        private static FastMethod SourceSetMethod;
        /// <summary>
        /// 再生するサウンドを取得・設定します。
        /// </summary>
        public Sound Source
        {
            get => Sound.FromSource(SourceGetMethod.Invoke(Src, null));
            set => SourceSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod DistanceToSourceFrontGetMethod;
        private static FastMethod DistanceToSourceFrontSetMethod;
        /// <summary>
        /// 他列車原点から音源域の前側までの距離 [m] を取得・設定します。
        /// </summary>
        public double DistanceToSourceFront
        {
            get => DistanceToSourceFrontGetMethod.Invoke(Src, null);
            set => DistanceToSourceFrontSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod FunctionGetMethod;
        private static FastMethod FunctionSetMethod;
        /// <summary>
        /// サウンドの種別を取得・設定します。
        /// </summary>
        public SoundFunction Function
        {
            get => FunctionGetMethod.Invoke(Src, null);
            set => FunctionSetMethod.Invoke(Src, new object[] { value });
        }


        /// <summary>
        /// サウンドの種別を指定します。
        /// </summary>
        public enum SoundFunction
        {
            /// <summary>
            /// 定常音。
            /// </summary>
            Stationary,

            /// <summary>
            /// 転動音。
            /// </summary>
            Rolling,

            /// <summary>
            /// 加速音。
            /// </summary>
            Acceleration,

            /// <summary>
            /// 減速音。
            /// </summary>
            Deceleration,
        }
    }
}
