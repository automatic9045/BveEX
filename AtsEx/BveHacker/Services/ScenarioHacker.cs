using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;

using AtsEx.PluginHost;

namespace AtsEx.BveHackerServices
{
    internal sealed class ScenarioHacker : IDisposable
    {
        private readonly MainForm MainForm;

        private readonly HarmonyPatch OpenPatch;
        private readonly HarmonyPatch LoadPatch;
        private readonly HarmonyPatch UnloadPatch;
        private readonly HarmonyPatch InitializeTimeAndLocationMethodPatch;
        private readonly HarmonyPatch InitializeMethodPatch;

        public ScenarioInfo CurrentScenarioInfo
        {
            get => MainForm.CurrentScenarioInfo;
            set => MainForm.CurrentScenarioInfo = value;
        }

        public Scenario CurrentScenario
        {
            get => MainForm.CurrentScenario;
            set => MainForm.CurrentScenario = value;
        }

        public event ScenarioOpenedEventHandler ScenarioOpened;
        public event ScenarioClosedEventHandler ScenarioClosed;
        public event ScenarioCreatedEventHandler ScenarioCreated;

        public ScenarioHacker(MainFormHacker mainFormHacker, BveTypeSet bveTypes)
        {
            MainForm = mainFormHacker.TargetForm;

            ClassMemberSet mainFormMembers = bveTypes.GetClassInfoOf<MainForm>();
            ClassMemberSet scenarioMembers = bveTypes.GetClassInfoOf<Scenario>();

            FastMethod openMethod = mainFormMembers.GetSourceMethodOf(nameof(BveTypes.ClassWrappers.MainForm.OpenScenario));
            FastMethod loadMethod = mainFormMembers.GetSourceMethodOf(nameof(BveTypes.ClassWrappers.MainForm.LoadScenario));
            FastMethod unloadMethod = mainFormMembers.GetSourceMethodOf(nameof(BveTypes.ClassWrappers.MainForm.UnloadScenario));

            FastMethod initializeTimeAndLocationMethod = scenarioMembers.GetSourceMethodOf(nameof(Scenario.InitializeTimeAndLocation));
            FastMethod initializeMethod = scenarioMembers.GetSourceMethodOf(nameof(Scenario.Initialize));

            OpenPatch = CreateAndSetupPatch(openMethod.Source);
            LoadPatch = CreateAndSetupPatch(loadMethod.Source);
            UnloadPatch = CreateAndSetupPatch(unloadMethod.Source);
            InitializeTimeAndLocationMethodPatch = CreateAndSetupPatch(initializeTimeAndLocationMethod.Source, PatchType.Postfix);
            InitializeMethodPatch = CreateAndSetupPatch(initializeMethod.Source, PatchType.Postfix);

            OpenPatch.Invoked += OnOpened;
            LoadPatch.Invoked += OnLoaded;
            UnloadPatch.Invoked += OnDisposed;


            HarmonyPatch CreateAndSetupPatch(MethodBase original, PatchType patchType = PatchType.Prefix)
            {
                HarmonyPatch patch = HarmonyPatch.Patch(nameof(ScenarioHacker), original, patchType);
                return patch;
            }
        }

        private PatchInvokationResult OnOpened(object sender, PatchInvokedEventArgs e)
        {
            if (0 < e.Args.Length && (string)e.Args[0] == string.Empty)
            {
                MainForm instance = MainForm.FromSource(e.Instance);
                instance.OpenScenario(null);

                return new PatchInvokationResult(SkipModes.SkipPatches | SkipModes.SkipOriginal);
            }

            return PatchInvokationResult.DoNothing(e);
        }

        private PatchInvokationResult OnLoaded(object sender, PatchInvokedEventArgs e)
        {
            // 再読込の場合は MainForm.UnloadScenario が呼ばれない
            if (!(CurrentScenario is null)) ScenarioClosed?.Invoke(EventArgs.Empty);

            ScenarioInfo scenarioInfo = ScenarioInfo.FromSource(e.Args[0]);
            ScenarioOpened?.Invoke(new ScenarioOpenedEventArgs(scenarioInfo));

            return PatchInvokationResult.DoNothing(e);
        }

        private PatchInvokationResult OnDisposed(object sender, PatchInvokedEventArgs e)
        {
            ScenarioClosed?.Invoke(EventArgs.Empty);
            return PatchInvokationResult.DoNothing(e);
        }

        public void Dispose()
        {
            OpenPatch.Dispose();
            LoadPatch.Dispose();
            UnloadPatch.Dispose();
            InitializeTimeAndLocationMethodPatch.Dispose();
            InitializeMethodPatch.Dispose();
        }

        public void BeginObserveInitialization()
        {
            InitializeTimeAndLocationMethodPatch.Invoked += OnPatchInvoked;
            InitializeMethodPatch.Invoked += OnPatchInvoked;


            PatchInvokationResult OnPatchInvoked(object sender, PatchInvokedEventArgs e)
            {
                InitializeTimeAndLocationMethodPatch.Invoked -= OnPatchInvoked;
                InitializeMethodPatch.Invoked -= OnPatchInvoked;

                Scenario scenario = Scenario.FromSource(e.Instance);
                ScenarioCreatedEventArgs scenarioCreatedEventArgs = new ScenarioCreatedEventArgs(scenario);
                ScenarioCreated?.Invoke(scenarioCreatedEventArgs);

                return PatchInvokationResult.DoNothing(e);
            }
        }
    }
}
