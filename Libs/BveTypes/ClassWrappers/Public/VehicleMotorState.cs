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
    /// 自列車のモーターの状態に関する情報を提供します。
    /// </summary>
    public class VehicleMotorState : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleMotorState>();

            CarForceField = members.GetSourceFieldOf(nameof(CarForce));
            CurrentField = members.GetSourceFieldOf(nameof(Current));
            TorqueCurrentField = members.GetSourceFieldOf(nameof(TorqueCurrent));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleMotorState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleMotorState(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleMotorState"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleMotorState FromSource(object src) => src is null ? null : new VehicleMotorState(src);

        private static FastField CarForceField;
        /// <summary>
        /// 1 両当たりの引張力 [N] を取得・設定します。
        /// </summary>
        public double CarForce
        {
            get => CarForceField.GetValue(Src);
            set => CarForceField.SetValue(Src, value);
        }

        private static FastField CurrentField;
        /// <summary>
        /// 電流 [A] を取得・設定します。
        /// </summary>
        public double Current
        {
            get => CurrentField.GetValue(Src);
            set => CurrentField.SetValue(Src, value);
        }

        private static FastField TorqueCurrentField;
        /// <summary>
        /// トルク分電流 [A] を取得・設定します。
        /// </summary>
        public double TorqueCurrent
        {
            get => TorqueCurrentField.GetValue(Src);
            set => TorqueCurrentField.SetValue(Src, value);
        }
    }
}
