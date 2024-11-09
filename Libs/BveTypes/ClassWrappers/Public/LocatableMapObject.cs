using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 基準となる距離程・軌道からの変位を指定するマップ オブジェクトを表します。
    /// </summary>
    /// <seealso cref="Structure"/>
    public class LocatableMapObject : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<LocatableMapObject>();

            Constructor1 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(int), typeof(double) });
            Constructor2 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(int), typeof(double) });

            MatrixGetMethod = members.GetSourcePropertyGetterOf(nameof(Matrix));
            MatrixSetMethod = members.GetSourcePropertySetterOf(nameof(Matrix));

            TrackKeyGetMethod = members.GetSourcePropertyGetterOf(nameof(TrackKey));
            TrackKeySetMethod = members.GetSourcePropertySetterOf(nameof(TrackKey));

            SpanGetMethod = members.GetSourcePropertyGetterOf(nameof(Span));
            SpanSetMethod = members.GetSourcePropertySetterOf(nameof(Span));

            TiltsAlongCantGetMethod = members.GetSourcePropertyGetterOf(nameof(TiltsAlongCant));
            TiltsAlongCantSetMethod = members.GetSourcePropertySetterOf(nameof(TiltsAlongCant));

            TiltsAlongGradientGetMethod = members.GetSourcePropertyGetterOf(nameof(TiltsAlongGradient));
            TiltsAlongGradientSetMethod = members.GetSourcePropertySetterOf(nameof(TiltsAlongGradient));
        }

        /// <summary>
        /// <see cref="LocatableMapObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected LocatableMapObject(object src) : base(src)
        {
        }

        private static FastConstructor Constructor1;
        private LocatableMapObject(double location, string trackKey, double x, double y, double z, double dx, double dy, double dz, int tilt, double span)
            : this(Constructor1.Invoke(new object[] { location, trackKey, x, y, z, dx, dy, dz, tilt, span }))
        {
        }

        /// <summary>
        /// 距離程、設置先の軌道、軌道からの変位、傾斜オプション、弦の長さを指定して <see cref="LocatableMapObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey">設置先の軌道名。</param>
        /// <param name="x">軌道からの x 座標 [m]。</param>
        /// <param name="y">軌道からの y 座標 [m]。</param>
        /// <param name="z">軌道からの z 座標 [m]。</param>
        /// <param name="dx">軌道に対する x 軸回りの角 [deg]。</param>
        /// <param name="dy">軌道に対する y 軸回りの角 [deg]。</param>
        /// <param name="dz">軌道に対する z 軸回りの角 [deg]。</param>
        /// <param name="tiltOptions">傾斜オプション。</param>
        /// <param name="span">曲線における弦の長さ [m]。</param>
        protected LocatableMapObject(double location, string trackKey, double x, double y, double z, double dx, double dy, double dz, TiltOptions tiltOptions, double span)
            : this(location, trackKey, x, y, z, dx, dy, dz, (int)tiltOptions, span)
        {
        }

        private static FastConstructor Constructor2;
        private LocatableMapObject(double location, string trackKey, int tilt, double span)
            : this(Constructor2.Invoke(new object[] { location, trackKey, tilt, span }))
        {
        }

        /// <summary>
        /// 距離程、設置先の軌道、傾斜オプション、弦の長さを指定して <see cref="LocatableMapObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey">設置先の軌道名。</param>
        /// <param name="tiltOptions">傾斜オプション。</param>
        /// <param name="span">曲線における弦の長さ [m]。</param>
        protected LocatableMapObject(double location, string trackKey, TiltOptions tiltOptions, double span)
            : this(location, trackKey, (int)tiltOptions, span)
        {
        }

        private static FastMethod MatrixGetMethod;
        private static FastMethod MatrixSetMethod;
        /// <summary>
        /// 軌道からの変位を表す <see cref="SlimDX.Matrix"/> を取得・設定します。<see cref="SlimDX.Matrix"/> の単位は [m] です。
        /// </summary>
        public Matrix Matrix
        {
            get => (Matrix)MatrixGetMethod.Invoke(Src, null);
            set => MatrixSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TrackKeyGetMethod;
        private static FastMethod TrackKeySetMethod;
        /// <summary>
        /// 設置先の軌道名を取得・設定します。
        /// </summary>
        public string TrackKey
        {
            get => TrackKeyGetMethod.Invoke(Src, null) as string;
            set => TrackKeySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SpanGetMethod;
        private static FastMethod SpanSetMethod;
        /// <summary>
        /// 曲線における弦の長さ [m] を取得・設定します。
        /// </summary>
        public double Span
        {
            get => (double)SpanGetMethod.Invoke(Src, null);
            set => SpanSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TiltsAlongCantGetMethod;
        private static FastMethod TiltsAlongCantSetMethod;
        /// <summary>
        /// カントに連動して傾斜させるかどうかを取得・設定します。
        /// </summary>
        public bool TiltsAlongCant
        {
            get => (bool)TiltsAlongCantGetMethod.Invoke(Src, null);
            set => TiltsAlongCantSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TiltsAlongGradientGetMethod;
        private static FastMethod TiltsAlongGradientSetMethod;
        /// <summary>
        /// 勾配に連動して傾斜させるかどうかを取得・設定します。
        /// </summary>
        public bool TiltsAlongGradient
        {
            get => (bool)TiltsAlongGradientGetMethod.Invoke(Src, null);
            set => TiltsAlongGradientSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// マップ オブジェクトを傾斜させる方法を取得・設定します。
        /// </summary>
        public TiltOptions TiltOptions
        {
            get
            {
                TiltOptions result = TiltOptions.Default;
                if (TiltsAlongGradient) result |= TiltOptions.TiltsAlongGradient;
                if (TiltsAlongCant) result |= TiltOptions.TiltsAlongCant;

                return result;
            }
            set
            {
                TiltsAlongGradient = TiltOptions.HasFlag(TiltOptions.TiltsAlongGradient);
                TiltsAlongCant = TiltOptions.HasFlag(TiltOptions.TiltsAlongCant);
            }
        }
    }
}
