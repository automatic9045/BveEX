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
    public class BrakeSystem : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakeSystem>();

            TimeDelayedHandlesGetMethod = members.GetSourcePropertyGetterOf(nameof(TimeDelayedHandles));

            BrakeBlenderGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeBlender));
            BrakeBlenderSetMethod = members.GetSourcePropertySetterOf(nameof(BrakeBlender));

            AirSupplementGetMethod = members.GetSourcePropertyGetterOf(nameof(AirSupplement));

            LockoutValveGetMethod = members.GetSourcePropertyGetterOf(nameof(LockoutValve));

            BrakeControllerGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeController));
            BrakeControllerSetMethod = members.GetSourcePropertySetterOf(nameof(BrakeController));

            EcbGetMethod = members.GetSourcePropertyGetterOf(nameof(Ecb));

            SmeeGetMethod = members.GetSourcePropertyGetterOf(nameof(Smee));

            ClGetMethod = members.GetSourcePropertyGetterOf(nameof(Cl));

            MotorCarBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCarBrake));

            TrailerCarBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(TrailerCarBrake));

            FirstCarBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(FirstCarBrake));
            FirstCarBrakeSetMethod = members.GetSourcePropertySetterOf(nameof(FirstCarBrake));

            CompressorGetMethod = members.GetSourcePropertyGetterOf(nameof(Compressor));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
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

        private static FastMethod TimeDelayedHandlesGetMethod;
        /// <summary>
        /// 出力を遅延させたハンドルのセットを取得します。
        /// </summary>
        public CscFilteredTimeDelayedHandleSet TimeDelayedHandles => CscFilteredTimeDelayedHandleSet.FromSource(TimeDelayedHandlesGetMethod.Invoke(Src, null));

        private static FastMethod BrakeBlenderGetMethod;
        private static FastMethod BrakeBlenderSetMethod;
        /// <summary>
        /// 自列車が使用する電空協調制御を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 取得される値は、パラメーターファイルでの設定に合わせて <see cref="AirSupplement"/> プロパティ、<see cref="LockoutValve"/> プロパティのどちらかとなります。
        /// </remarks>
        public BrakeBlenderBase BrakeBlender
        {
            get => CreateFromSource(BrakeBlenderGetMethod.Invoke(Src, null)) as BrakeBlenderBase;
            set => BrakeBlenderSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod AirSupplementGetMethod;
        /// <summary>
        /// 遅れ込め制御式電空協調制御を取得します。
        /// </summary>
        public AirSupplement AirSupplement => AirSupplement.FromSource(AirSupplementGetMethod.Invoke(Src, null));

        private static FastMethod LockoutValveGetMethod;
        /// <summary>
        /// 締切電磁弁式電空協調制御を取得します。
        /// </summary>
        public LockoutValve LockoutValve => LockoutValve.FromSource(LockoutValveGetMethod.Invoke(Src, null));

        private static FastMethod BrakeControllerGetMethod;
        private static FastMethod BrakeControllerSetMethod;
        /// <summary>
        /// 自列車が使用するブレーキ方式を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 取得される値は、パラメーターファイルでの設定に合わせて <see cref="Ecb"/> プロパティ、<see cref="Smee"/> プロパティ、<see cref="Cl"/> プロパティのいずれかとなります。
        /// </remarks>
        public BrakeControllerBase BrakeController
        {
            get => CreateFromSource(BrakeControllerGetMethod.Invoke(Src, null)) as BrakeControllerBase;
            set => BrakeControllerSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod EcbGetMethod;
        /// <summary>
        /// 電気指令式ブレーキを取得します。
        /// </summary>
        public Ecb Ecb => Ecb.FromSource(EcbGetMethod.Invoke(Src, null));

        private static FastMethod SmeeGetMethod;
        /// <summary>
        /// 電磁直通空気ブレーキを取得します。
        /// </summary>
        public Smee Smee => Smee.FromSource(SmeeGetMethod.Invoke(Src, null));

        private static FastMethod ClGetMethod;
        /// <summary>
        /// 自動空気ブレーキを取得します。
        /// </summary>
        public Cl Cl => Cl.FromSource(ClGetMethod.Invoke(Src, null));

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

        private static FastMethod CompressorGetMethod;
        /// <summary>
        /// 空気圧縮機を取得します。
        /// </summary>
        public Compressor Compressor => Compressor.FromSource(CompressorGetMethod.Invoke(Src, null));

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize() => InitializeMethod.Invoke(Src, null);

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
    }
}
