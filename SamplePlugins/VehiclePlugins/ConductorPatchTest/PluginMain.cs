using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Extensions.Native;
using BveEx.PluginHost;
using BveEx.PluginHost.Input;
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

            INative native = Extensions.GetExtension<INative>();
            native.AtsKeys.GetKey(AtsKeyName.D).Pressed += OnDPressed;
            native.AtsKeys.GetKey(AtsKeyName.E).Pressed += OnEPressed;
            native.AtsKeys.GetKey(AtsKeyName.F).Pressed += OnFPressed;
            native.AtsKeys.GetKey(AtsKeyName.G).Pressed += OnGPressed;
            native.AtsKeys.GetKey(AtsKeyName.H).Pressed += OnHPressed;
        }

        public override void Dispose()
        {
            if (!(Patch is null))
            {
                IConductorPatchFactory conductorPatchFactory = Extensions.GetExtension<IConductorPatchFactory>();
                conductorPatchFactory.Unpatch(Patch);
            }

            BveHacker.ScenarioCreated -= OnScenarioCreated;
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
