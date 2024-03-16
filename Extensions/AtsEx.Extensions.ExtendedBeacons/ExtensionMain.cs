using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;
using AtsEx.Scripting.CSharp;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IExtensionMain))]
    internal class ExtensionMain : AssemblyPluginBase, IExtensionMain
    {
        private ExtendedBeaconSet _ExtendedBeacons;
        public IExtendedBeaconSet ExtendedBeacons => _ExtendedBeacons;

        public ExtensionMain(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            try
            {
                _ExtendedBeacons = ExtendedBeaconSet.Load(Native, BveHacker, e.Scenario);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BveFileLoadException exception:
                        BveHacker.LoadErrorManager.Throw(exception.Message, exception.SenderFileName, exception.LineIndex, exception.CharIndex);
                        break;

                    case CompilationException exception:
                        foreach (Diagnostic diagnostic in exception.CompilationErrors)
                        {
                            string message = diagnostic.GetMessage();
                            string fileName = Path.GetFileName(diagnostic.Location.SourceTree.FilePath);

                            LinePosition position = diagnostic.Location.GetLineSpan().StartLinePosition;
                            int lineIndex = position.Line;
                            int charIndex = position.Character;

                            BveHacker.LoadErrorManager.Throw(message, fileName, lineIndex, charIndex);
                        }
                        break;

                    default:
                        BveHacker.LoadErrorManager.Throw(ex.Message);
                        _ = MessageBox.Show(ex.ToString(), App.Instance.ProductName);
                        break;
                }
            }
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            double location = BveHacker.Scenario.LocationManager.Location;
            int time = BveHacker.Scenario.TimeManager.TimeMilliseconds;
            double preTrainLocation = BveHacker.Scenario.Route.PreTrainObjects.GetPreTrainLocation(time);

            _ExtendedBeacons.Tick(location, preTrainLocation);

            return new ExtensionTickResult();
        }
    }
}
