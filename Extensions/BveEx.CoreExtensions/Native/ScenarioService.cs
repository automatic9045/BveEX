using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Extensions.Native.Input;

namespace BveEx.Extensions.Native
{
    internal class ScenarioService
    {
        public IList<int> AtsPanelValues { get; }
        public IList<int> AtsSounds { get; }
        public AtsKeySet AtsKeys { get; } = new AtsKeySet();
        public VehicleSpec VehicleSpec { get; set; } = null;
        public VehicleState VehicleState { get; set; } = null;

        public ScenarioService(AtsPlugin atsPlugin)
        {
            AtsPanelValues = new PanelArray(atsPlugin);
            AtsSounds = new SoundArray(atsPlugin);
        }
    }
}
