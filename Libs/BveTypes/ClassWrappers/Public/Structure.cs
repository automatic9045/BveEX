using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// Structure[].Put ステートメント、Structure[].Put0 ステートメントで設置されたストラクチャーを表します。
    /// </summary>
    /// <seealso cref="StructureSet"/>
    public class Structure : LocatableMapObject
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Structure>();

            Constructor1 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(int), typeof(double), typeof(Model) });
            Constructor2 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(string), typeof(int), typeof(double), typeof(Model) });

            ModelGetMethod = members.GetSourcePropertyGetterOf(nameof(Model));
            ModelSetMethod = members.GetSourcePropertySetterOf(nameof(Model));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Structure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Structure(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Structure"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Structure FromSource(object src) => src is null ? null : new Structure(src);

        private static FastConstructor Constructor1;
        private Structure(double location, string trackKey, double x, double y, double z, double dx, double dy, double dz, int tilt, double span, Model model)
            : this(Constructor1.Invoke(new object[] { location, trackKey, x, y, z, dx, dy, dz, tilt, span, model.Src }))
        {
        }

        /// <summary>
        /// 距離程、設置先の軌道、軌道からの変位、傾斜オプション、弦の長さ、モデルを指定して <see cref="Structure"/> クラスの新しいインスタンスを初期化します。
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
        /// <param name="model">ストラクチャーの 3D モデルを表す <see cref="ClassWrappers.Model"/>。</param>
        public Structure(double location, string trackKey, double x, double y, double z, double dx, double dy, double dz, TiltOptions tiltOptions, double span, Model model)
            : this(location, trackKey, x, y, z, dx, dy, dz, (int)tiltOptions, span, model)
        {
        }

        private static FastConstructor Constructor2;
        private Structure(double location, string trackKey, int tilt, double span, Model model)
            : this(Constructor2.Invoke(new object[] { location, trackKey, tilt, span, model.Src }))
        {
        }

        /// <summary>
        /// 距離程、設置先の軌道、傾斜オプション、弦の長さ、モデルを指定して <see cref="Structure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="trackKey">設置先の軌道名。</param>
        /// <param name="tiltOptions">傾斜オプション。</param>
        /// <param name="span">曲線における弦の長さ [m]。</param>
        /// <param name="model">ストラクチャーの 3D モデルを表す <see cref="ClassWrappers.Model"/>。</param>
        public Structure(double location, string trackKey, TiltOptions tiltOptions, double span, Model model)
            : this(location, trackKey, (int)tiltOptions, span, model)
        {
        }

        private static FastMethod ModelGetMethod;
        private static FastMethod ModelSetMethod;
        /// <summary>
        /// ストラクチャーの 3D モデルを表す <see cref="ClassWrappers.Model"/> を取得・設定します。
        /// </summary>
        public Model Model
        {
            get => ClassWrappers.Model.FromSource(ModelGetMethod.Invoke(Src, null));
            set => ModelSetMethod.Invoke(Src, new object[] { value.Src });
        }
    }
}
