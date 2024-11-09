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
    /// 自列車のブレーキシステム全体を表します。
    /// </summary>
    public class BrakeSystem : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakeSystem>();

            BrakeBlenderGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeBlender));
            AirSupplementGetMethod = members.GetSourcePropertyGetterOf(nameof(AirSupplement));
            LockoutValveGetMethod = members.GetSourcePropertyGetterOf(nameof(LockoutValve));

            BrakeControllerGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeController));
            EcbGetMethod = members.GetSourcePropertyGetterOf(nameof(Ecb));
            SmeeGetMethod = members.GetSourcePropertyGetterOf(nameof(Smee));
            ClGetMethod = members.GetSourcePropertyGetterOf(nameof(Cl));

            MotorCarBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCarBrake));

            TrailerCarBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(TrailerCarBrake));

            FirstCarBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(FirstCarBrake));
            FirstCarBrakeSetMethod = members.GetSourcePropertySetterOf(nameof(FirstCarBrake));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BrakeSystem"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakeSystem(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BrakeSystem"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BrakeSystem FromSource(object src) => src is null ? null : new BrakeSystem(src);

        private static FastMethod BrakeBlenderGetMethod;
        /// <summary>
        /// 自列車が使用する電空協調制御を取得します。
        /// </summary>
        /// <remarks>
        /// 取得される値は、パラメーターファイルでの設定に合わせて <see cref="AirSupplement"/> プロパティ、<see cref="LockoutValve"/> プロパティのどちらかとなります。
        /// </remarks>
        public BrakeBlenderBase BrakeBlender => CreateFromSource(BrakeBlenderGetMethod.Invoke(Src, null)) as BrakeBlenderBase;

        private static FastMethod AirSupplementGetMethod;
        /// <summary>
        /// 遅れ込め制御式電空協調制御を取得します。
        /// </summary>
        public AirSupplement AirSupplement => ClassWrappers.AirSupplement.FromSource(AirSupplementGetMethod.Invoke(Src, null));

        private static FastMethod LockoutValveGetMethod;
        /// <summary>
        /// 締切電磁弁式電空協調制御を取得します。
        /// </summary>
        public LockoutValve LockoutValve => ClassWrappers.LockoutValve.FromSource(LockoutValveGetMethod.Invoke(Src, null));

        private static FastMethod BrakeControllerGetMethod;
        /// <summary>
        /// 自列車が使用するブレーキ方式を取得します。
        /// </summary>
        /// <remarks>
        /// 取得される値は、パラメーターファイルでの設定に合わせて <see cref="Ecb"/> プロパティ、<see cref="Smee"/> プロパティ、<see cref="Cl"/> プロパティのいずれかとなります。
        /// </remarks>
        public BrakeControllerBase BrakeController => CreateFromSource(BrakeControllerGetMethod.Invoke(Src, null)) as BrakeControllerBase;

        private static FastMethod EcbGetMethod;
        /// <summary>
        /// 電気指令式ブレーキを取得します。
        /// </summary>
        public Ecb Ecb => ClassWrappers.Ecb.FromSource(EcbGetMethod.Invoke(Src, null));

        private static FastMethod SmeeGetMethod;
        /// <summary>
        /// 電磁直通空気ブレーキを取得します。
        /// </summary>
        public Smee Smee => ClassWrappers.Smee.FromSource(SmeeGetMethod.Invoke(Src, null));

        private static FastMethod ClGetMethod;
        /// <summary>
        /// 自動空気ブレーキを取得します。
        /// </summary>
        public Cl Cl => ClassWrappers.Cl.FromSource(ClGetMethod.Invoke(Src, null));

        private static FastMethod MotorCarBrakeGetMethod;
        /// <summary>
        /// 動力車のブレーキを表す <see cref="CarBrake"/> を取得します。
        /// </summary>
        public CarBrake MotorCarBrake => CarBrake.FromSource(MotorCarBrakeGetMethod.Invoke(Src, null));

        private static FastMethod TrailerCarBrakeGetMethod;
        /// <summary>
        /// 付随車のブレーキを表す <see cref="CarBrake"/> を取得します。
        /// </summary>
        public CarBrake TrailerCarBrake => CarBrake.FromSource(TrailerCarBrakeGetMethod.Invoke(Src, null));

        private static FastMethod FirstCarBrakeGetMethod;
        private static FastMethod FirstCarBrakeSetMethod;
        /// <summary>
        /// 先頭車両のブレーキを表す <see cref="CarBrake"/> を取得・設定します。
        /// </summary>
        public CarBrake FirstCarBrake
        {
            get => CarBrake.FromSource(FirstCarBrakeGetMethod.Invoke(Src, null));
            set => FirstCarBrakeSetMethod.Invoke(Src, new object[] { value?.Src });
        }
    }
}
