using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using TypeWrapping;
using UnembeddedResources;

using BveEx.Diagnostics;
using BveEx.Extensions.ContextMenuHacker;
using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

using BveEx.Launching;
using BveEx.Native;
using BveEx.Plugins.Extensions;

namespace BveEx
{
    internal partial class BveEx : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<BveEx>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ManualDisposeHeader { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ManualDisposeMessage { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static BveEx()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


        private readonly PatchSet Patches;
        private readonly ExtensionService ExtensionService;

        public event EventHandler<ValueEventArgs<ScenarioInfo>> ScenarioOpened;
        public event EventHandler<ValueEventArgs<Scenario>> ScenarioClosed;

        public event EventHandler<ValueEventArgs<VehicleSpec>> OnSetVehicleSpec;
        public event EventHandler<ValueEventArgs<DefaultBrakePosition>> OnInitialize;
        public event EventHandler<OnElapseEventArgs> PostElapse;
        public event EventHandler<ValueEventArgs<int>> OnSetPower;
        public event EventHandler<ValueEventArgs<int>> OnSetBrake;
        public event EventHandler<ValueEventArgs<int>> OnSetReverser;
        public event EventHandler<ValueEventArgs<ATSKeys>> OnKeyDown;
        public event EventHandler<ValueEventArgs<ATSKeys>> OnKeyUp;
        public event EventHandler<ValueEventArgs<HornType>> OnHornBlow;
        public event EventHandler OnDoorOpen;
        public event EventHandler OnDoorClose;
        public event EventHandler<ValueEventArgs<int>> OnSetSignal;
        public event EventHandler<ValueEventArgs<BeaconData>> OnSetBeaconData;

        public BveHacker BveHacker { get; }
        public IExtensionSet Extensions { get; }

        public VersionFormProvider VersionFormProvider { get; }

        public BveEx(BveTypeSet bveTypes)
        {
            BveHacker = new BveHacker(bveTypes);
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;

            ClassMemberSet mainFormMembers = BveHacker.BveTypes.GetClassInfoOf<MainForm>();
            ClassMemberSet scenarioMembers = BveHacker.BveTypes.GetClassInfoOf<Scenario>();
            ClassMemberSet atsPluginMembers = BveHacker.BveTypes.GetClassInfoOf<AtsPlugin>();

            Patches = new PatchSet(mainFormMembers, scenarioMembers, atsPluginMembers);
            ListenPatchEvents();

            Extensions = ExtensionSetFactory.Load(BveHacker);
            ExtensionService = new ExtensionService(Extensions);

            VersionFormProvider = CreateVersionFormProvider(Extensions);
        }

        private VersionFormProvider CreateVersionFormProvider(IEnumerable<PluginBase> extensions)
            => new VersionFormProvider(BveHacker.MainFormSource, extensions, Extensions.GetExtension<IContextMenuHacker>());

        public void Dispose()
        {
            AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceException;

            if (BveHacker.IsConfigFormReady)
            {
                string header = string.Format(Resources.Value.ManualDisposeHeader.Value, App.Instance.ProductShortName);
                string message = string.Format(Resources.Value.ManualDisposeMessage.Value, App.Instance.ProductShortName);
                ErrorDialogInfo dialogInfo = new ErrorDialogInfo(header, App.Instance.ProductShortName, message)
                {
                    HelpLink = new Uri("https://www.okaoka-depot.com/AtsEX.Docs/support/report/"),
                };

                Diagnostics.ErrorDialog.Show(dialogInfo);
            }

            Patches.Dispose();

            ((ExtensionSet)Extensions).SaveStates();

            VersionFormProvider.Dispose();
            ExtensionService.Dispose();
            BveHacker.Dispose();
        }

        private void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            if (e.Exception is LaunchModeException)
            {
                LaunchModeManager.RestartAsLegacyMode(BveHacker.ScenarioInfo?.Path);
            }
        }

        public void Tick(TimeSpan elapsed)
        {
            ExtensionService.Tick(elapsed);
        }


        internal class ValueEventArgs<T> : EventArgs
        {
            public T Value { get; }

            public ValueEventArgs(T value)
            {
                Value = value;
            }
        }

        internal class OnElapseEventArgs : EventArgs
        {
            public VehicleState VehicleState { get; }
            public int[] Panel { get; }
            public int[] Sound { get; }

            public OnElapseEventArgs(VehicleState vehicleState, int[] panel, int[] sound)
            {
                VehicleState = vehicleState;
                Panel = panel;
                Sound = sound;
            }
        }
    }
}