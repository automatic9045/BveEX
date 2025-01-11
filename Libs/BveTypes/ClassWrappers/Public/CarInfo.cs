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
    /// 自列車の車両に関する物理演算機能を提供します。
    /// </summary>
    public class CarInfo : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CarInfo>();

            WeightGetMethod = members.GetSourcePropertyGetterOf(nameof(Weight));
            WeightSetMethod = members.GetSourcePropertySetterOf(nameof(Weight));

            CountGetMethod = members.GetSourcePropertyGetterOf(nameof(Count));
            CountSetMethod = members.GetSourcePropertySetterOf(nameof(Count));

            InertiaFactorGetMethod = members.GetSourcePropertyGetterOf(nameof(InertiaFactor));
            InertiaFactorSetMethod = members.GetSourcePropertySetterOf(nameof(InertiaFactor));

            TachogeneratorGetMethod = members.GetSourcePropertyGetterOf(nameof(Tachogenerator));

            MotorStateGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorState));
            MotorStateSetMethod = members.GetSourcePropertySetterOf(nameof(MotorState));

            BrakePistonForceGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakePistonForce));
            BrakePistonForceSetMethod = members.GetSourcePropertySetterOf(nameof(BrakePistonForce));

            ShoeFrictionAGetMethod = members.GetSourcePropertyGetterOf(nameof(ShoeFrictionA));
            ShoeFrictionASetMethod = members.GetSourcePropertySetterOf(nameof(ShoeFrictionA));

            ShoeFrictionBGetMethod = members.GetSourcePropertyGetterOf(nameof(ShoeFrictionB));
            ShoeFrictionBSetMethod = members.GetSourcePropertySetterOf(nameof(ShoeFrictionB));

            ShoeFrictionCGetMethod = members.GetSourcePropertyGetterOf(nameof(ShoeFrictionC));
            ShoeFrictionCSetMethod = members.GetSourcePropertySetterOf(nameof(ShoeFrictionC));

            IsSlippingField = members.GetSourceFieldOf(nameof(IsSlipping));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CarInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CarInfo(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CarInfo"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static CarInfo FromSource(object src) => src is null ? null : new CarInfo(src);

        private static FastMethod WeightGetMethod;
        private static FastMethod WeightSetMethod;
        /// <summary>
        /// 1 両当たりの重量 [kg] を取得・設定します。
        /// </summary>
        public double Weight
        {
            get => (double)WeightGetMethod.Invoke(Src, null);
            set => WeightSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CountGetMethod;
        private static FastMethod CountSetMethod;
        /// <summary>
        /// 両数を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 必要に応じて <see cref="AirSupplement.MotorCarRatio"/> プロパティも設定してください。
        /// </remarks>
        /// <seealso cref="AirSupplement.MotorCarRatio"/>
        public double Count
        {
            get => (double)CountGetMethod.Invoke(Src, null);
            set => CountSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod InertiaFactorGetMethod;
        private static FastMethod InertiaFactorSetMethod;
        /// <summary>
        /// 慣性係数を取得・設定します。
        /// </summary>
        public double InertiaFactor
        {
            get => (double)InertiaFactorGetMethod.Invoke(Src, null);
            set => InertiaFactorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TachogeneratorGetMethod;
        /// <summary>
        /// 車輪の運動を測定する速度発電機を取得します。
        /// </summary>
        public Tachogenerator Tachogenerator => Tachogenerator.FromSource(TachogeneratorGetMethod.Invoke(Src, null));

        private static FastMethod MotorStateGetMethod;
        private static FastMethod MotorStateSetMethod;
        /// <summary>
        /// モーターの状態を取得・設定します。
        /// </summary>
        public VehicleMotorState MotorState
        {
            get => VehicleMotorState.FromSource(MotorStateGetMethod.Invoke(Src, null));
            set => MotorStateSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod BrakePistonForceGetMethod;
        private static FastMethod BrakePistonForceSetMethod;
        /// <summary>
        /// 基礎ブレーキ装置のピストンの 1 両当たりの押し付け力 [N] を取得・設定します。
        /// </summary>
        public ValueContainer BrakePistonForce
        {
            get => ValueContainer.FromSource(BrakePistonForceGetMethod.Invoke(Src, null));
            set => BrakePistonForceSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod ShoeFrictionAGetMethod;
        private static FastMethod ShoeFrictionASetMethod;
        /// <summary>
        /// 制輪子摩擦係数式の係数 a を取得・設定します。
        /// </summary>
        public double ShoeFrictionA
        {
            get => (double)ShoeFrictionAGetMethod.Invoke(Src, null);
            set => ShoeFrictionASetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ShoeFrictionBGetMethod;
        private static FastMethod ShoeFrictionBSetMethod;
        /// <summary>
        /// 制輪子摩擦係数式の係数 b を取得・設定します。
        /// </summary>
        public double ShoeFrictionB
        {
            get => (double)ShoeFrictionBGetMethod.Invoke(Src, null);
            set => ShoeFrictionBSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ShoeFrictionCGetMethod;
        private static FastMethod ShoeFrictionCSetMethod;
        /// <summary>
        /// 制輪子摩擦係数式の係数 c を取得・設定します。
        /// </summary>
        public double ShoeFrictionC
        {
            get => (double)ShoeFrictionCGetMethod.Invoke(Src, null);
            set => ShoeFrictionCSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField IsSlippingField;
        /// <summary>
        /// 現在空転または滑走しているかどうかを取得・設定します。
        /// </summary>
        public bool IsSlipping
        {
            get => (bool)IsSlippingField.GetValue(Src);
            set => IsSlippingField.SetValue(Src, value);
        }
    }
}
