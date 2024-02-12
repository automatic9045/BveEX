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

            StateGetMethod = members.GetSourcePropertyGetterOf(nameof(State));

            CapacityGetMethod = members.GetSourcePropertyGetterOf(nameof(Capacity));
            CapacitySetMethod = members.GetSourcePropertySetterOf(nameof(Capacity));

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
            get => CapacityGetMethod.Invoke(Src, null);
            set => CapacitySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField CountField;
        /// <summary>
        /// 現在の 1 両当たりの乗車人数を取得・設定します。
        /// </summary>
        public double Count
        {
            get => CountField.GetValue(Src);
            set => CountField.SetValue(Src, value);
        }
    }
}
