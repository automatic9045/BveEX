using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.AtsArrayExtender
{
    [Plugin(PluginType.Extension)]
    [HideExtensionMain]
    internal class ExtensionMain : AssemblyPluginBase, IExtension
    {
        private readonly static Type[] atsPluginConstructorParamTypes = new Type[]
        {
            typeof(VehicleLocation),
            typeof(InputManager),
            typeof(HandleSet),
            typeof(HandleSet),
            typeof(VehicleStateStore),
            typeof(SectionManager),
            typeof(MapFunctionList),
            typeof(DoorSet),
        };

        private readonly HarmonyPatch AtsPluginConstructorPatch;

        public ExtensionMain(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet atsPluginMembers = BveHacker.BveTypes.GetClassInfoOf<AtsPlugin>();
            FastConstructor atsPluginConstructor = atsPluginMembers.GetSourceConstructor(atsPluginConstructorParamTypes);

            AtsPluginConstructorPatch = HarmonyPatch.Patch(nameof(AtsArrayExtender), atsPluginConstructor.Source, PatchType.Postfix);
            AtsPluginConstructorPatch.Invoked += AtsPluginConstructed;
        }

        public override void Dispose()
        {
            AtsPluginConstructorPatch?.Dispose();
        }

        private PatchInvokationResult AtsPluginConstructed(object sender, PatchInvokedEventArgs e)
        {
            AtsPlugin atsPlugin = AtsPlugin.FromSource(e.Instance);

            atsPlugin.PanelArray = new int[1024];
            atsPlugin.SoundArray = new int[1024];
            atsPlugin.OldSoundArray = new int[1024];

            atsPlugin.StateStore.PanelArray = new double[1024];

            return PatchInvokationResult.DoNothing(e);
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
