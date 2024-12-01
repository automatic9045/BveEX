using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Extensions.Native;
using BveEx.Extensions.SignalPatch;
using BveEx.PluginHost;
using BveEx.PluginHost.Input;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.MapPlugins.SignalController
{
    [Plugin(PluginType.MapPlugin)]
    public class SignalController : AssemblyPluginBase
    {
        private SignalPatch SignalPatch;
        private int SignalIndex = 0;

        public SignalController(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;

            INative native = Extensions.GetExtension<INative>();
            native.VehicleSpecLoaded += (sender, e) =>
            {
                native.AtsKeys.GetKey(AtsKeyName.D).Pressed += OnDPressed;
                native.AtsKeys.GetKey(AtsKeyName.E).Pressed += OnEPressed;
                native.AtsKeys.GetKey(AtsKeyName.F).Pressed += OnFPressed;
                native.AtsKeys.GetKey(AtsKeyName.G).Pressed += OnGPressed;
                native.AtsKeys.GetKey(AtsKeyName.H).Pressed += OnHPressed;
            };
        }

        private void OnDPressed(object sender, EventArgs e) => SignalIndex = 0;
        private void OnEPressed(object sender, EventArgs e) => SignalIndex = 1;
        private void OnFPressed(object sender, EventArgs e) => SignalIndex = 2;
        private void OnGPressed(object sender, EventArgs e) => SignalIndex = 3;
        private void OnHPressed(object sender, EventArgs e) => SignalIndex = 4;

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            Section section = e.Scenario.SectionManager.Sections[2] as Section;

            SignalPatch = Extensions.GetExtension<ISignalPatchFactory>().Patch(nameof(SignalPatch), section, source => SignalIndex);
        }

        public override void Dispose()
        {
            SignalPatch?.Dispose();
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
