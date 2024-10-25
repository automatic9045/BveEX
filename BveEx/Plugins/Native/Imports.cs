using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Plugins.Native
{
    internal static class Imports
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        internal delegate void VoidDelegate();
        internal delegate void SetVehicleSpecDelegate(VehicleSpec vehicleSpec);
        internal delegate void Int32Delegate(int arg);
        internal delegate Handles ElapseDelegate(VehicleState vehicleState, int[] panel, int[] sound);
        internal delegate void SetBeaconDataDelegate(BeaconData beaconData);
        internal delegate int SetPluginVersionDelegate();

        internal struct VehicleSpec
        {
            public int BrakeNotches;
            public int PowerNotches;
            public int AtsNotch;
            public int B67Notch;
            public int Cars;
        };

        internal struct VehicleState
        {
            public double Location;
            public float Speed;
            public int Time;
            public float BcPressure;
            public float MrPressure;
            public float ErPressure;
            public float BpPressure;
            public float SapPressure;
            public float Current;
        };

        internal struct BeaconData
        {
            public int Type;
            public int Signal;
            public float Distance;
            public int Optional;
        };

        internal struct Handles
        {
            public int Brake;
            public int Power;
            public int Reverser;
            public int ConstantSpeed;
        };
    }
}
