using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;
using UnembeddedResources;

using AtsEx.Native.Ats;
using AtsEx.Plugins;
using AtsEx.Troubleshooting;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Input.Native;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Native.InputDevices
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

        private readonly CallerInfo CallerInfo;
        private readonly TroubleshooterSet Troubleshooters;

        private AtsEx.AsInputDevice AtsEx = null;
        private ScenarioService.AsInputDevice ScenarioService = null;
        private FrameSpan FrameSpan = null;

        private PluginSourceSet LoadedVehiclePluginUsing = null;
        private VehicleConfig LoadedVehicleConfig = null;

        public InputDeviceMain(CallerInfo callerInfo)
        {
            CallerInfo = callerInfo;

            AppInitializer.Initialize(CallerInfo, LaunchMode.InputDevice);

            if (Application.OpenForms.Count > 1)
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

            AtsMain.LoadedAsInputDevice();
            AtsMain.VehiclePluginUsingLoaded += (sender, e) =>
            {
                LoadedVehiclePluginUsing = e.VehiclePluginUsing;
                LoadedVehicleConfig = e.VehicleConfig;
            };


            PatchInvokationResult OnCreateDirectXDevices(object sender, PatchInvokedEventArgs e)
            {
                createDirectXDevicesPatch.Invoked -= OnCreateDirectXDevices;

                InitializeAtsEx();
                return new PatchInvokationResult(SkipModes.Continue);
            }

            void InitializeAtsEx()
            {
                AtsEx = new AtsEx.AsInputDevice(bveTypes);

                AtsEx.ScenarioClosed += OnScenarioClosed;
                AtsEx.OnSetVehicleSpec += OnSetVehicleSpec;
                AtsEx.OnInitialize += OnInitialize;
                AtsEx.PostElapse += PostElapse;
                AtsEx.OnSetPower += OnSetPower;
                AtsEx.OnSetBrake += OnSetBrake;
                AtsEx.OnSetReverser += OnSetReverser;
                AtsEx.OnKeyDown += OnKeyDown;
                AtsEx.OnKeyUp += OnKeyUp;
                AtsEx.OnHornBlow += OnHornBlow;
                AtsEx.OnDoorOpen += OnDoorOpen;
                AtsEx.OnDoorClose += OnDoorClose;
                AtsEx.OnSetSignal += OnSetSignal;
                AtsEx.OnSetBeaconData += OnSetBeaconData;
            }
        }

        private void OnSetVehicleSpec(object sender, AtsEx.AsInputDevice.ValueEventArgs<VehicleSpec> e)
        {
            PluginHost.Native.VehicleSpec exVehicleSpec = new PluginHost.Native.VehicleSpec(
                e.Value.BrakeNotches, e.Value.PowerNotches, e.Value.AtsNotch, e.Value.B67Notch, e.Value.Cars);

            string vehiclePath = AtsEx.BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path;
            VehicleConfig vehicleConfig = LoadedVehicleConfig ?? VehicleConfig.Resolve(vehiclePath);
            PluginSourceSet pluginUsing = !(LoadedVehiclePluginUsing is null) ? LoadedVehiclePluginUsing
                : vehicleConfig.PluginUsingPath is null ? PluginSourceSet.ResolvePluginUsingToLoad(PluginType.VehiclePlugin, true, vehiclePath)
                : PluginSourceSet.FromPluginUsing(PluginType.VehiclePlugin, true, vehicleConfig.PluginUsingPath);

            ScenarioService = new ScenarioService.AsInputDevice(AtsEx, pluginUsing, vehicleConfig, exVehicleSpec);
            FrameSpan = new FrameSpan();
        }

        private void OnInitialize(object sender, AtsEx.AsInputDevice.ValueEventArgs<DefaultBrakePosition> e)
        {
            ScenarioService.Started((BrakePosition)e.Value);
            FrameSpan.Initialize();
        }

        private void PostElapse(object sender, AtsEx.AsInputDevice.OnElapseEventArgs e)
        {
            ScenarioService?.PreviewTick();

            TimeSpan now = TimeSpan.FromMilliseconds(e.VehicleState.Time);
            TimeSpan elapsed = FrameSpan.Tick(now);

            PluginHost.Native.VehicleState exVehicleState = new PluginHost.Native.VehicleState(
                e.VehicleState.Location, e.VehicleState.Speed, TimeSpan.FromMilliseconds(e.VehicleState.Time),
                e.VehicleState.BcPressure, e.VehicleState.MrPressure, e.VehicleState.ErPressure, e.VehicleState.BpPressure, e.VehicleState.SapPressure, e.VehicleState.Current);

            AtsEx.Tick(elapsed);
            _ = ScenarioService?.Tick(elapsed, exVehicleState, e.Panel, e.Sound);

            ScenarioService?.PostTick();
        }

        private void OnSetPower(object sender, AtsEx.AsInputDevice.ValueEventArgs<int> e)
        {
            ScenarioService?.SetPower(e.Value, true);
        }

        private void OnSetBrake(object sender, AtsEx.AsInputDevice.ValueEventArgs<int> e)
        {
            ScenarioService?.SetBrake(e.Value, true);
        }

        private void OnSetReverser(object sender, AtsEx.AsInputDevice.ValueEventArgs<int> e)
        {
            ScenarioService?.SetReverser((ReverserPosition)e.Value, true);
        }

        private void OnKeyDown(object sender, AtsEx.AsInputDevice.ValueEventArgs<ATSKeys> e)
        {
            ScenarioService?.KeyDown((NativeAtsKeyName)e.Value);
        }

        private void OnKeyUp(object sender, AtsEx.AsInputDevice.ValueEventArgs<ATSKeys> e)
        {
            ScenarioService?.KeyUp((NativeAtsKeyName)e.Value);
        }

        private void OnHornBlow(object sender, AtsEx.AsInputDevice.ValueEventArgs<HornType> e)
        {
            ScenarioService?.HornBlow((PluginHost.Native.HornType)e.Value);
        }

        private void OnDoorOpen(object sender, EventArgs e)
        {
            ScenarioService?.DoorOpened();
        }

        private void OnDoorClose(object sender, EventArgs e)
        {
            ScenarioService?.DoorClosed();
        }

        private void OnSetSignal(object sender, AtsEx.AsInputDevice.ValueEventArgs<int> e)
        {
            ScenarioService?.SetSignal(e.Value);
        }

        private void OnSetBeaconData(object sender, AtsEx.AsInputDevice.ValueEventArgs<BeaconData> e)
        {
            BeaconPassedEventArgs args = new BeaconPassedEventArgs(e.Value.Num, e.Value.Sig, e.Value.Z, e.Value.Data);
            ScenarioService?.BeaconPassed(args);
        }

        private void OnScenarioClosed(object sender, AtsEx.AsInputDevice.ValueEventArgs<Scenario> e)
        {
            // Scenario クラスのデストラクタ由来の場合
            if (e.Value != ScenarioService?.Target) return;

            ScenarioService?.Dispose();
            ScenarioService = null;

            LoadedVehiclePluginUsing = null;
            LoadedVehicleConfig = null;
        }

        public void Dispose()
        {
            ScenarioService?.Dispose();
            AtsEx?.Dispose();
            Troubleshooters?.Dispose();
        }

        public void Configure(IWin32Window owner) => AtsEx.VersionFormProvider.ShowForm();

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
