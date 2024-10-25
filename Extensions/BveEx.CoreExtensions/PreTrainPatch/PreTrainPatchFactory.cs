using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.PreTrainPatch
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IPreTrainPatchFactory))]
    internal class PreTrainPatchFactory : AssemblyPluginBase, IPreTrainPatchFactory
    {
        private readonly FastMethod UpdatePreTrainSectionMethod;

        public override string Title { get; } = nameof(PreTrainPatch);
        public override string Description { get; } = "先行列車の走行位置を自由に変更できるようにするパッチを提供します。";

        public PreTrainPatchFactory(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet members = BveHacker.BveTypes.GetClassInfoOf<SectionManager>();
            UpdatePreTrainSectionMethod = members.GetSourceMethodOf(nameof(SectionManager.UpdatePreTrainSection));
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed) => new ExtensionTickResult();

        public PreTrainPatch Patch(string name, SectionManager sectionManager, IPreTrainLocationConverter converter)
            => new PreTrainPatch(name, UpdatePreTrainSectionMethod, sectionManager, converter);
    }
}
