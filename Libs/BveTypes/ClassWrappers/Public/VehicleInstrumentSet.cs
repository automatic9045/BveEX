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
    /// 自列車を構成する機器のセットを表します。
    /// </summary>
    public class VehicleInstrumentSet : ClassWrapperBase, ITickable, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleInstrumentSet>();

            Constructor = members.GetSourceConstructor();

            ConstantSpeedRelayGetMethod = members.GetSourcePropertyGetterOf(nameof(ConstantSpeedRelay));

            BrakeSystemGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeSystem));

            ElectricityGetMethod = members.GetSourcePropertyGetterOf(nameof(Electricity));

            CabGetMethod = members.GetSourcePropertyGetterOf(nameof(Cab));
            CabSetMethod = members.GetSourcePropertySetterOf(nameof(Cab));

            AtsPluginGetMethod = members.GetSourcePropertyGetterOf(nameof(AtsPlugin));
            AtsPluginSetMethod = members.GetSourcePropertySetterOf(nameof(AtsPlugin));

            LoadCompensatorGetMethod = members.GetSourcePropertyGetterOf(nameof(LoadCompensator));

            InitializeMethod1 = members.GetSourceMethodOf(nameof(Initialize), new Type[0]);
            InitializeMethod2 = members.GetSourceMethodOf(nameof(Initialize), new Type[] { typeof(BrakePosition) });
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleInstrumentSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleInstrumentSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleInstrumentSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleInstrumentSet FromSource(object src) => src is null ? null : new VehicleInstrumentSet(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="VehicleInstrumentSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="handles">ハンドル入力。</param>
        /// <param name="inertiaRatio">空車時に対する自列車全体の慣性比。</param>
        /// <param name="motorCarTachogenerator">動力車の車輪の運動を測定する速度発電機。</param>
        /// <param name="trailerCarTachogenerator">付随車の車輪の運動を測定する速度発電機。</param>
        public VehicleInstrumentSet(HandleSet handles, ValueContainer inertiaRatio, Tachogenerator motorCarTachogenerator, Tachogenerator trailerCarTachogenerator)
            : this(Constructor.Invoke(new object[] { handles?.Src, inertiaRatio?.Src, motorCarTachogenerator?.Src, trailerCarTachogenerator?.Src }))
        {

        }

        private static FastMethod ConstantSpeedRelayGetMethod;
        /// <summary>
        /// 定速制御を行う <see cref="ClassWrappers.ConstantSpeedRelay"/> を取得します。
        /// </summary>
        public ConstantSpeedRelay ConstantSpeedRelay => ConstantSpeedRelay.FromSource(ConstantSpeedRelayGetMethod.Invoke(Src, null));

        private static FastMethod BrakeSystemGetMethod;
        /// <summary>
        /// ブレーキシステム全体を表す <see cref="ClassWrappers.BrakeSystem"/> を取得します。
        /// </summary>
        public BrakeSystem BrakeSystem => BrakeSystem.FromSource(BrakeSystemGetMethod.Invoke(Src, null));

        private static FastMethod ElectricityGetMethod;
        /// <summary>
        /// 電気の系全体を表す <see cref="VehicleElectricity"/> を取得します。
        /// </summary>
        public VehicleElectricity Electricity => VehicleElectricity.FromSource(ElectricityGetMethod.Invoke(Src, null));

        private static FastMethod CabGetMethod;
        private static FastMethod CabSetMethod;
        /// <summary>
        /// 運転台のハンドルを取得・設定します。
        /// </summary>
        public CabBase Cab
        {
            get => CreateFromSource(CabGetMethod.Invoke(Src, null)) as CabBase;
            set => CabSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod AtsPluginGetMethod;
        private static FastMethod AtsPluginSetMethod;
        /// <summary>
        /// ATS プラグインを取得・設定します。
        /// </summary>
        public AtsPlugin AtsPlugin
        {
            get => AtsPlugin.FromSource(AtsPluginGetMethod.Invoke(Src, null));
            set => AtsPluginSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod LoadCompensatorGetMethod;
        /// <summary>
        /// 応荷重制御装置を取得します。
        /// </summary>
        public LoadCompensator LoadCompensator => LoadCompensator.FromSource(LoadCompensatorGetMethod.Invoke(Src, null));

        private static FastMethod InitializeMethod1;
        /// <inheritdoc/>
        public void Initialize() => InitializeMethod1.Invoke(Src, null);

        private static FastMethod InitializeMethod2;
        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="brakePosition">ブレーキハンドルの位置。</param>
        public void Initialize(BrakePosition brakePosition) => InitializeMethod2.Invoke(Src, new object[] { brakePosition });

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドはオリジナルではないため、<see cref="ClassMemberSet.GetSourceMethodOf(string, Type[])"/> メソッドから参照することはできません。<br/>
        /// このメソッドのオリジナルバージョンは <see cref="Tick(double)"/> です。
        /// </remarks>
        /// <param name="elapsed">前フレームからの経過時間。</param>
        /// <seealso cref="Tick(double)"/>
        public void Tick(TimeSpan elapsed) => Tick(elapsed.TotalSeconds);

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
