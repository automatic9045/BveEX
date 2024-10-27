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
    /// 自列車の力学系全体を表します。
    /// </summary>
    public class VehicleDynamics : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleDynamics>();

            CurveResistanceFactorGetMethod = members.GetSourcePropertyGetterOf(nameof(CurveResistanceFactor));
            CurveResistanceFactorSetMethod = members.GetSourcePropertySetterOf(nameof(CurveResistanceFactor));

            RunningResistanceFactorAGetMethod = members.GetSourcePropertyGetterOf(nameof(RunningResistanceFactorA));
            RunningResistanceFactorASetMethod = members.GetSourcePropertySetterOf(nameof(RunningResistanceFactorA));

            RunningResistanceFactorBGetMethod = members.GetSourcePropertyGetterOf(nameof(RunningResistanceFactorB));
            RunningResistanceFactorBSetMethod = members.GetSourcePropertySetterOf(nameof(RunningResistanceFactorB));

            RunningResistanceFactorCGetMethod = members.GetSourcePropertyGetterOf(nameof(RunningResistanceFactorC));
            RunningResistanceFactorCSetMethod = members.GetSourcePropertySetterOf(nameof(RunningResistanceFactorC));

            CarLengthGetMethod = members.GetSourcePropertyGetterOf(nameof(CarLength));
            CarLengthSetMethod = members.GetSourcePropertySetterOf(nameof(CarLength));

            TrailerCarGetMethod = members.GetSourcePropertyGetterOf(nameof(TrailerCar));

            MotorCarGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCar));

            FirstCarGetMethod = members.GetSourcePropertyGetterOf(nameof(FirstCar));
            FirstCarSetMethod = members.GetSourcePropertySetterOf(nameof(FirstCar));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleDynamics"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleDynamics(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleDynamics"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleDynamics FromSource(object src) => src is null ? null : new VehicleDynamics(src);

        private static FastMethod CurveResistanceFactorGetMethod;
        private static FastMethod CurveResistanceFactorSetMethod;
        /// <summary>
        /// 曲線抵抗の係数を取得・設定します。
        /// </summary>
        public double CurveResistanceFactor
        {
            get => CurveResistanceFactorGetMethod.Invoke(Src, null);
            set => CurveResistanceFactorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RunningResistanceFactorAGetMethod;
        private static FastMethod RunningResistanceFactorASetMethod;
        /// <summary>
        /// 速度の単位を [m/s] としたときの走行抵抗の係数 a を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 車両パラメーターファイルで定義する係数は速度の単位を [km/h] としたときのもののため、
        /// ここで取得・設定する値はその 3.6 ^ 2 = 12.96 倍となります。
        /// </remarks>
        public double RunningResistanceFactorA
        {
            get => RunningResistanceFactorAGetMethod.Invoke(Src, null);
            set => RunningResistanceFactorASetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RunningResistanceFactorBGetMethod;
        private static FastMethod RunningResistanceFactorBSetMethod;
        /// <summary>
        /// 速度の単位を [m/s] としたときの走行抵抗の係数 b を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 車両パラメーターファイルで定義する係数は速度の単位を [km/h] としたときのもののため、
        /// ここで取得・設定する値はその 3.6 倍となります。
        /// </remarks>
        public double RunningResistanceFactorB
        {
            get => RunningResistanceFactorBGetMethod.Invoke(Src, null);
            set => RunningResistanceFactorBSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RunningResistanceFactorCGetMethod;
        private static FastMethod RunningResistanceFactorCSetMethod;
        /// <summary>
        /// 走行抵抗の係数 c を取得・設定します。
        /// </summary>
        public double RunningResistanceFactorC
        {
            get => RunningResistanceFactorCGetMethod.Invoke(Src, null);
            set => RunningResistanceFactorCSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CarLengthGetMethod;
        private static FastMethod CarLengthSetMethod;
        /// <summary>
        /// 1 両当たりの長さ [m] を取得・設定します。
        /// </summary>
        public double CarLength
        {
            get => CarLengthGetMethod.Invoke(Src, null);
            set => CarLengthSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TrailerCarGetMethod;
        /// <summary>
        /// 付随車の情報を提供する <see cref="CarInfo"/> を取得します。
        /// </summary>
        public CarInfo TrailerCar => CarInfo.FromSource(TrailerCarGetMethod.Invoke(Src, null));

        private static FastMethod MotorCarGetMethod;
        /// <summary>
        /// 動力車の情報を提供する <see cref="CarInfo"/> を取得します。
        /// </summary>
        public CarInfo MotorCar => CarInfo.FromSource(MotorCarGetMethod.Invoke(Src, null));

        private static FastMethod FirstCarGetMethod;
        private static FastMethod FirstCarSetMethod;
        /// <summary>
        /// 先頭車両の情報を提供する <see cref="CarInfo"/> を取得します。
        /// </summary>
        public CarInfo FirstCar
        {
            get => CarInfo.FromSource(FirstCarGetMethod.Invoke(Src, null));
            set => FirstCarSetMethod.Invoke(Src, new object[] { value?.Src });
        }
    }
}
