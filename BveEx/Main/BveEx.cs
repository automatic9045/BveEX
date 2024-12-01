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
        private readonly AtsPluginOverrider AtsPluginOverrider;
        private readonly ExtensionService ExtensionService;

        public event EventHandler<ValueEventArgs<ScenarioInfo>> ScenarioOpened;
        public event EventHandler<ValueEventArgs<Scenario>> ScenarioClosed;

        public event EventHandler OnLoad;
        public event EventHandler OnInitialize;
        public event EventHandler<ValueEventArgs<TimeSpan>> OnElapse;

        public BveHacker BveHacker { get; }
        public IExtensionSet Extensions { get; }

        public VersionFormProvider VersionFormProvider { get; }

        public BveEx(BveTypeSet bveTypes)
        {
            BveHacker = new BveHacker(bveTypes);
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;

            ClassMemberSet mainFormMembers = bveTypes.GetClassInfoOf<MainForm>();
            ClassMemberSet scenarioMembers = bveTypes.GetClassInfoOf<Scenario>();
            ClassMemberSet vehicleMembers = bveTypes.GetClassInfoOf<Vehicle>();
            ClassMemberSet atsPluginMembers = bveTypes.GetClassInfoOf<AtsPlugin>();

            Patches = new PatchSet(mainFormMembers, scenarioMembers, atsPluginMembers);
            ListenPatchEvents();

            AtsPluginOverrider = new AtsPluginOverrider(BveHacker.LoadingProgressForm, vehicleMembers, atsPluginMembers);

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
            AtsPluginOverrider.Dispose();

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
    }
}