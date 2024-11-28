using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Input;
using BveEx.PluginHost.Input.Native;
using BveEx.PluginHost.Plugins;

using BveEx.Extensions.ConductorPatch;

namespace BveEx.Samples.VehiclePlugins.ConductorPatchTest
{
    [Plugin(PluginType.VehiclePlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private TestConductor Conductor = null;
        private ConductorPatch Patch = null;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;

            Native.NativeKeys.AtsKeys[NativeAtsKeyName.D].Pressed += OnDPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.E].Pressed += OnEPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.F].Pressed += OnFPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.G].Pressed += OnGPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.H].Pressed += OnHPressed;
        }

        public override void Dispose()
        {
            if (!(Patch is null))
            {
                IConductorPatchFactory conductorPatchFactory = Extensions.GetExtension<IConductorPatchFactory>();
                conductorPatchFactory.Unpatch(Patch);
            }

            BveHacker.ScenarioCreated -= OnScenarioCreated;

            Native.NativeKeys.AtsKeys[NativeAtsKeyName.D].Pressed -= OnDPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.E].Pressed -= OnEPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.F].Pressed -= OnFPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.G].Pressed -= OnGPressed;
            Native.NativeKeys.AtsKeys[NativeAtsKeyName.H].Pressed -= OnHPressed;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            Conductor originalConductor = e.Scenario.Vehicle.Conductor;

            IConductorPatchFactory conductorPatchFactory = Extensions.GetExtension<IConductorPatchFactory>();
            Conductor = new TestConductor(originalConductor);
            Patch = conductorPatchFactory.Patch(Conductor, DeclarationPriority.Sequentially);
        }

        private void OnDPressed(object sender, EventArgs e) => Conductor.OpenDoors(DoorSide.Left);
        private void OnEPressed(object sender, EventArgs e) => Conductor.CloseDoors(DoorSide.Left);
        private void OnFPressed(object sender, EventArgs e) => Conductor.OpenDoors(DoorSide.Right);
        private void OnGPressed(object sender, EventArgs e) => Conductor.CloseDoors(DoorSide.Right);
        private void OnHPressed(object sender, EventArgs e) => Conductor.RequestFixStopPosition();

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
