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
    /// 自列車の状態に関する情報を提供します。
    /// </summary>
    public class VehicleStateStore : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleStateStore>();

            SpeedField = members.GetSourceFieldOf(nameof(Speed));
            CurrentField = members.GetSourceFieldOf(nameof(Current));
            BcPressureField = members.GetSourceFieldOf(nameof(BcPressure));
            MrPressureField = members.GetSourceFieldOf(nameof(MrPressure));
            SapPressureField = members.GetSourceFieldOf(nameof(SapPressure));
            BpPressureField = members.GetSourceFieldOf(nameof(BpPressure));
            ErPressureField = members.GetSourceFieldOf(nameof(ErPressure));
            PanelArrayField = members.GetSourceFieldOf(nameof(PanelArray));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleStateStore"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleStateStore(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleStateStore"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleStateStore FromSource(object src) => src is null ? null : new VehicleStateStore(src);

        private static FastField SpeedField;
        /// <summary>
        /// 列車速度 [km/h] を取得・設定します。
        /// </summary>
        public double[] Speed
        {
            get => SpeedField.GetValue(Src);
            set => SpeedField.SetValue(Src, value);
        }

        private static FastField CurrentField;
        /// <summary>
        /// 電流 [A] を取得・設定します。
        /// </summary>
        public double[] Current
        {
            get => CurrentField.GetValue(Src);
            set => CurrentField.SetValue(Src, value);
        }

        private static FastField BcPressureField;
        /// <summary>
        /// ブレーキシリンダ圧力 [kPa] を取得・設定します。
        /// </summary>
        public double[] BcPressure
        {
            get => BcPressureField.GetValue(Src);
            set => BcPressureField.SetValue(Src, value);
        }

        private static FastField MrPressureField;
        /// <summary>
        /// 元空気ダメ圧力 [kPa] を取得・設定します。
        /// </summary>
        public double[] MrPressure
        {
            get => MrPressureField.GetValue(Src);
            set => MrPressureField.SetValue(Src, value);
        }

        private static FastField SapPressureField;
        /// <summary>
        /// 直通管圧力 [kPa] を取得・設定します。
        /// </summary>
        public double[] SapPressure
        {
            get => SapPressureField.GetValue(Src);
            set => SapPressureField.SetValue(Src, value);
        }

        private static FastField BpPressureField;
        /// <summary>
        /// ブレーキ管圧力 [kPa] を取得・設定します。
        /// </summary>
        public double[] BpPressure
        {
            get => BpPressureField.GetValue(Src);
            set => BpPressureField.SetValue(Src, value);
        }

        private static FastField ErPressureField;
        /// <summary>
        /// 釣り合い空気ダメ圧力 [kPa] を取得・設定します。
        /// </summary>
        public double[] ErPressure
        {
            get => ErPressureField.GetValue(Src);
            set => ErPressureField.SetValue(Src, value);
        }

        private static FastField PanelArrayField;
        /// <summary>
        /// パネルに渡す値の配列を取得・設定します。
        /// </summary>
        public double[] PanelArray
        {
            get => PanelArrayField.GetValue(Src);
            set => PanelArrayField.SetValue(Src, value);
        }
    }
}
