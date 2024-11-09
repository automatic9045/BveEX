using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の制御段を表します。
    /// </summary>
    public class VehicleStep : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleStep>();

            CurvesGetMethod = members.GetSourcePropertyGetterOf(nameof(Curves));

            ForceGetMethod = members.GetSourcePropertyGetterOf(nameof(Force));
            MaxForceGetMethod = members.GetSourcePropertyGetterOf(nameof(MaxForce));
            CurrentGetMethod = members.GetSourcePropertyGetterOf(nameof(Current));
            MaxCurrentGetMethod = members.GetSourcePropertyGetterOf(nameof(MaxCurrent));
            NoLoadCurrentGetMethod = members.GetSourcePropertyGetterOf(nameof(NoLoadCurrent));

            JerkRegulationUpGetMethod = members.GetSourcePropertyGetterOf(nameof(JerkRegulationUp));
            JerkRegulationUpSetMethod = members.GetSourcePropertySetterOf(nameof(JerkRegulationUp));

            JerkRegulationDownGetMethod = members.GetSourcePropertyGetterOf(nameof(JerkRegulationDown));
            JerkRegulationDownSetMethod = members.GetSourcePropertySetterOf(nameof(JerkRegulationDown));

            BreakerDelayOnGetMethod = members.GetSourcePropertyGetterOf(nameof(BreakerDelayOn));
            BreakerDelayOnSetMethod = members.GetSourcePropertySetterOf(nameof(BreakerDelayOn));

            BreakerDelayOffGetMethod = members.GetSourcePropertyGetterOf(nameof(BreakerDelayOff));
            BreakerDelayOffSetMethod = members.GetSourcePropertySetterOf(nameof(BreakerDelayOff));

            ResetTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(ResetTime));
            ResetTimeSetMethod = members.GetSourcePropertySetterOf(nameof(ResetTime));

            CurrentReducingTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentReducingTime));
            CurrentReducingTimeSetMethod = members.GetSourcePropertySetterOf(nameof(CurrentReducingTime));

            RequiredNotchUpGetMethod = members.GetSourcePropertyGetterOf(nameof(RequiredNotchUp));
            RequiredNotchUpSetMethod = members.GetSourcePropertySetterOf(nameof(RequiredNotchUp));

            RequiredNotchDownGetMethod = members.GetSourcePropertyGetterOf(nameof(RequiredNotchDown));
            RequiredNotchDownSetMethod = members.GetSourcePropertySetterOf(nameof(RequiredNotchDown));

            StopDelayUpGetMethod = members.GetSourcePropertyGetterOf(nameof(StopDelayUp));
            StopDelayUpSetMethod = members.GetSourcePropertySetterOf(nameof(StopDelayUp));

            StopDelayDownGetMethod = members.GetSourcePropertyGetterOf(nameof(StopDelayDown));
            StopDelayDownSetMethod = members.GetSourcePropertySetterOf(nameof(StopDelayDown));

            CurrentLimitingValueEmptyGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentLimitingValueEmpty));
            CurrentLimitingValueEmptySetMethod = members.GetSourcePropertySetterOf(nameof(CurrentLimitingValueEmpty));

            CurrentLimitingValueFullGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentLimitingValueFull));
            CurrentLimitingValueFullSetMethod = members.GetSourcePropertySetterOf(nameof(CurrentLimitingValueFull));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleStep"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleStep(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleStep"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleStep FromSource(object src) => src is null ? null : new VehicleStep(src);

        private static FastMethod CurvesGetMethod;
        /// <summary>
        /// テーブルの一覧を取得します。
        /// </summary>
        public WrappedSortedList<string, GraphCurve> Curves
        {
            get
            {
                IDictionary dictionarySrc = CurvesGetMethod.Invoke(Src, null) as IDictionary;
                return new WrappedSortedList<string, GraphCurve>(dictionarySrc);
            }
        }

        private static FastMethod ForceGetMethod;
        /// <summary>
        /// 空車時の引張力を指定する引張力テーブルを取得します。
        /// </summary>
        public GraphCurve Force => GraphCurve.FromSource(ForceGetMethod.Invoke(Src, null));

        private static FastMethod MaxForceGetMethod;
        /// <summary>
        /// 応荷重制御で増加できる最大の引張力を指定する引張力テーブルを取得します。
        /// </summary>
        public GraphCurve MaxForce => GraphCurve.FromSource(MaxForceGetMethod.Invoke(Src, null));

        private static FastMethod CurrentGetMethod;
        /// <summary>
        /// 空車時の主電動機電流を指定する電流テーブルを取得します。
        /// </summary>
        public GraphCurve Current => GraphCurve.FromSource(CurrentGetMethod.Invoke(Src, null));

        private static FastMethod MaxCurrentGetMethod;
        /// <summary>
        /// 応荷重制御で増加できる最大の主電動機電流を指定する電流テーブルを取得します。
        /// </summary>
        public GraphCurve MaxCurrent => GraphCurve.FromSource(MaxCurrentGetMethod.Invoke(Src, null));

        private static FastMethod NoLoadCurrentGetMethod;
        /// <summary>
        /// 引張力が 0 となる主電動機電流を指定する電流テーブルを取得します。
        /// </summary>
        public GraphCurve NoLoadCurrent => GraphCurve.FromSource(NoLoadCurrentGetMethod.Invoke(Src, null));

        private static FastMethod JerkRegulationUpGetMethod;
        private static FastMethod JerkRegulationUpSetMethod;
        /// <summary>
        /// ジャーク制御の電流が増加する速さ [A/s] を取得・設定します。
        /// </summary>
        public double JerkRegulationUp
        {
            get => (double)JerkRegulationUpGetMethod.Invoke(Src, null);
            set => JerkRegulationUpSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod JerkRegulationDownGetMethod;
        private static FastMethod JerkRegulationDownSetMethod;
        /// <summary>
        /// ジャーク制御の電流が減少する速さ [A/s] を取得・設定します。
        /// </summary>
        public double JerkRegulationDown
        {
            get => (double)JerkRegulationDownGetMethod.Invoke(Src, null);
            set => JerkRegulationDownSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BreakerDelayOnGetMethod;
        private static FastMethod BreakerDelayOnSetMethod;
        /// <summary>
        /// 断流器入時の遅れ時間 [s] を取得・設定します。
        /// </summary>
        public double BreakerDelayOn
        {
            get => (double)BreakerDelayOnGetMethod.Invoke(Src, null);
            set => BreakerDelayOnSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BreakerDelayOffGetMethod;
        private static FastMethod BreakerDelayOffSetMethod;
        /// <summary>
        /// 断流器切時の遅れ時間 [s] を取得・設定します。
        /// </summary>
        public double BreakerDelayOff
        {
            get => (double)BreakerDelayOffGetMethod.Invoke(Src, null);
            set => BreakerDelayOffSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ResetTimeGetMethod;
        private static FastMethod ResetTimeSetMethod;
        /// <summary>
        /// 断流器切後、入可能になるまでの時間 [s] を取得・設定します。
        /// </summary>
        public double ResetTime
        {
            get => (double)ResetTimeGetMethod.Invoke(Src, null);
            set => ResetTimeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CurrentReducingTimeGetMethod;
        private static FastMethod CurrentReducingTimeSetMethod;
        /// <summary>
        /// 減流遮断における減流時間 [s] を取得・設定します。
        /// </summary>
        public double CurrentReducingTime
        {
            get => (double)CurrentReducingTimeGetMethod.Invoke(Src, null);
            set => CurrentReducingTimeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RequiredNotchUpGetMethod;
        private static FastMethod RequiredNotchUpSetMethod;
        /// <summary>
        /// 力行または抑速時、次の段に進むために必要な最小のハンドル位置を取得・設定します。
        /// </summary>
        public int RequiredNotchUp
        {
            get => (int)RequiredNotchUpGetMethod.Invoke(Src, null);
            set => RequiredNotchUpSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RequiredNotchDownGetMethod;
        private static FastMethod RequiredNotchDownSetMethod;
        /// <summary>
        /// 力行または抑速時、前の段に戻るために必要な最大のハンドル位置を取得・設定します。
        /// </summary>
        public int RequiredNotchDown
        {
            get => (int)RequiredNotchDownGetMethod.Invoke(Src, null);
            set => RequiredNotchDownSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod StopDelayUpGetMethod;
        private static FastMethod StopDelayUpSetMethod;
        /// <summary>
        /// 次の段に進むのに要する時間 [s] を取得・設定します。
        /// </summary>
        public double StopDelayUp
        {
            get => (double)StopDelayUpGetMethod.Invoke(Src, null);
            set => StopDelayUpSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod StopDelayDownGetMethod;
        private static FastMethod StopDelayDownSetMethod;
        /// <summary>
        /// 前の段に進むのに要する時間 [s] を取得・設定します。
        /// </summary>
        public double StopDelayDown
        {
            get => (double)StopDelayDownGetMethod.Invoke(Src, null);
            set => StopDelayDownSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CurrentLimitingValueEmptyGetMethod;
        private static FastMethod CurrentLimitingValueEmptySetMethod;
        /// <summary>
        /// 空車時における、次の段に進むことができる最大の電流 [A] を取得・設定します。
        /// </summary>
        public double CurrentLimitingValueEmpty
        {
            get => (double)CurrentLimitingValueEmptyGetMethod.Invoke(Src, null);
            set => CurrentLimitingValueEmptySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CurrentLimitingValueFullGetMethod;
        private static FastMethod CurrentLimitingValueFullSetMethod;
        /// <summary>
        /// 応荷重最大時における、次の段に進むことができる最大の電流 [A] を取得・設定します。
        /// </summary>
        public double CurrentLimitingValueFull
        {
            get => (double)CurrentLimitingValueFullGetMethod.Invoke(Src, null);
            set => CurrentLimitingValueFullSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
