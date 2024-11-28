using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mackoy.Bvets;

using BveTypes;
using BveTypes.ClassWrappers;

namespace BveEx.PluginHost
{
    /// <summary>
    /// <see cref="IBveHacker.ScenarioOpened"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベント データを格納している <see cref="ScenarioOpenedEventArgs"/>。</param>
    public delegate void ScenarioOpenedEventHandler(ScenarioOpenedEventArgs e);

    /// <summary>
    /// <see cref="IBveHacker.ScenarioClosed"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベント データを格納している <see cref="EventArgs"/>。</param>
    public delegate void ScenarioClosedEventHandler(EventArgs e);

    /// <summary>
    /// <see cref="IBveHacker.ScenarioCreated"/> または <see cref="IBveHacker.PreviewScenarioCreated"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベント データを格納している <see cref="ScenarioCreatedEventArgs"/>。</param>
    public delegate void ScenarioCreatedEventHandler(ScenarioCreatedEventArgs e);

    /// <summary>
    /// 本来 ATS プラグインからは利用できない BVE 本体の諸機能へアクセスするための機能を提供します。
    /// </summary>
    public interface IBveHacker : IDisposable
    {
        /// <summary>
        /// クラスラッパーに対応する BVE の型とメンバーの定義を表す <see cref="BveTypeSet"/> を取得します。
        /// </summary>
        BveTypeSet BveTypes { get; }


        /// <summary>
        /// BVE のメインフォームのハンドルを取得します。
        /// </summary>
        IntPtr MainFormHandle { get; }

        /// <summary>
        /// BVE のメインフォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form MainFormSource { get; }

        /// <summary>
        /// BVE のメインフォームを取得します。
        /// </summary>
        MainForm MainForm { get; }


        /// <summary>
        /// BVE の「設定」フォームが表示できる状態にあるかどうかを取得します。
        /// </summary>
        bool IsConfigFormReady { get; }

        /// <summary>
        /// BVE の「設定」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        /// <remarks>
        /// 表示中以外は <see langword="null"/> を返します。また、再表示する度に異なるインスタンスとなるため注意してください。
        /// </remarks>
        Form ConfigFormSource { get; }

        /// <summary>
        /// BVE の「設定」フォームを取得します。
        /// </summary>
        /// <remarks>
        /// 表示中以外は <see langword="null"/> を返します。また、再表示する度に異なるインスタンスとなるため注意してください。
        /// </remarks>
        ConfigForm ConfigForm { get; }


        /// <summary>
        /// BVE の「シナリオの選択」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form ScenarioSelectionFormSource { get; }

        /// <summary>
        /// BVE の「シナリオの選択」フォームを取得します。
        /// </summary>
        ScenarioSelectionForm ScenarioSelectionForm { get; }

        /// <summary>
        /// BVE の「シナリオを読み込んでいます...」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form LoadingProgressFormSource { get; }

        /// <summary>
        /// BVE の「シナリオを読み込んでいます...」フォームを取得します。
        /// </summary>
        LoadingProgressForm LoadingProgressForm { get; }

        /// <summary>
        /// BVE の「時刻と位置」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form TimePosFormSource { get; }

        /// <summary>
        /// BVE の「時刻と位置」フォームを取得します。
        /// </summary>
        TimePosForm TimePosForm { get; }

        /// <summary>
        /// BVE の「車両物理量」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form ChartFormSource { get; }

        /// <summary>
        /// BVE の「車両物理量」フォームを取得します。
        /// </summary>
        ChartForm ChartForm { get; }


        /// <summary>
        /// BVE の設定が格納された <see cref="Mackoy.Bvets.Preferences"/> を取得します。
        /// </summary>
        Preferences Preferences { get; }

        /// <summary>
        /// キー入力を管理する <see cref="BveTypes.ClassWrappers.InputManager"/> を取得します。
        /// </summary>
        InputManager InputManager { get; }


        /// <summary>
        /// 現在実行中のシナリオの読込に使用されている <see cref="BveTypes.ClassWrappers.MapLoader"/> を取得します。
        /// </summary>
        /// <remarks>
        /// シナリオが読み込まれていない時は <see langword="null"/> になります。
        /// </remarks>
        MapLoader MapLoader { get; }


        /// <summary>
        /// シナリオが選択され、読込を開始したときに発生します。
        /// </summary>
        /// <remarks>
        /// 読み込まれたシナリオを表す <see cref="BveTypes.ClassWrappers.Scenario"/> を取得するには、<see cref="ScenarioCreated"/> または <see cref="PreviewScenarioCreated"/> イベントを使用してください。
        /// </remarks>
        event ScenarioOpenedEventHandler ScenarioOpened;

        /// <summary>
        /// シナリオが閉じられたときに発生します。
        /// </summary>
        event ScenarioClosedEventHandler ScenarioClosed;


        /// <summary>
        /// <see cref="ScenarioCreated"/> が発生する直前に通知します。特に理由がなければ <see cref="ScenarioCreated"/> を使用してください。
        /// </summary>
        event ScenarioCreatedEventHandler PreviewScenarioCreated;

        /// <summary>
        /// <see cref="BveTypes.ClassWrappers.Scenario"/> のインスタンスが生成されたときに通知します。
        /// </summary>
        event ScenarioCreatedEventHandler ScenarioCreated;

        /// <summary>
        /// 現在読込中または実行中のシナリオの情報を取得します。
        /// </summary>
        ScenarioInfo ScenarioInfo { get; }

        /// <summary>
        /// 現在実行中のシナリオを取得します。シナリオの読込中は <see cref="InvalidOperationException"/> をスローします。
        /// シナリオの読込中に <see cref="BveTypes.ClassWrappers.Scenario"/> を取得するには <see cref="ScenarioCreated"/> イベントを購読してください。
        /// </summary>
        Scenario Scenario { get; }

        /// <summary>
        /// <see cref="Scenario"/> が取得可能かどうかを取得します。
        /// </summary>
        bool IsScenarioCreated { get; }
    }
}
