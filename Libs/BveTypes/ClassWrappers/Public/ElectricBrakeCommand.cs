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
    /// 電空協調制御から主回路へ送られる電気ブレーキ力の目標値の指令を表します。
    /// </summary>
    public class ElectricBrakeCommand : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ElectricBrakeCommand>();

            RateField = members.GetSourceFieldOf(nameof(Rate));
            ForceField = members.GetSourceFieldOf(nameof(Force));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ElectricBrakeCommand"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ElectricBrakeCommand(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ElectricBrakeCommand"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ElectricBrakeCommand FromSource(object src) => src is null ? null : new ElectricBrakeCommand(src);

        private static FastField RateField;
        /// <summary>
        /// ブレーキ率を取得・設定します。
        /// </summary>
        public double Rate
        {
            get => (double)RateField.GetValue(Src);
            set => RateField.SetValue(Src, value);
        }

        private static FastField ForceField;
        /// <summary>
        /// ブレーキ力 [N] を取得・設定します。
        /// </summary>
        public double Force
        {
            get => (double)ForceField.GetValue(Src);
            set => ForceField.SetValue(Src, value);
        }
    }
}
