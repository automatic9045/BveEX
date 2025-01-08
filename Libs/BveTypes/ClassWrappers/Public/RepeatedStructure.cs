using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 沿線ストラクチャーを表します。
    /// </summary>
    public class RepeatedStructure : Structure
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<RepeatedStructure>();

            Constructor1 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(int), typeof(double), typeof(double), typeof(WrappedList<Model>) });
            Constructor2 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(int), typeof(double), typeof(double), typeof(WrappedList<Model>) });

            ModelsGetMethod = members.GetSourcePropertyGetterOf(nameof(Models));
            ModelsSetMethod = members.GetSourcePropertySetterOf(nameof(Models));

            IntervalGetMethod = members.GetSourcePropertyGetterOf(nameof(Interval));
            IntervalSetMethod = members.GetSourcePropertySetterOf(nameof(Interval));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="RepeatedStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected RepeatedStructure(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="RepeatedStructure"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new RepeatedStructure FromSource(object src) => src is null ? null : new RepeatedStructure(src);

        private static FastConstructor Constructor1;
        private RepeatedStructure(double location, string trackKey, double x, double y, double z, double dx, double dy, double dz, int tilt, double span, double interval, WrappedList<Model> models)
            : this(Constructor1.Invoke(new object[] { location, trackKey, x, y, z, dx, dy, dz, tilt, span, interval, models }))
        {
        }

        /// <summary>
        /// 距離程、設置先の軌道、軌道からの変位、傾斜オプション、弦の長さ、設置間隔、モデルを指定して <see cref="RepeatedStructure"/> クラスの新しいインスタンスを初期化します。
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
        /// <param name="interval">設置間隔 [m]。</param>
        /// <param name="models">ストラクチャーの 3D モデルを表す <see cref="Model"/> のリスト。</param>
        public RepeatedStructure(double location, string trackKey, double x, double y, double z, double dx, double dy, double dz, TiltOptions tiltOptions, double span, double interval, WrappedList<Model> models)
            : this(location, trackKey, x, y, z, dx, dy, dz, (int)tiltOptions, span, interval, models)
        {
        }

        private static FastConstructor Constructor2;
        private RepeatedStructure(double location, string trackKey, int tilt, double span, double interval, WrappedList<Model> models)
            : this(Constructor2.Invoke(new object[] { location, trackKey, tilt, span, interval, models }))
        {
        }

        /// <summary>
        /// 距離程、設置先の軌道、傾斜オプション、弦の長さ、設置間隔、モデルを指定して <see cref="RepeatedStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey">設置先の軌道名。</param>
        /// <param name="tiltOptions">傾斜オプション。</param>
        /// <param name="span">曲線における弦の長さ [m]。</param>
        /// <param name="interval">設置間隔 [m]。</param>
        /// <param name="models">ストラクチャーの 3D モデルを表す <see cref="Model"/> のリスト。</param>
        public RepeatedStructure(double location, string trackKey, TiltOptions tiltOptions, double span, double interval, WrappedList<Model> models)
            : this(location, trackKey, (int)tiltOptions, span, interval, models)
        {
        }

        private static FastMethod ModelsGetMethod;
        private static FastMethod ModelsSetMethod;
        /// <summary>
        /// ストラクチャーの 3D モデルを表す <see cref="Model"/> のリストを取得・設定します。
        /// </summary>
        public WrappedList<Model> Models
        {
            get => WrappedList<Model>.FromSource(ModelsGetMethod.Invoke(Src, null) as IList);
            set => ModelsSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod IntervalGetMethod;
        private static FastMethod IntervalSetMethod;
        /// <summary>
        /// 設置間隔 [m] を取得・設定します。
        /// </summary>
        public double Interval
        {
            get => (double)IntervalGetMethod.Invoke(Src, null);
            set => IntervalSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
