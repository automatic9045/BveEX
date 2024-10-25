using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;

using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.ConductorPatch
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IConductorPatchFactory))]
    internal class ConductorPatchFactory : AssemblyPluginBase, IConductorPatchFactory
    {
        private readonly HarmonyPatchSet HarmonyPatches;
        private readonly ConductorPatchInvoker PatchInvoker;

        private readonly List<ConductorPatch> Patches = new List<ConductorPatch>();

        public override string Title { get; } = nameof(ConductorPatch);
        public override string Description { get; } = "車掌の動作を自由に変更できるようにするパッチを提供します。";

        public ConductorPatchFactory(PluginBuilder builder) : base(builder)
        {
            HarmonyPatches = new HarmonyPatchSet(BveHacker.BveTypes);
            PatchInvoker = new ConductorPatchInvoker(HarmonyPatches);

            BveHacker.ScenarioClosed += OnScenarioClosed;
        }

        public override void Dispose()
        {
            HarmonyPatches.Dispose();
        }

        private void OnScenarioClosed(EventArgs e)
        {
            PatchInvoker.ActivePatch = null;
        }

        public override TickResult Tick(TimeSpan elapsed) => new ExtensionTickResult();

        public void BeginPatch(Func<Conductor, ConductorBase> conductorFactory, DeclarationPriority priority, Action<ConductorPatch> patchedCallback)
        {
            HarmonyPatches.Constructor.Invoked += OnConductorConstructed;


            PatchInvokationResult OnConductorConstructed(object sender, PatchInvokedEventArgs e)
            {
                HarmonyPatches.Constructor.Invoked -= OnConductorConstructed;

                Conductor original = Conductor.FromSource(e.Instance);
                ConductorBase conductor = conductorFactory(original);
                ConductorPatch patch = Patch(conductor, priority);

                patchedCallback(patch);
                return PatchInvokationResult.DoNothing(e);
            }
        }

        public ConductorPatch Patch(ConductorBase conductor, DeclarationPriority priority)
        {
            if (PatchInvoker.ActivePatch?.Priority == DeclarationPriority.TopMost)
            {
                throw new InvalidOperationException($"優先度 {DeclarationPriority.TopMost} のパッチが既に適用されています。");
            }

            ConductorPatch patch = new ConductorPatch(conductor, priority);
            Patches.Add(patch);

            PatchInvoker.ActivePatch = GetStrongest();
            return patch;
        }

        public void Unpatch(ConductorPatch patch)
        {
            Patches.Remove(patch);
            PatchInvoker.ActivePatch = GetStrongest();
        }

        private ConductorPatch GetStrongest()
        {
            ConductorPatch topMost = Patches.FirstOrDefault(patch => patch.Priority == DeclarationPriority.TopMost);
            if (!(topMost is null)) return topMost;

            ConductorPatch sequentially = Patches.LastOrDefault(patch => patch.Priority == DeclarationPriority.Sequentially);
            if (!(sequentially is null)) return sequentially;

            ConductorPatch asDefaultValue = Patches.LastOrDefault();
            return asDefaultValue;
        }
    }
}
