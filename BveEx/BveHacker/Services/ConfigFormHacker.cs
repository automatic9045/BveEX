using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace BveEx.BveHackerServices
{
    internal class ConfigFormHacker : IDisposable
    {
        private readonly HarmonyPatch ConstructorPatch;
        private readonly HarmonyPatch DisposePatch;

        public bool IsReady => !(Form is null);
        public ConfigForm Form { get; private set; } = null;
        public Form FormSource => Form?.Src as Form;

        public ConfigFormHacker(BveTypeSet bveTypes)
        {
            ClassMemberSet configFormMembers = bveTypes.GetClassInfoOf<ConfigForm>();

            FastConstructor constructor = configFormMembers.GetSourceConstructor();
            FastMethod disposeMethod = configFormMembers.GetSourceMethodOf(nameof(ConfigForm.Dispose));

            ConstructorPatch = HarmonyPatch.Patch(nameof(ConfigFormHacker), constructor.Source, PatchType.Prefix);
            DisposePatch = HarmonyPatch.Patch(nameof(ConfigFormHacker), disposeMethod.Source.GetBaseDefinition(), PatchType.Prefix);

            ConstructorPatch.Invoked += (sender, e) =>
            {
                Form = ConfigForm.FromSource(e.Instance);
                return PatchInvokationResult.DoNothing(e);
            };

            DisposePatch.Invoked += (sender, e) =>
            {
                Form = null;
                return PatchInvokationResult.DoNothing(e);
            };
        }

        public void Dispose()
        {
            ConstructorPatch.Dispose();
            DisposePatch.Dispose();
        }
    }
}
