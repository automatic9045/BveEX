using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.DummyAts
{
    public enum AtsKey
    {
        S,
        A1,
        A2,
        B1,
        B2,
        C1,
        C2,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
    }

    public enum AtsInitialHandlePosition
    {
        ServiceBrake,
        EmergencyBrake,
        Removed,
    }

    public enum AtsHornType
    {
        Primary,
        Secondary,
        Music,
    }

    public static class AtsCscInstruction
    {
        public const int Continue = 0;
        public const int Enable = 1;
        public const int Disable = 2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AtsVehicleSpec
    {
        public int BrakeNotches;
        public int PowerNotches;
        public int AtsNotch;
        public int B67Notch;
        public int Cars;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AtsVehicleState
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
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AtsBeaconData
    {
        public int Type;
        public int Signal;
        public float Distance;
        public int Optional;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AtsHandles
    {
        public int Brake;
        public int Power;
        public int Reverser;
        public int ConstantSpeed;
    }
}
