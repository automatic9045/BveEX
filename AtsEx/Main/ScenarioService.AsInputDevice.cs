using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.Plugins;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Native;

namespace AtsEx
{
    internal partial class ScenarioService
    {
        internal sealed class AsInputDevice : ScenarioService
        {
            public Scenario Target { get; private set; } = null;

            public AsInputDevice(AtsEx.AsInputDevice atsEx, PluginSourceSet vehiclePluginUsing, VehicleConfig vehicleConfig, VehicleSpec vehicleSpec)
                : base(atsEx, vehiclePluginUsing, vehicleConfig, vehicleSpec)
            {
                AtsEx.BveHacker.ScenarioCreated += OnScenarioCreated;
            }

            public override void Dispose()
            {
                AtsEx.BveHacker.ScenarioCreated -= OnScenarioCreated;
                base.Dispose();
            }

            private void OnScenarioCreated(ScenarioCreatedEventArgs e)
            {
                Target = e.Scenario;
            }
        }
    }
}
