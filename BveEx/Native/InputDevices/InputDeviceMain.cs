using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;
using UnembeddedResources;

using BveEx.Plugins;
using BveEx.Troubleshooting;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Native.InputDevices
{
    public class InputDeviceMain : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<InputDeviceMain>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> RequiresReboot { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> LaunchingFailed { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static InputDeviceMain()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly TroubleshooterSet Troubleshooters;

        private BveEx BveEx = null;
        private ScenarioService ScenarioService = null;
        private FrameSpan FrameSpan = null;

        public InputDeviceMain(CallerInfo callerInfo)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            App.CreateInstance(callerInfo.Process, callerInfo.BveAssembly, callerInfo.LauncherAssembly, executingAssembly);

            if (Application.OpenForms.Count > 0)
            {
                string confirmMessage = string.Format(Resources.Value.RequiresReboot.Value, App.Instance.ProductShortName);
                if (MessageBox.Show(confirmMessage, App.Instance.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Application.OpenForms[0].Close();
                }
                else
                {
                    ErrorDialog.Show(4, string.Format(Resources.Value.LaunchingFailed.Value, App.Instance.ProductShortName));
                }

                return;
            }

            Troubleshooters = TroubleshooterSet.Load();

            BveTypeSetLoader bveTypesLoader = new BveTypeSetLoader();
            bveTypesLoader.DifferentVersionProfileLoaded += (sender, e) =>
            {
                // TODO
            };

            BveTypeSet bveTypes = bveTypesLoader.Load();

            ClassMemberSet mainFormMembers = bveTypes.GetClassInfoOf<MainForm>();
            FastMethod createDirectXDevicesMethod = mainFormMembers.GetSourceMethodOf(nameof(MainForm.CreateDirectXDevices));

            HarmonyPatch createDirectXDevicesPatch = HarmonyPatch.Patch(nameof(InputDeviceMain), createDirectXDevicesMethod.Source, PatchType.Prefix);
            createDirectXDevicesPatch.Invoked += OnCreateDirectXDevices;


            PatchInvokationResult OnCreateDirectXDevices(object sender, PatchInvokedEventArgs e)
            {
                createDirectXDevicesPatch.Invoked -= OnCreateDirectXDevices;

                InitializeBveEx();
                return new PatchInvokationResult(SkipModes.Continue);
            }

            void InitializeBveEx()
            {
                BveEx = new BveEx(bveTypes);

                BveEx.ScenarioClosed += OnScenarioClosed;
                BveEx.OnLoad += OnLoad;
                BveEx.OnInitialize += OnInitialize;
                BveEx.OnElapse += OnElapse;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            string vehiclePath = BveEx.BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path;
            PluginSourceSet vehiclePluginUsing = PluginSourceSet.ResolvePluginUsingToLoad(PluginType.VehiclePlugin, true, vehiclePath);

            ScenarioService = new ScenarioService(BveEx, vehiclePluginUsing);
            FrameSpan = new FrameSpan();
        }

        private void OnInitialize(object sender, EventArgs e)
        {
            FrameSpan.Initialize();
        }

        private void OnElapse(object sender, BveEx.ValueEventArgs<TimeSpan> e)
        {
            ScenarioService?.PreviewTick();

            TimeSpan now = e.Value;
            TimeSpan elapsed = FrameSpan.Tick(now);

            BveEx.Tick(elapsed);
            ScenarioService?.Tick(elapsed);

            ScenarioService?.PostTick();
        }

        private void OnScenarioClosed(object sender, BveEx.ValueEventArgs<Scenario> e)
        {
            // Scenario クラスのデストラクタ由来の場合
            if (e.Value != ScenarioService?.Target) return;

            ScenarioService?.Dispose();
            ScenarioService = null;
        }

        public void Dispose()
        {
            ScenarioService?.Dispose();
            BveEx?.Dispose();
            Troubleshooters?.Dispose();
        }

        public void Configure(IWin32Window owner) => BveEx.VersionFormProvider.ShowForm();

        public void Load(string settingsPath)
        {
        }

        public void SetAxisRanges(int[][] ranges)
        {
        }

        public void Tick()
        {
        }
    }
}
