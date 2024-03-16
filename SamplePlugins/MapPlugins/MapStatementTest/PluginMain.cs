using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Samples.MapPlugins.MapStatementTest
{
    [Plugin(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private static readonly string AssemblyFileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            IReadOnlyList<IHeader> headers = BveHacker.MapHeaders.GetAll(Identifiers.HeaderAlert);
            for (int i = 0; i < headers.Count; i++)
            {
                IHeader header = headers[i];
                MessageBox.Show($"ヘッダー {header.Name.FullName} が読み込まれました ({i}):\n\n{header.Argument}");
            }

            if (!headers.Any())
            {
                throw new BveFileLoadException($"ヘッダー {Identifiers.HeaderAlert.FullName} が見つかりませんでした。", AssemblyFileName);
            }

            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e2)
        {
            Train train = e2.Scenario.Trains["test"];

            IReadOnlyList<IStatement> putStatements = BveHacker.MapStatements.GetAll(Identifiers.Alert.Put);
            foreach (IStatement statement in putStatements)
            {
                if (statement.AdditionalDeclaration.Length != 1)
                {
                    throw new BveFileLoadException($"{statement.Name.FullName} ステートメントで、引数の長さが不正なものを検出しました。", AssemblyFileName);
                }

                Identifier alertType = statement.AdditionalDeclaration[0];
                if (alertType != Identifiers.AlertType.Basic && alertType != Identifiers.AlertType.ShowValue)
                {
                    throw new BveFileLoadException($"アラートの種類 {alertType.FullName} は不正です。", AssemblyFileName);
                }

                statement.ObserveTrain(train);

                statement.Entered += (sender, e) => OnPassed(statement, e.Direction, "0", "自列車", "通過");
                statement.PreTrainEntered += (sender, e) => OnPassed(statement, e.Direction, "0", "先行列車", "通過");
                statement.TrainEntered += (sender, e) => OnPassed(statement, e.Direction, e.SenderTrain.TrainInfo.TrackKey, $"他列車 (名前: '{e.SenderTrainName}') ", "通過");
            }

            IReadOnlyList<IStatement> putRangeStatements = BveHacker.MapStatements.GetAll(Identifiers.Alert.PutRange);
            foreach (IStatement statement in putRangeStatements)
            {
                if (statement.AdditionalDeclaration.Length != 0)
                {
                    throw new BveFileLoadException($"{statement.Name.FullName} ステートメントで、引数の長さが不正なものを検出しました。", AssemblyFileName);
                }

                statement.Entered += (sender, e) => OnPassed(statement, e.Direction, "0", "自列車", "進入");
                statement.PreTrainEntered += (sender, e) => OnPassed(statement, e.Direction, "0", "先行列車", "進入");
                statement.TrainEntered += (sender, e) => OnPassed(statement, e.Direction, e.SenderTrain.TrainInfo.TrackKey, $"他列車 (名前: '{e.SenderTrainName}') ", "進入");

                statement.Exited += (sender, e) => OnPassed(statement, e.Direction, "0", "自列車", "進出");
                statement.PreTrainExited += (sender, e) => OnPassed(statement, e.Direction, "0", "先行列車", "進出");
                statement.TrainExited += (sender, e) => OnPassed(statement, e.Direction, e.SenderTrain.TrainInfo.TrackKey, $"他列車 (名前: '{e.SenderTrainName}') ", "進出");
            }


            void OnPassed(IStatement statement, Direction targetTrainDirection, string targetTrainTrackKey, string targetTrainDescription, string actionDescription)
            {
                if (targetTrainTrackKey != statement.DefinedStructure.TrackKey) return;

                string positionDescription = statement.From == statement.To ? statement.From.ToString() : $"{statement.From} ～ {statement.To}";
                string detail = GenerateDetailText();

                string text = $"{targetTrainDescription}の{actionDescription}を検知しました。\n\n" +
                    $"進行方向：{targetTrainDirection}\n" +
                    $"配置位置：軌道 '{targetTrainTrackKey}'、距離程 {positionDescription}" +
                    detail;
                MessageBox.Show(text);


                string GenerateDetailText()
                {
                    if (statement.AdditionalDeclaration.Length == 0) return "";

                    Identifier alertType = statement.AdditionalDeclaration[0];
                    if (alertType == Identifiers.AlertType.Basic)
                    {
                        return "";
                    }
                    else if (alertType == Identifiers.AlertType.ShowValue)
                    {
                        Argument argument = Argument.FromStatement(statement);
                        double value = argument.GetParameter<double>("value");
                        return $"\n渡された値：{value}";
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        public override TickResult Tick(TimeSpan elapsed) => new MapPluginTickResult();
    }
}
