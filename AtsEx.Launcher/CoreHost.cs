using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    public class CoreHost : IDisposable
    {
        public static string VehicleConfigPath { get; private set; } = null;
        public static string VehiclePluginUsingPath { get; private set; } = null;


        private VehicleSpec VehicleSpec;

        private int BrakeNotch = 0;
        private int PowerNotch = 0;
        private int ReverserPosition = 0;

        internal CoreHost(Assembly callerAssembly)
        {
            string directory = Path.GetDirectoryName(callerAssembly.Location);

            VehicleConfigPath = Path.Combine(directory, Path.GetFileNameWithoutExtension(callerAssembly.Location) + ".VehicleConfig.xml");
            VehiclePluginUsingPath = Path.Combine(directory, Path.GetFileNameWithoutExtension(callerAssembly.Location) + ".VehiclePluginUsing.xml");
        }

        public void Dispose()
        {
            VehiclePluginUsingPath = null;
        }

        public void SetVehicleSpec(VehicleSpec vehicleSpec)
        {
            VehicleSpec = vehicleSpec;
        }

        public void Initialize(int defaultBrakePosition)
        {
            switch (defaultBrakePosition)
            {
                case -2:
                    BrakeNotch = 0;
                    break;

                case -1:
                    BrakeNotch = VehicleSpec.B67Notch;
                    break;

                default:
                    BrakeNotch = VehicleSpec.BrakeNotches;
                    break;
            }
        }

        public AtsHandles Elapse(VehicleState vehicleState, IntPtr panel, IntPtr sound)
        {
            return new AtsHandles()
            {
                Brake = BrakeNotch,
                Power = PowerNotch,
                Reverser = ReverserPosition,
                ConstantSpeed = 0,
            };
        }

        public void SetPower(int notch)
        {
            PowerNotch = notch;
        }

        public void SetBrake(int notch)
        {
            BrakeNotch = notch;
        }

        public void SetReverser(int position)
        {
            ReverserPosition = position;
        }

        public void KeyDown(int atsKeyCode)
        {
        }

        public void KeyUp(int atsKeyCode)
        {
        }

        public void HornBlow(int hornType)
        {
        }

        public void DoorOpen()
        {
        }

        public void DoorClose()
        {
        }

        public void SetSignal(int signal)
        {
        }

        public void SetBeaconData(BeaconData beaconData)
        {
        }
    }
}
