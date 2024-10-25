using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using TypeWrapping;
using UnembeddedResources;

using AtsEx.Diagnostics;
using AtsEx.Extensions.ContextMenuHacker;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

using AtsEx.Native;
using AtsEx.Plugins.Extensions;

namespace AtsEx
{
    internal partial class AtsEx : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<AtsEx>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExAssemblyLocationIllegalMessage { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExAssemblyLocationIllegalApproach { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> IgnoreAndContinue { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ManualDisposeHeader { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ManualDisposeMessage { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static AtsEx()
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

        public AtsEx(BveTypeSet bveTypes)
        {
            BveHacker = new BveHacker(bveTypes);

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