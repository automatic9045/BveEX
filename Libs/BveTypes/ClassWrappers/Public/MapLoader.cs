using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX.DirectSound;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// マップを読み込むための機能を提供します。
    /// </summary>
    public class MapLoader : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapLoader>();

            Constructor = members.GetSourceConstructor(
                new Type[] { typeof(LoadingProgressForm), typeof(string), typeof(DirectSound), typeof(TimeManager), typeof(Map), typeof(CameraLocation), typeof(bool) });

            LoadingProgressFormField = members.GetSourceFieldOf(nameof(LoadingProgressForm));
            TimeManagerField = members.GetSourceFieldOf(nameof(TimeManager));
            MapField = members.GetSourceFieldOf(nameof(Map));
            DirectSoundField = members.GetSourceFieldOf(nameof(DirectSound));
            VariablesField = members.GetSourceFieldOf(nameof(Variables));
            StationsField = members.GetSourceFieldOf(nameof(Stations));
            CurrentLocationField = members.GetSourceFieldOf(nameof(CurrentLocation));
            FilePathField = members.GetSourceFieldOf(nameof(FilePath));
            DirectoryField = members.GetSourceFieldOf(nameof(Directory));
            CameraLocationField = members.GetSourceFieldOf(nameof(CameraLocation));
            StatementsField = members.GetSourceFieldOf(nameof(Statements));

            LoadMethod = members.GetSourceMethodOf(nameof(Load));
            RegisterFileMethod = members.GetSourceMethodOf(nameof(RegisterFile));
            ParseStatementMethod = members.GetSourceMethodOf(nameof(ParseStatement));
            LoadSound3DListMethod = members.GetSourceMethodOf(nameof(LoadSound3DList));
            LoadSoundListMethod = members.GetSourceMethodOf(nameof(LoadSoundList));
            LoadSignalListMethod = members.GetSourceMethodOf(nameof(LoadSignalList));
            LoadStructureListMethod = members.GetSourceMethodOf(nameof(LoadStructureList));
            LoadStationListMethod = members.GetSourceMethodOf(nameof(LoadStationList));
            IncludeMethod = members.GetSourceMethodOf(nameof(Include));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapLoader"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapLoader(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapLoader"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapLoader FromSource(object src) => src is null ? null : new MapLoader(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="MapLoader"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="loadingProgressForm">読込の進捗の報告先として使用する「シナリオを読み込んでいます...」フォーム。</param>
        /// <param name="filePath">読込対象となるマップファイルのパス。</param>
        /// <param name="directSound">DirectSound デバイス。</param>
        /// <param name="timeManager">時間を制御する <see cref="ClassWrappers.TimeManager"/>。</param>
        /// <param name="map">読み込んだマップデータを記憶する <see cref="ClassWrappers.Map"/>。</param>
        /// <param name="cameraLocation"><see cref="Sound"/> を読み込む際に指定する <see cref="ClassWrappers.CameraLocation"/>。</param>
        /// <param name="skipLoadStructures">ストラクチャーの読込をスキップするかどうか。</param>
        public MapLoader(LoadingProgressForm loadingProgressForm, string filePath, DirectSound directSound, TimeManager timeManager, Map map, CameraLocation cameraLocation, bool skipLoadStructures)
            : this(Constructor.Invoke(new object[] { loadingProgressForm?.Src, filePath, directSound, timeManager?.Src, map?.Src, cameraLocation?.Src, skipLoadStructures }))
        {
        }


        private static FastField LoadingProgressFormField;
        /// <summary>
        /// 読込の進捗の報告先として使用する「シナリオを読み込んでいます...」フォームを取得・設定します。
        /// </summary>
        public LoadingProgressForm LoadingProgressForm
        {
            get => ClassWrappers.LoadingProgressForm.FromSource(LoadingProgressFormField.GetValue(Src));
            set => LoadingProgressFormField.SetValue(Src, value?.Src);
        }

        private static FastField TimeManagerField;
        /// <summary>
        /// 時間を制御する <see cref="ClassWrappers.TimeManager"/> を取得・設定します。
        /// </summary>
        public TimeManager TimeManager
        {
            get => ClassWrappers.TimeManager.FromSource(TimeManagerField.GetValue(Src));
            set => TimeManagerField.SetValue(Src, value?.Src);
        }

        private static FastField MapField;
        /// <summary>
        /// 読み込んだマップデータを記憶する <see cref="ClassWrappers.Map"/> を取得・設定します。
        /// </summary>
        public Map Map
        {
            get => ClassWrappers.Map.FromSource(MapField.GetValue(Src));
            set => MapField.SetValue(Src, value?.Src);
        }

        private static FastField DirectSoundField;
        /// <summary>
        /// DirectSound デバイスを取得・設定します。
        /// </summary>
        public DirectSound DirectSound
        {
            get => DirectSoundField.GetValue(Src);
            set => DirectSoundField.SetValue(Src, value);
        }

        private static FastField VariablesField;
        /// <summary>
        /// 変数の一覧を取得・設定します。
        /// </summary>
        public SortedList<string, object> Variables
        {
            get => VariablesField.GetValue(Src);
            set => VariablesField.SetValue(Src, value);
        }

        private static FastField StationsField;
        /// <summary>
        /// 駅の一覧を取得・設定します。
        /// </summary>
        public WrappedSortedList<string, Station> Stations
        {
            get
            {
                IDictionary dictionarySrc = StationsField.GetValue(Src);
                return new WrappedSortedList<string, Station>(dictionarySrc);
            }
            set => VariablesField.SetValue(Src, value?.Src);
        }

        private static FastField CurrentLocationField;
        /// <summary>
        /// 現在の距離程 [m] を取得・設定します。
        /// </summary>
        public double CurrentLocation
        {
            get => CurrentLocationField.GetValue(Src);
            set => CurrentLocationField.SetValue(Src, value);
        }

        private static FastField FilePathField;
        /// <summary>
        /// 読込対象となるマップファイルのパスを取得・設定します。
        /// </summary>
        public string FilePath
        {
            get => FilePathField.GetValue(Src);
            set => FilePathField.SetValue(Src, value);
        }

        private static FastField DirectoryField;
        /// <summary>
        /// 読込対象となるマップファイルの配置されているディレクトリを取得・設定します。
        /// </summary>
        public string Directory
        {
            get => DirectoryField.GetValue(Src);
            set => DirectoryField.SetValue(Src, value);
        }

        private static FastField CameraLocationField;
        /// <summary>
        /// <see cref="Sound"/> を読み込む際に指定する <see cref="ClassWrappers.CameraLocation"/> を取得・設定します。
        /// </summary>
        public CameraLocation CameraLocation
        {
            get => ClassWrappers.CameraLocation.FromSource(CameraLocationField.GetValue(Src));
            set => CameraLocationField.SetValue(Src, value?.Src);
        }

        private static FastField StatementsField;
        /// <summary>
        /// ステートメントの一覧を取得・設定します。
        /// </summary>
        public MapStatementList Statements
        {
            get => MapStatementList.FromSource(StatementsField.GetValue(Src));
            set => StatementsField.SetValue(Src, value?.Src);
        }


        private static FastMethod LoadMethod;
        /// <summary>
        /// マップを読み込みます。
        /// </summary>
        /// <returns>読込に成功した場合は <see langword="true"/>、失敗した場合は <see langword="false"/>。</returns>
        public bool Load() => LoadMethod.Invoke(Src, null);

        private static FastMethod RegisterFileMethod;
        /// <summary>
        /// 読込対象となるマップファイルを登録します。
        /// </summary>
        /// <param name="filePath">マップファイルのパス。</param>
        /// <returns>読込に成功した場合は <see langword="true"/>、失敗した場合は <see langword="false"/>。</returns>
        public bool RegisterFile(string filePath) => RegisterFileMethod.Invoke(Src, new object[] { filePath });

        private static FastMethod ParseStatementMethod;
        /// <summary>
        /// 1 つのステートメントを構文解析します。
        /// </summary>
        /// <param name="clauses">ステートメントを構成する句の一覧。</param>
        public void ParseStatement(WrappedList<MapStatementClause> clauses) => ParseStatementMethod.Invoke(Src, new object[] { clauses?.Src });

        private static FastMethod LoadSound3DListMethod;
        /// <summary>
        /// 3D サウンドのリストを読み込みます。
        /// </summary>
        /// <param name="filePath">リストファイルのパス。</param>
        public void LoadSound3DList(string filePath) => LoadSound3DListMethod.Invoke(Src, new object[] { filePath });

        private static FastMethod LoadSoundListMethod;
        /// <summary>
        /// サウンドリストを読み込みます。
        /// </summary>
        /// <param name="filePath">リストファイルのパス。</param>
        public void LoadSoundList(string filePath) => LoadSoundListMethod.Invoke(Src, new object[] { filePath });

        private static FastMethod LoadSignalListMethod;
        /// <summary>
        /// 信号現示リストを読み込みます。
        /// </summary>
        /// <param name="filePath">リストファイルのパス。</param>
        public void LoadSignalList(string filePath) => LoadSignalListMethod.Invoke(Src, new object[] { filePath });

        private static FastMethod LoadStructureListMethod;
        /// <summary>
        /// ストラクチャーリストを読み込みます。
        /// </summary>
        /// <param name="filePath">リストファイルのパス。</param>
        public void LoadStructureList(string filePath) => LoadStructureListMethod.Invoke(Src, new object[] { filePath });

        private static FastMethod LoadStationListMethod;
        /// <summary>
        /// 停車場リストを読み込みます。
        /// </summary>
        /// <param name="filePath">リストファイルのパス。</param>
        public void LoadStationList(string filePath) => LoadStationListMethod.Invoke(Src, new object[] { filePath });

        private static FastMethod IncludeMethod;
        /// <summary>
        /// 他のマップファイルの内容を挿入します。
        /// </summary>
        /// <param name="filePath">挿入するマップファイルのパス。</param>
        public void Include(string filePath) => IncludeMethod.Invoke(Src, new object[] { filePath });
    }
}
