using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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
using AtsEx.Launching;

namespace AtsEx
{
    internal partial class AtsEx : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<AtsEx>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExAssemblyLocationIllegalMessage { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExAssemblyLocationIllegalApproach { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ConflictedMessage { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ConflictedApproach { get; private set; }
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
        private readonly LaunchModeManager LaunchModeManager;

        private bool IsFirstLoad;

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
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            IsFirstLoad = 2 < commandLineArgs.Length && !string.IsNullOrWhiteSpace(commandLineArgs[2]);

            BveHacker = new BveHacker(bveTypes);
            BveHacker.ScenarioCreated += OnScenarioCreated;
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;

            ClassMemberSet mainFormMembers = BveHacker.BveTypes.GetClassInfoOf<MainForm>();
            ClassMemberSet scenarioMembers = BveHacker.BveTypes.GetClassInfoOf<Scenario>();
            ClassMemberSet atsPluginMembers = BveHacker.BveTypes.GetClassInfoOf<AtsPlugin>();

            Patches = new PatchSet(mainFormMembers, scenarioMembers, atsPluginMembers);
            ListenPatchEvents();

            Extensions = ExtensionSetFactory.Load(BveHacker);
            ExtensionService = new ExtensionService(Extensions);

            VersionFormProvider = CreateVersionFormProvider(Extensions);

            ClassMemberSet scenarioSelectionFormMembers = bveTypes.GetClassInfoOf<ScenarioSelectionForm>();
            MethodInfo saveSettingsMethod = scenarioSelectionFormMembers.GetSourceMethodOf(nameof(ScenarioSelectionForm.SaveSettings)).Source;
            LaunchModeManager = new LaunchModeManager(BveHacker.MainForm, BveHacker.ScenarioSelectionForm, saveSettingsMethod);
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            IsFirstLoad = false;
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
                if (IsFirstLoad)
                {
                    ErrorDialog.Show(5, Resources.Value.ConflictedMessage.Value, Resources.Value.ConflictedApproach.Value);
                    throw new InvalidOperationException(Resources.Value.ConflictedMessage.Value);
                }

                LaunchModeManager.RestartAsNormalMode(BveHacker.ScenarioInfo?.Path);
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