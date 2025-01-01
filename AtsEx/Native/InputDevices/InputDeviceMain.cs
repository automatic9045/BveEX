using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly TroubleshooterSet Troubleshooters;

        private AtsEx AtsEx = null;
        private ScenarioService ScenarioService = null;
        private FrameSpan FrameSpan = null;

        public InputDeviceMain(CallerInfo callerInfo)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            App.CreateInstance(callerInfo.Process, callerInfo.BveAssembly, callerInfo.AtsExLauncherAssembly, executingAssembly);

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

                InitializeAtsEx();
                return new PatchInvokationResult(SkipModes.Continue);
            }

            void InitializeAtsEx()
            {
                AtsEx = new AtsEx(bveTypes);

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

        private void OnSetVehicleSpec(object sender, AtsEx.ValueEventArgs<VehicleSpec> e)
        {
            PluginHost.Native.VehicleSpec exVehicleSpec = new PluginHost.Native.VehicleSpec(
                e.Value.BrakeNotches, e.Value.PowerNotches, e.Value.AtsNotch, e.Value.B67Notch, e.Value.Cars);

            VehicleConfig callerLinkedConfig = null;
            try
            {
                if (File.Exists(Launcher.CoreHost.VehicleConfigPath))
                {
                    callerLinkedConfig = VehicleConfig.LoadFrom(Launcher.CoreHost.VehicleConfigPath);
                }
            }
            catch { }

            PluginSourceSet callerLinkedPluginUsing = null;
            try
            {
                if (File.Exists(Launcher.CoreHost.VehiclePluginUsingPath))
                {
                    callerLinkedPluginUsing = PluginSourceSet.FromPluginUsing(PluginType.VehiclePlugin, false, Launcher.CoreHost.VehiclePluginUsingPath);
                }
            }
            catch { }

            string vehiclePath = AtsEx.BveHacker.ScenarioInfo.VehicleFiles.SelectedFile.Path;
            VehicleConfig vehicleConfig = callerLinkedConfig ?? VehicleConfig.Resolve(vehiclePath);
            PluginSourceSet pluginUsing = !(callerLinkedPluginUsing is null) ? callerLinkedPluginUsing
                : vehicleConfig.PluginUsingPath is null ? PluginSourceSet.ResolvePluginUsingToLoad(PluginType.VehiclePlugin, true, vehiclePath)
                : PluginSourceSet.FromPluginUsing(PluginType.VehiclePlugin, true, vehicleConfig.PluginUsingPath);

            ScenarioService = new ScenarioService(AtsEx, pluginUsing, vehicleConfig, exVehicleSpec);
            FrameSpan = new FrameSpan();
        }

        private void OnInitialize(object sender, AtsEx.ValueEventArgs<DefaultBrakePosition> e)
        {
            ScenarioService.Started((BrakePosition)e.Value);
            FrameSpan.Initialize();
        }

        private void PostElapse(object sender, AtsEx.OnElapseEventArgs e)
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

        private void OnSetPower(object sender, AtsEx.ValueEventArgs<int> e)
        {
            ScenarioService?.SetPower(e.Value, true);
        }

        private void OnSetBrake(object sender, AtsEx.ValueEventArgs<int> e)
        {
            ScenarioService?.SetBrake(e.Value, true);
        }

        private void OnSetReverser(object sender, AtsEx.ValueEventArgs<int> e)
        {
            ScenarioService?.SetReverser((ReverserPosition)e.Value, true);
        }

        private void OnKeyDown(object sender, AtsEx.ValueEventArgs<ATSKeys> e)
        {
            ScenarioService?.KeyDown((NativeAtsKeyName)e.Value);
        }

        private void OnKeyUp(object sender, AtsEx.ValueEventArgs<ATSKeys> e)
        {
            ScenarioService?.KeyUp((NativeAtsKeyName)e.Value);
        }

        private void OnHornBlow(object sender, AtsEx.ValueEventArgs<HornType> e)
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

        private void OnSetSignal(object sender, AtsEx.ValueEventArgs<int> e)
        {
            ScenarioService?.SetSignal(e.Value);
        }

        private void OnSetBeaconData(object sender, AtsEx.ValueEventArgs<BeaconData> e)
        {
            BeaconPassedEventArgs args = new BeaconPassedEventArgs(e.Value.Num, e.Value.Sig, e.Value.Z, e.Value.Data);
            ScenarioService?.BeaconPassed(args);
        }

        private void OnScenarioClosed(object sender, AtsEx.ValueEventArgs<Scenario> e)
        {
            // Scenario クラスのデストラクタ由来の場合
            if (e.Value != ScenarioService?.Target) return;

            ScenarioService?.Dispose();
            ScenarioService = null;
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
