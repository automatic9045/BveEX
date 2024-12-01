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
    /// 閉塞を制御するための機能を提供します。
    /// </summary>
    public class SectionManager : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SectionManager>();

            SectionsGetMethod = members.GetSourcePropertyGetterOf(nameof(Sections));
            CurrentSectionSignalIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentSectionSignalIndex));
            CurrentSectionSpeedLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentSectionSpeedLimit));
            ForwardSectionSpeedLimitGetMethod = members.GetSourcePropertyGetterOf(nameof(ForwardSectionSpeedLimit));
            LastSectionGetMethod = members.GetSourcePropertyGetterOf(nameof(LastSection));

            TimeManagerField = members.GetSourceFieldOf(nameof(TimeManager));
            PreTrainPassObjectsField = members.GetSourceFieldOf(nameof(PreTrainPassObjects));
            PreTrainSectionIndexField = members.GetSourceFieldOf(nameof(PreTrainSectionIndex));
            StopSignalSectionIndexesField = members.GetSourceFieldOf(nameof(StopSignalSectionIndexes));
            PreTrainLocationField = members.GetSourceFieldOf(nameof(PreTrainLocation));

            OnSectionChangedMethod = members.GetSourceMethodOf(nameof(OnSectionChanged));
            UpdateSignalsMethod = members.GetSourceMethodOf(nameof(OnSignalChanged));
            UpdatePreTrainSectionMethod = members.GetSourceMethodOf(nameof(UpdatePreTrainSection));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SectionManager"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SectionManager(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SectionManager"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static SectionManager FromSource(object src) => src is null ? null : new SectionManager(src);

        private static FastMethod SectionsGetMethod;
        /// <summary>
        /// 閉塞の一覧を取得します。
        /// </summary>
        public MapFunctionList Sections => MapFunctionList.FromSource(SectionsGetMethod.Invoke(Src, null));

        private static FastMethod CurrentSectionSignalIndexGetMethod;
        /// <summary>
        /// 現在の閉塞における信号インデックスを取得します。
        /// </summary>
        public int CurrentSectionSignalIndex => (int)CurrentSectionSignalIndexGetMethod.Invoke(Src, null);

        private static FastMethod CurrentSectionSpeedLimitGetMethod;
        /// <summary>
        /// 現在の閉塞における、信号による制限速度 [m/s] を取得します。
        /// </summary>
        public double CurrentSectionSpeedLimit => (double)CurrentSectionSpeedLimitGetMethod.Invoke(Src, null);

        private static FastMethod ForwardSectionSpeedLimitGetMethod;
        /// <summary>
        /// 次の閉塞における、信号による制限速度 [m/s] を取得します。
        /// </summary>
        public double ForwardSectionSpeedLimit => (double)ForwardSectionSpeedLimitGetMethod.Invoke(Src, null);

        private static FastMethod LastSectionGetMethod;
        /// <summary>
        /// 最後の閉塞を取得します。
        /// </summary>
        public Section LastSection => Section.FromSource(LastSectionGetMethod.Invoke(Src, null));

        private static FastField TimeManagerField;
        /// <summary>
        /// 閉塞の制御に使用する <see cref="ClassWrappers.TimeManager"/> を取得・設定します。
        /// </summary>
        public TimeManager TimeManager
        {
            get => ClassWrappers.TimeManager.FromSource(TimeManagerField.GetValue(Src));
            set => TimeManagerField.SetValue(Src, value?.Src);
        }

        private static FastField PreTrainPassObjectsField;
        /// <summary>
        /// 先行列車の走行位置を定義している PreTrain.Pass ステートメントの一覧を取得・設定します。
        /// </summary>
        public MapObjectList PreTrainPassObjects
        {
            get => MapObjectList.FromSource(PreTrainPassObjectsField.GetValue(Src));
            set => PreTrainPassObjectsField.SetValue(Src, value?.Src);
        }

        private static FastField PreTrainSectionIndexField;
        /// <summary>
        /// 先行列車が走行している閉塞のインデックスを取得・設定します。
        /// </summary>
        public int PreTrainSectionIndex
        {
            get => (int)PreTrainSectionIndexField.GetValue(Src);
            set => PreTrainSectionIndexField.SetValue(Src, value);
        }

        private static FastField StopSignalSectionIndexesField;
        /// <summary>
        /// 停止現示の基となっている閉塞のインデックスの一覧を取得・設定します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>signalFlag に 1 が設定されている閉塞のうち、自列車より前方にあって、かつ自列車からみて最も近いもの</item>
        /// <item>先行列車が走行している閉塞</item>
        /// </list>
        /// これらの閉塞のインデックスを手前から順に並べたものです。
        /// </remarks>
        public List<int> StopSignalSectionIndexes
        {
            get => StopSignalSectionIndexesField.GetValue(Src) as List<int>;
            set => StopSignalSectionIndexesField.SetValue(Src, value);
        }

        private static FastField PreTrainLocationField;
        /// <summary>
        /// 先行列車が走行している距離程 [m] を取得・設定します。
        /// </summary>
        public double PreTrainLocation
        {
            get => (double)PreTrainLocationField.GetValue(Src);
            set => PreTrainLocationField.SetValue(Src, value);
        }

        private static FastMethod OnSectionChangedMethod;
        /// <summary>
        /// 自列車または先行列車の走行している閉塞が変更されたときに呼び出されます。
        /// </summary>
        public void OnSectionChanged() => OnSectionChangedMethod.Invoke(Src, new object[] { });

        private static FastMethod UpdateSignalsMethod;
        /// <summary>
        /// 各閉塞の信号現示が変更されたとき、または走行している閉塞が変更されたときに呼び出されます。
        /// </summary>
        /// <remarks>
        /// 各閉塞の信号現示を取得するには <see cref="Section.CurrentSignalIndex"/> プロパティを使用してください。
        /// </remarks>
        public void OnSignalChanged() => UpdateSignalsMethod.Invoke(Src, new object[] { });

        private static FastMethod UpdatePreTrainSectionMethod;
        /// <summary>
        /// 先行列車が走行している閉塞を最新の状態に更新します。
        /// </summary>
        public void UpdatePreTrainSection() => UpdatePreTrainSectionMethod.Invoke(Src, new object[] { });
    }
}
