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
    /// 自列車の乗客を表します。
    /// </summary>
    public partial class Passenger : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Passenger>();

            LoadGetMethod = members.GetSourcePropertyGetterOf(nameof(Load));

            BodyWeightGetMethod = members.GetSourcePropertyGetterOf(nameof(BodyWeight));
            BodyWeightSetMethod = members.GetSourcePropertySetterOf(nameof(BodyWeight));

            StateGetMethod = members.GetSourcePropertyGetterOf(nameof(State));

            CapacityGetMethod = members.GetSourcePropertyGetterOf(nameof(Capacity));
            CapacitySetMethod = members.GetSourcePropertySetterOf(nameof(Capacity));

            BoardingSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(BoardingSpeed));
            BoardingSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(BoardingSpeed));

            AlightingSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(AlightingSpeed));
            AlightingSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(AlightingSpeed));

            CountField = members.GetSourceFieldOf(nameof(Count));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Passenger"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Passenger(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Passenger"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Passenger FromSource(object src) => src is null ? null : new Passenger(src);

        private static FastMethod LoadGetMethod;
        /// <summary>
        /// 1 両当たりの乗客の合計質量 [kg] を取得します。
        /// </summary>
        public ValueContainer Load => ValueContainer.FromSource(LoadGetMethod.Invoke(Src, null));

        private static FastMethod BodyWeightGetMethod;
        private static FastMethod BodyWeightSetMethod;
        /// <summary>
        /// 乗客 1 人当たりの質量 [kg] を取得・設定します。
        /// </summary>
        public double BodyWeight
        {
            get => (double)BodyWeightGetMethod.Invoke(Src, null);
            set => BodyWeightSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod StateGetMethod;
        /// <summary>
        /// 停車場における乗降の進捗を取得します。
        /// </summary>
        public StationProcess State => (StationProcess)StateGetMethod.Invoke(Src, null);

        private static FastMethod CapacityGetMethod;
        private static FastMethod CapacitySetMethod;
        /// <summary>
        /// 1 両当たりの定員を取得・設定します。
        /// </summary>
        public double Capacity
        {
            get => (double)CapacityGetMethod.Invoke(Src, null);
            set => CapacitySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BoardingSpeedGetMethod;
        private static FastMethod BoardingSpeedSetMethod;
        /// <summary>
        /// 旅客が乗車する速さ [/s] を取得・設定します。
        /// </summary>
        public double BoardingSpeed
        {
            get => (double)BoardingSpeedGetMethod.Invoke(Src, null);
            set => BoardingSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod AlightingSpeedGetMethod;
        private static FastMethod AlightingSpeedSetMethod;
        /// <summary>
        /// 旅客が降車する速さ [/s] を取得・設定します。
        /// </summary>
        public double AlightingSpeed
        {
            get => (double)AlightingSpeedGetMethod.Invoke(Src, null);
            set => AlightingSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField CountField;
        /// <summary>
        /// 現在の 1 両当たりの乗車人数を取得・設定します。
        /// </summary>
        public double Count
        {
            get => (double)CountField.GetValue(Src);
            set => CountField.SetValue(Src, value);
        }
    }
}
