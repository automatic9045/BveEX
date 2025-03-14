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
    /// 主電動機を表します。
    /// </summary>
    public class TractionMotor : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TractionMotor>();

            Constructor = members.GetSourceConstructor();

            PerformanceField = members.GetSourceFieldOf(nameof(Performance));
            MotorCarTachogeneratorField = members.GetSourceFieldOf(nameof(MotorCarTachogenerator));
            ForceEmptyField = members.GetSourceFieldOf(nameof(ForceEmpty));
            ForceFullField = members.GetSourceFieldOf(nameof(ForceFull));
            CurrentEmptyField = members.GetSourceFieldOf(nameof(CurrentEmpty));
            CurrentFullField = members.GetSourceFieldOf(nameof(CurrentFull));
            NoLoadCurrentField = members.GetSourceFieldOf(nameof(NoLoadCurrent));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TractionMotor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TractionMotor(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TractionMotor"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static TractionMotor FromSource(object src) => src is null ? null : new TractionMotor(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="TractionMotor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="motorCarTachogenerator">電動車の車輪に取り付けられた速度発電機。</param>
        /// <param name="performance">車両性能。</param>
        public TractionMotor(Tachogenerator motorCarTachogenerator, VehiclePerformance performance)
            : this(Constructor.Invoke(new object[] { motorCarTachogenerator?.Src, performance?.Src }))
        {
        }

        private static FastField PerformanceField;
        /// <summary>
        /// 車両性能を取得・設定します。
        /// </summary>
        public VehiclePerformance Performance
        {
            get => VehiclePerformance.FromSource(PerformanceField.GetValue(Src));
            set => PerformanceField.SetValue(Src, value?.Src);
        }

        private static FastField MotorCarTachogeneratorField;
        /// <summary>
        /// 電動車の車輪に取り付けられた速度発電機を取得・設定します。
        /// </summary>
        public Tachogenerator MotorCarTachogenerator
        {
            get => Tachogenerator.FromSource(MotorCarTachogeneratorField.GetValue(Src));
            set => MotorCarTachogeneratorField.SetValue(Src, value?.Src);
        }

        private static FastField ForceEmptyField;
        /// <summary>
        /// 空車時の 1 両当たりの引張力 [N] を取得・設定します。
        /// </summary>
        public double ForceEmpty
        {
            get => (double)ForceEmptyField.GetValue(Src);
            set => ForceEmptyField.SetValue(Src, value);
        }

        private static FastField ForceFullField;
        /// <summary>
        /// 満車時の 1 両当たりの引張力 [N] を取得・設定します。
        /// </summary>
        public double ForceFull
        {
            get => (double)ForceFullField.GetValue(Src);
            set => ForceFullField.SetValue(Src, value);
        }

        private static FastField CurrentEmptyField;
        /// <summary>
        /// 空車時の電流 [A] を取得・設定します。
        /// </summary>
        public double CurrentEmpty
        {
            get => (double)CurrentEmptyField.GetValue(Src);
            set => CurrentEmptyField.SetValue(Src, value);
        }

        private static FastField CurrentFullField;
        /// <summary>
        /// 満車時の電流 [A] を取得・設定します。
        /// </summary>
        public double CurrentFull
        {
            get => (double)CurrentFullField.GetValue(Src);
            set => CurrentFullField.SetValue(Src, value);
        }

        private static FastField NoLoadCurrentField;
        /// <summary>
        /// 無負荷電流 [A] を取得・設定します。
        /// </summary>
        public double NoLoadCurrent
        {
            get => (double)NoLoadCurrentField.GetValue(Src);
            set => NoLoadCurrentField.SetValue(Src, value);
        }
    }
}
