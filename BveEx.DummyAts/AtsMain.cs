using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.DummyAts
{
    public static class AtsMain
    {
        private const int Version = 0x00020000;

        private static int Power = 0;
        private static int Brake = 0;
        private static int Reverser = 0;

        [DllExport(CallingConvention.StdCall)]
        public static void Load()
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void Dispose()
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static int GetPluginVersion()
        {
            return Version;
        }

        [DllExport(CallingConvention.StdCall)]
        public static void SetVehicleSpec(AtsVehicleSpec vehicleSpec)
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void Initialize(int initialHandlePosition)
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static AtsHandles Elapse(AtsVehicleState vehicleState, IntPtr panel, IntPtr sound)
        {
            return new AtsHandles()
            {
                Power = Power,
                Brake = Brake,
                Reverser = Reverser,
                ConstantSpeed = AtsCscInstruction.Continue,
            };
        }

        [DllExport(CallingConvention.StdCall)]
        public static void SetPower(int handlePosition)
        {
            Power = handlePosition;
        }

        [DllExport(CallingConvention.StdCall)]
        public static void SetBrake(int handlePosition)
        {
            Brake = handlePosition;
        }

        [DllExport(CallingConvention.StdCall)]
        public static void SetReverser(int handlePosition)
        {
            Reverser = handlePosition;
        }

        [DllExport(CallingConvention.StdCall)]
        public static void KeyDown(int keyIndex)
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void KeyUp(int keyIndex)
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void HornBlow(int hornIndex)
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void DoorOpen()
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void DoorClose()
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void SetSignal(int signalIndex)
        {
        }

        [DllExport(CallingConvention.StdCall)]
        public static void SetBeaconData(AtsBeaconData beaconData)
        {
        }
    }
}
