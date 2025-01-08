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
    /// シナリオ本体の詳細へアクセスするための機能を提供します。
    /// </summary>
    /// <remarks>
    /// 読み込まれたファイルに関する情報にアクセスするには <see cref="ScenarioInfo"/> を使用してください。
    /// </remarks>
    /// <seealso cref="ScenarioInfo"/>
    public class Scenario : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Scenario>();

            TimeManagerGetMethod = members.GetSourcePropertyGetterOf(nameof(TimeManager));
            VehicleLocationGetMethod = members.GetSourcePropertyGetterOf(nameof(VehicleLocation));
            MapGetMethod = members.GetSourcePropertyGetterOf(nameof(Map));
            VehicleGetMethod = members.GetSourcePropertyGetterOf(nameof(Vehicle));
            TrainsGetMethod = members.GetSourcePropertyGetterOf(nameof(Trains));
            SectionManagerGetMethod = members.GetSourcePropertyGetterOf(nameof(SectionManager));
            TimeTableGetMethod = members.GetSourcePropertyGetterOf(nameof(TimeTable));

            ObjectDrawerField = members.GetSourceFieldOf(nameof(ObjectDrawer));

            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
            InitializeTimeAndLocationMethod = members.GetSourceMethodOf(nameof(InitializeTimeAndLocation));
            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Scenario"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Scenario(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Scenario"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Scenario FromSource(object src) => src is null ? null : new Scenario(src);

        private static FastMethod TimeManagerGetMethod;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="ClassWrappers.TimeManager"/> のインスタンスを取得します。
        /// </summary>
        public TimeManager TimeManager => ClassWrappers.TimeManager.FromSource(TimeManagerGetMethod.Invoke(Src, null));

        private static FastMethod VehicleLocationGetMethod;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="ClassWrappers.VehicleLocation"/> のインスタンスを取得します。
        /// </summary>
        public VehicleLocation VehicleLocation => ClassWrappers.VehicleLocation.FromSource(VehicleLocationGetMethod.Invoke(Src, null));

        private static FastMethod MapGetMethod;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="ClassWrappers.Map"/> のインスタンスを取得します。
        /// </summary>
        public Map Map => ClassWrappers.Map.FromSource(MapGetMethod.Invoke(Src, null));

        private static FastMethod VehicleGetMethod;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="ClassWrappers.Vehicle"/> のインスタンスを取得します。
        /// </summary>
        public Vehicle Vehicle => ClassWrappers.Vehicle.FromSource(VehicleGetMethod.Invoke(Src, null));

        private static FastMethod TrainsGetMethod;
        /// <summary>
        /// 他列車の一覧を取得します。
        /// </summary>
        /// <remarks>キーはマップファイル内で定義した他列車名、値は他列車を表す <see cref="Train"/> です。</remarks>
        public WrappedSortedList<string, Train> Trains
        {
            get
            {
                IDictionary dictionarySrc = TrainsGetMethod.Invoke(Src, null) as IDictionary;
                return new WrappedSortedList<string, Train>(dictionarySrc);
            }
        }

        private static FastMethod SectionManagerGetMethod;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="SectionManager"/> のインスタンスを取得します。
        /// </summary>
        public SectionManager SectionManager => ClassWrappers.SectionManager.FromSource(SectionManagerGetMethod.Invoke(Src, null));

        private static FastMethod TimeTableGetMethod;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="ClassWrappers.TimeTable"/> のインスタンスを取得します。
        /// </summary>
        public TimeTable TimeTable => ClassWrappers.TimeTable.FromSource(TimeTableGetMethod.Invoke(Src, null));

        private static FastField ObjectDrawerField;
        /// <summary>
        /// このシナリオに関連付けられた <see cref="ClassWrappers.ObjectDrawer"/> のインスタンスを取得します。
        /// </summary>
        public ObjectDrawer ObjectDrawer => ClassWrappers.ObjectDrawer.FromSource(ObjectDrawerField.GetValue(Src));

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);

        private static FastMethod InitializeTimeAndLocationMethod;
        /// <summary>
        /// 現在の時刻と自列車の位置を初期化します。
        /// </summary>
        /// <param name="location">自列車の距離程 [m]。</param>
        /// <param name="timeMilliseconds">時刻。0 時丁度から経過した時間 [ms] で指定します。</param>
        public void InitializeTimeAndLocation(double location, int timeMilliseconds) => InitializeTimeAndLocationMethod.Invoke(Src, new object[] { location, timeMilliseconds });

        private static FastMethod InitializeMethod;
        /// <summary>
        /// 走行中である停車場間の情報を初期化します。
        /// </summary>
        /// <param name="stationIndex">停車場のインデックス。</param>
        public void Initialize(int stationIndex) => InitializeMethod.Invoke(Src, new object[] { stationIndex });

        private static FastMethod DrawMethod;
        /// <summary>
        /// ストラクチャーと運転台パネルを描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        public void Draw(Direct3DProvider direct3DProvider) => DrawMethod.Invoke(Src, new object[] { direct3DProvider?.Src });
    }
}
