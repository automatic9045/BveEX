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
    public class VehicleInstrumentSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleInstrumentSet>();

            ConstantSpeedRelayGetMethod = members.GetSourcePropertyGetterOf(nameof(ConstantSpeedRelay));

            BrakeSystemGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeSystem));

            ElectricityGetMethod = members.GetSourcePropertyGetterOf(nameof(Electricity));

            CabGetMethod = members.GetSourcePropertyGetterOf(nameof(Cab));
            CabSetMethod = members.GetSourcePropertySetterOf(nameof(Cab));

            AtsPluginGetMethod = members.GetSourcePropertyGetterOf(nameof(AtsPlugin));
            AtsPluginSetMethod = members.GetSourcePropertySetterOf(nameof(AtsPlugin));
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

        private static FastMethod ConstantSpeedRelayGetMethod;
        /// <summary>
        /// 定速制御を行う <see cref="ClassWrappers.ConstantSpeedRelay"/> を取得します。
        /// </summary>
        public ConstantSpeedRelay ConstantSpeedRelay => ClassWrappers.ConstantSpeedRelay.FromSource(ConstantSpeedRelayGetMethod.Invoke(Src, null));

        private static FastMethod BrakeSystemGetMethod;
        /// <summary>
        /// ブレーキシステム全体を表す <see cref="ClassWrappers.BrakeSystem"/> を取得します。
        /// </summary>
        public BrakeSystem BrakeSystem => ClassWrappers.BrakeSystem.FromSource(BrakeSystemGetMethod.Invoke(Src, null));

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
            get => (CabBase)CreateFromSource(CabGetMethod.Invoke(Src, null));
            set => CabSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod AtsPluginGetMethod;
        private static FastMethod AtsPluginSetMethod;
        /// <summary>
        /// ATS プラグインを取得・設定します。
        /// </summary>
        public AtsPlugin AtsPlugin
        {
            get => ClassWrappers.AtsPlugin.FromSource(AtsPluginGetMethod.Invoke(Src, null));
            set => AtsPluginSetMethod.Invoke(Src, new object[] { value?.Src });
        }
    }
}
