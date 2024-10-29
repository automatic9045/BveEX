using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SlimDX.DirectSound;

using Mackoy.Bvets;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// メインのフォームを表します。
    /// </summary>
    public class MainForm : ClassWrapperBase, IDrawable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MainForm>();

            DirectSoundField = members.GetSourceFieldOf(nameof(DirectSound));
            ScenarioSelectFormField = members.GetSourceFieldOf(nameof(ScenarioSelectForm));
            TimePosFormField = members.GetSourceFieldOf(nameof(TimePosForm));
            ChartFormField = members.GetSourceFieldOf(nameof(ChartForm));
            LoadingProgressFormField = members.GetSourceFieldOf(nameof(LoadingProgressForm));
            AssistantDrawerField = members.GetSourceFieldOf(nameof(AssistantDrawer));
            InputManagerField = members.GetSourceFieldOf(nameof(InputManager));
            CurrentScenarioField = members.GetSourceFieldOf(nameof(CurrentScenario));
            CurrentScenarioInfoField = members.GetSourceFieldOf(nameof(CurrentScenarioInfo));
            PreferencesField = members.GetSourceFieldOf(nameof(Preferences));
            ContextMenuField = members.GetSourceFieldOf(nameof(ContextMenu));

            CreateDirectXDevicesMethod = members.GetSourceMethodOf(nameof(CreateDirectXDevices));
            OpenScenarioMethod = members.GetSourceMethodOf(nameof(OpenScenario));
            LoadScenarioMethod = members.GetSourceMethodOf(nameof(LoadScenario));
            UnloadScenarioMethod = members.GetSourceMethodOf(nameof(UnloadScenario));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            OnDeviceLostMethod = members.GetSourceMethodOf(nameof(OnDeviceLost));
            OnDeviceResetMethod = members.GetSourceMethodOf(nameof(OnDeviceReset));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MainForm"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MainForm(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MainForm"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MainForm FromSource(object src) => src is null ? null : new MainForm(src);


        private static FastField DirectSoundField;
        /// <summary>
        /// BVE が使用する DirectSound デバイスを取得・設定します。
        /// </summary>
        public DirectSound DirectSound
        {
            get => DirectSoundField.GetValue(Src);
            set => DirectSoundField.SetValue(Src, value);
        }

        private static FastField ScenarioSelectFormField;
        /// <summary>
        /// 「シナリオの選択」フォームを取得します。
        /// </summary>
        public ScenarioSelectionForm ScenarioSelectForm => ClassWrappers.ScenarioSelectionForm.FromSource(ScenarioSelectFormField.GetValue(Src));

        private static FastField TimePosFormField;
        /// <summary>
        /// 「時刻と位置」フォームを取得します。
        /// </summary>
        public TimePosForm TimePosForm => ClassWrappers.TimePosForm.FromSource(TimePosFormField.GetValue(Src));

        private static FastField ChartFormField;
        /// <summary>
        /// 「車両物理量」フォームを取得します。
        /// </summary>
        public ChartForm ChartForm => ClassWrappers.ChartForm.FromSource(ChartFormField.GetValue(Src));

        private static FastField LoadingProgressFormField;
        /// <summary>
        /// 「シナリオを読み込んでいます...」フォームを取得します。
        /// </summary>
        public LoadingProgressForm LoadingProgressForm => ClassWrappers.LoadingProgressForm.FromSource(LoadingProgressFormField.GetValue(Src));

        private static FastField AssistantDrawerField;
        /// <summary>
        /// 補助表示を描画する <see cref="ClassWrappers.AssistantDrawer"/> を取得します。
        /// </summary>
        public AssistantDrawer AssistantDrawer => ClassWrappers.AssistantDrawer.FromSource(AssistantDrawerField.GetValue(Src));

        private static FastField InputManagerField;
        /// <summary>
        /// キー入力を管理する <see cref="ClassWrappers.InputManager"/> を取得・設定します。
        /// </summary>
        public InputManager InputManager
        {
            get => ClassWrappers.InputManager.FromSource(InputManagerField.GetValue(Src));
            set => InputManagerField.SetValue(Src, value?.Src);
        }

        private static FastField CurrentScenarioField;
        /// <summary>
        /// 現在のシナリオのインスタンスを取得・設定します。
        /// </summary>
        public Scenario CurrentScenario
        {
            get => Scenario.FromSource(CurrentScenarioField.GetValue(Src));
            set => CurrentScenarioField.SetValue(Src, value?.Src);
        }

        private static FastField CurrentScenarioInfoField;
        /// <summary>
        /// 現在のシナリオの <see cref="ScenarioInfo"/> を取得・設定します。
        /// </summary>
        public ScenarioInfo CurrentScenarioInfo
        {
            get => ScenarioInfo.FromSource(CurrentScenarioInfoField.GetValue(Src));
            set => CurrentScenarioInfoField.SetValue(Src, value?.Src);
        }

        private static FastField PreferencesField;
        /// <summary>
        /// BVE の設定が格納された <see cref="Mackoy.Bvets.Preferences"/> を取得・設定します。
        /// </summary>
        public Preferences Preferences
        {
            get => PreferencesField.GetValue(Src);
            set => PreferencesField.SetValue(Src, value);
        }

        private static FastField ContextMenuField;
        /// <summary>
        /// 右クリックで表示されるショートカット メニューを取得・設定します。
        /// </summary>
        public ContextMenuStrip ContextMenu
        {
            get => ContextMenuField.GetValue(Src);
            set => ContextMenuField.SetValue(Src, value);
        }


        private static FastMethod CreateDirectXDevicesMethod;
        /// <summary>
        /// シナリオを閉じます。
        /// </summary>
        public void CreateDirectXDevices() => CreateDirectXDevicesMethod.Invoke(Src, null);

        private static FastMethod OpenScenarioMethod;
        /// <summary>
        /// シナリオを開き、読み込みます。既に開かれているシナリオがあれば閉じられます。
        /// </summary>
        /// <param name="scenarioFilePath">シナリオファイルのパス。<see langword="null"/> を指定すると、「シナリオの選択」フォームが表示されます。</param>
        public void OpenScenario(string scenarioFilePath) => OpenScenarioMethod.Invoke(Src, new object[] { scenarioFilePath });

        private static FastMethod LoadScenarioMethod;
        /// <summary>
        /// シナリオを読み込みます。
        /// </summary>
        /// <param name="scenarioInfo">シナリオを指定する <see cref="ScenarioInfo"/>。</param>
        /// <param name="skipReloadStructures">ストラクチャーの読込をスキップするかどうか。<see langword="true"/> を指定した場合、現時点で読み込まれているストラクチャーを流用します。</param>
        public void LoadScenario(ScenarioInfo scenarioInfo, bool skipReloadStructures) => LoadScenarioMethod.Invoke(Src, new object[] { scenarioInfo?.Src, skipReloadStructures });

        private static FastMethod UnloadScenarioMethod;
        /// <summary>
        /// シナリオを閉じます。
        /// </summary>
        public void UnloadScenario() => UnloadScenarioMethod.Invoke(Src, null);

        private static FastMethod DrawMethod;
        /// <inheritdoc/>
        public void Draw() => DrawMethod.Invoke(Src, null);

        private static FastMethod OnDeviceLostMethod;
        /// <inheritdoc/>
        public void OnDeviceLost() => OnDeviceLostMethod.Invoke(Src, null);

        private static FastMethod OnDeviceResetMethod;
        /// <inheritdoc/>
        public void OnDeviceReset() => OnDeviceResetMethod.Invoke(Src, null);
    }
}
