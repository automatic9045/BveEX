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

namespace BveEx.Extensions.TrainDrawPatch
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(ITrainDrawPatchFactory))]
    internal sealed class TrainDrawPatchFactory : AssemblyPluginBase, ITrainDrawPatchFactory
    {
        private readonly FastMethod DrawCarsMethod;

        public override string Title { get; } = nameof(TrainDrawPatch);
        public override string Description { get; } = "他列車を自由に移動・回転できるようにするパッチを提供します。";

        public TrainDrawPatchFactory(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet members = BveHacker.BveTypes.GetClassInfoOf<Train>();
            DrawCarsMethod = members.GetSourceMethodOf(nameof(Train.DrawCars));
        }

        public override void Dispose()
        {
        }

        public override IPluginTickResult Tick(TimeSpan elapsed) => new ExtensionTickResult();

        public TrainDrawPatch Patch(string name, Train target, IMatrixConverter worldMatrixConverter)
            => new TrainDrawPatch(name, DrawCarsMethod, target, worldMatrixConverter, new DefaultMatrixConverter());

        public TrainDrawPatch Patch(string name, Train target, IMatrixConverter worldMatrixConverter, IMatrixConverter viewMatrixConverter)
            => new TrainDrawPatch(name, DrawCarsMethod, target, worldMatrixConverter, viewMatrixConverter);

        public TrainDrawPatch Patch(string name, Train target, Action<Direct3DProvider, Matrix> overrideAction)
            => new TrainDrawPatch(name, DrawCarsMethod, target, overrideAction);


        private class DefaultMatrixConverter : IMatrixConverter
        {
            public Matrix Convert(Matrix source) => source;
        }
    }
}
