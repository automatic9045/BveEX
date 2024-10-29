using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mackoy.Bvets;

using BveTypes;
using BveTypes.ClassWrappers;
using UnembeddedResources;

using BveEx.BveHackerServices;
using BveEx.Handles;

using BveEx.PluginHost;
using BveEx.PluginHost.LoadErrorManager;

namespace BveEx
{
    internal sealed class BveHacker : IBveHacker, IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<BveHacker>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotGetScenario { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> NoPluginLoaded { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static BveHacker()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly StructureSetLifeProlonger StructureSetLifeProlonger;

        public BveHacker(BveTypeSet bveTypes)
        {
            BveTypes = bveTypes;

            MainFormHacker = new MainFormHacker(App.Instance.Process);
            ConfigFormHacker = new ConfigFormHacker(BveTypes);
            MapLoaderHacker = new MapLoaderHacker(BveTypes);
            ScenarioHacker = new ScenarioHacker(MainFormHacker, BveTypes);

            StructureSetLifeProlonger = new StructureSetLifeProlonger(this);

            LoadErrorManager = new LoadErrorManager.LoadErrorManager(LoadingProgressForm);

            ScenarioHacker.ScenarioCreated += e =>
            {
                try
                {
                    PreviewScenarioCreated?.Invoke(e);
                    OnScenarioCreated(e);
                    ScenarioCreated?.Invoke(e);
                }
                catch (BveFileLoadException ex)
                {
                    LoadErrorManager.Throw(ex.Message, ex.SenderFileName, ex.LineIndex, ex.CharIndex);
                }
            };

            ScenarioHacker.ScenarioOpened += e =>
            {
                ScenarioHacker.BeginObserveInitialization();

                ScenarioOpened?.Invoke(e);
            };

            ScenarioHacker.ScenarioClosed += e =>
            {
                MapLoaderHacker.Clear();

                ScenarioClosed?.Invoke(e);
            };
        }

        public void Dispose()
        {
            StructureSetLifeProlonger.Dispose();
            MapLoaderHacker.Dispose();
            ScenarioHacker.Dispose();
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            NotchInfo notchInfo = e.Scenario.Vehicle.Instruments.Cab.Handles.NotchInfo;

            BrakeHandle brake = BrakeHandle.FromNotchInfo(notchInfo);
            PowerHandle power = PowerHandle.FromNotchInfo(notchInfo);
            Reverser reverser = new Reverser();

            Handles = new PluginHost.Handles.HandleSet(brake, power, reverser);

            AtsPlugin atsPlugin = e.Scenario.Vehicle.Instruments.AtsPlugin;
            if (!atsPlugin.IsPluginLoaded)
            {
                string fileName = Path.GetFileName(ScenarioInfo.VehicleFiles.SelectedFile.Path);
                throw new BveFileLoadException(string.Format(Resources.Value.NoPluginLoaded.Value, App.Instance.ProductShortName), fileName);
            }
        }


        public BveTypeSet BveTypes { get; }


        private readonly MainFormHacker MainFormHacker;
        public IntPtr MainFormHandle => MainFormHacker.TargetFormHandle;
        public Form MainFormSource => MainFormHacker.TargetFormSource;
        public MainForm MainForm => MainFormHacker.TargetForm;

        private readonly ConfigFormHacker ConfigFormHacker;
        public bool IsConfigFormReady => ConfigFormHacker.IsReady;
        public Form ConfigFormSource => ConfigFormHacker.FormSource;
        public ConfigForm ConfigForm => ConfigFormHacker.Form;

        public Form ScenarioSelectionFormSource => ScenarioSelectionForm.Src;
        public ScenarioSelectionForm ScenarioSelectionForm => MainForm.ScenarioSelectForm;
        public Form LoadingProgressFormSource => LoadingProgressForm.Src;
        public LoadingProgressForm LoadingProgressForm => MainForm.LoadingProgressForm;
        public Form TimePosFormSource => TimePosForm.Src;
        public TimePosForm TimePosForm => MainForm.TimePosForm;
        public Form ChartFormSource => ChartForm.Src;
        public ChartForm ChartForm => MainForm.ChartForm;

        public Preferences Preferences => MainForm.Preferences;
        public KeyProvider KeyProvider => MainForm.KeyProvider;

        public ILoadErrorManager LoadErrorManager { get; }

        public PluginHost.Handles.HandleSet Handles { get; private set; }

        private readonly MapLoaderHacker MapLoaderHacker;
        public MapLoader MapLoader => MapLoaderHacker.MapLoader;

        private readonly ScenarioHacker ScenarioHacker;

        public event ScenarioOpenedEventHandler ScenarioOpened;
        public event ScenarioClosedEventHandler ScenarioClosed;
        public event ScenarioCreatedEventHandler PreviewScenarioCreated;
        public event ScenarioCreatedEventHandler ScenarioCreated;

        public ScenarioInfo ScenarioInfo
        {
            get => ScenarioHacker.CurrentScenarioInfo;
            set => ScenarioHacker.CurrentScenarioInfo = value;
        }
        public Scenario Scenario => ScenarioHacker.CurrentScenario ?? throw new InvalidOperationException(string.Format(Resources.Value.CannotGetScenario.Value, nameof(Scenario)));

        public bool IsScenarioCreated => !(ScenarioHacker.CurrentScenario is null);
    }
}
