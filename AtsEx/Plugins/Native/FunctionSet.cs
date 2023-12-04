using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using FastMember.PInvoke;

namespace AtsEx.Plugins.Native
{
    internal class FunctionSet
    {
        private readonly FastMethod LoadMethod;
        private readonly FastMethod DisposeMethod;
        private readonly FastMethod GetPluginVersionMethod;
        private readonly FastMethod SetVehicleSpecMethod;
        private readonly FastMethod InitializeMethod;
        private readonly FastMethod ElapseMethod;
        private readonly FastMethod SetPowerMethod;
        private readonly FastMethod SetBrakeMethod;
        private readonly FastMethod SetReverserMethod;
        private readonly FastMethod KeyDownMethod;
        private readonly FastMethod KeyUpMethod;
        private readonly FastMethod HornBlowMethod;
        private readonly FastMethod DoorOpenMethod;
        private readonly FastMethod DoorCloseMethod;
        private readonly FastMethod SetSignalMethod;
        private readonly FastMethod SetBeaconDataMethod;

        private FunctionSet(DynamicLibraryCaller source)
        {
            LoadMethod = source.RegisterFunction(nameof(Load), typeof(void));
            DisposeMethod = source.RegisterFunction(nameof(Dispose), typeof(void));
            GetPluginVersionMethod = source.RegisterFunction(nameof(GetPluginVersion), typeof(int));
            SetVehicleSpecMethod = source.RegisterFunction(nameof(SetVehicleSpec), typeof(void), typeof(ATS_VEHICLESPEC));
            InitializeMethod = source.RegisterFunction(nameof(Initialize), typeof(void), typeof(int));
            ElapseMethod = source.RegisterFunction(nameof(Elapse), typeof(ATS_HANDLES), typeof(ATS_VEHICLESTATE), typeof(int[]), typeof(int[]));
            SetPowerMethod = source.RegisterFunction(nameof(SetPower), typeof(void), typeof(int));
            SetBrakeMethod = source.RegisterFunction(nameof(SetBrake), typeof(void), typeof(int));
            SetReverserMethod = source.RegisterFunction(nameof(SetReverser), typeof(void), typeof(int));
            KeyDownMethod = source.RegisterFunction(nameof(KeyDown), typeof(void), typeof(int));
            KeyUpMethod = source.RegisterFunction(nameof(KeyUp), typeof(void), typeof(int));
            HornBlowMethod = source.RegisterFunction(nameof(HornBlow), typeof(void), typeof(int));
            DoorOpenMethod = source.RegisterFunction(nameof(DoorOpen), typeof(void));
            DoorCloseMethod = source.RegisterFunction(nameof(DoorClose), typeof(void));
            SetSignalMethod = source.RegisterFunction(nameof(SetSignal), typeof(void), typeof(int));
            SetBeaconDataMethod = source.RegisterFunction(nameof(SetBeaconData), typeof(void), typeof(ATS_BEACONDATA));
        }

        public static FunctionSet RegisterFunctions(DynamicLibraryCaller source)
        {
            FunctionSet functions = new FunctionSet(source);
            return functions;
        }

        public void Load() => LoadMethod.Invoke(null, null);
        public void Dispose() => DisposeMethod.Invoke(null, null);
        public int GetPluginVersion() => (int)GetPluginVersionMethod.Invoke(null, null);
        public void SetVehicleSpec(ATS_VEHICLESPEC vehicleSpec) => SetVehicleSpecMethod.Invoke(null, new object[] { vehicleSpec });
        public void Initialize(int brake) => InitializeMethod.Invoke(null, null);
        public ATS_HANDLES Elapse(ATS_VEHICLESTATE vehicleState, int[] panel, int[] sound) => (ATS_HANDLES)ElapseMethod.Invoke(null, new object[] { vehicleState, panel, sound });
        public void SetPower(int notch) => SetPowerMethod.Invoke(null, new object[] { notch });
        public void SetBrake(int notch) => SetBrakeMethod.Invoke(null, new object[] { notch });
        public void SetReverser(int pos) => SetReverserMethod.Invoke(null, new object[] { pos });
        public void KeyDown(int atsKeyCode) => KeyDownMethod.Invoke(null, new object[] { atsKeyCode });
        public void KeyUp(int atsKeyCode) => KeyUpMethod.Invoke(null, new object[] { atsKeyCode });
        public void HornBlow(int hornType) => HornBlowMethod.Invoke(null, new object[] { hornType });
        public void DoorOpen() => DoorOpenMethod.Invoke(null, null);
        public void DoorClose() => DoorCloseMethod.Invoke(null, null);
        public void SetSignal(int signal) => SetSignalMethod.Invoke(null, new object[] { signal });
        public void SetBeaconData(ATS_BEACONDATA beaconData) => SetBeaconDataMethod.Invoke(null, new object[] { beaconData });
    }
}
