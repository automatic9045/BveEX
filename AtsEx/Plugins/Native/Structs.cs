using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Plugins.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ATS_VEHICLESPEC
    {
        public int BrakeNotches;
        public int PowerNotches;
        public int AtsNotch;
        public int B67Notch;
        public int Cars;
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct ATS_VEHICLESTATE
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

    [StructLayout(LayoutKind.Sequential)]
    internal struct ATS_BEACONDATA
    {
        public int Type;
        public int Signal;
        public float Distance;
        public int Optional;
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct ATS_HANDLES
    {
        public int Brake;
        public int Power;
        public int Reverser;
        public int ConstantSpeed;
    };
}
