using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AtsEx.Launcher;

namespace AtsEx.Caller
{
    internal static class AtsCore
    {
        private const int Version = 0x00020000;

        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        private const string LauncherName = "AtsEx.Launcher";

        private const int CallerVersion = 3;

        private static VersionSelector.AsAtsPlugin VersionSelector;

        static AtsCore()
        {
            try
            {
                string configLocation = Path.Combine(Path.GetDirectoryName(Assembly.Location), "AtsEx.Caller.txt");
                string launcherLocation;
                using (StreamReader sr = new StreamReader(configLocation))
                {
                    launcherLocation = Path.Combine(Path.GetDirectoryName(Assembly.Location), sr.ReadLine(), LauncherName + ".dll");
                }

                AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                {
                    AssemblyName assemblyName = new AssemblyName(e.Name);
                    if (assemblyName.Name == LauncherName)
                    {
                        if (!File.Exists(launcherLocation))
                        {
                            throw new FileNotFoundException("AtsEX が見つからないため、AtsEX Caller による要求を続行できません。" +
                                "AtsEX をインストールしてください。インストール済の場合は、有効化されているか、またパスの指定が誤っていないかご確認ください。");
                        }

                        return Assembly.LoadFrom(launcherLocation);
                    }
                    else
                    {
                        return null;
                    }
                };

                CheckCallerCompatibility(launcherLocation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to run AtsEX Launcher.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }


            void CheckCallerCompatibility(string launcherLocation)
            {
                Assembly launcherAssembly = typeof(CallerCompatibilityVersionAttribute).Assembly;
                CallerCompatibilityVersionAttribute compatibilityVersionAttribute = launcherAssembly.GetCustomAttribute<CallerCompatibilityVersionAttribute>();
                if (compatibilityVersionAttribute.Version != CallerVersion)
                {
                    Version assemblyVersion = Assembly.GetName().Version;
                    throw new NotSupportedException($"読み込まれた AtsEX Caller (バージョン {assemblyVersion}) は現在の AtsEX ではサポートされていません。");
                }
            }
        }

        /// <summary>Called when this plugin is loaded</summary>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void Load() => VersionSelector = new VersionSelector.AsAtsPlugin(Assembly);

        /// <summary>Called when this plugin is unloaded</summary>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void Dispose() => VersionSelector?.CoreHost.Dispose();

        /// <summary>Called when the version number is needed</summary>
        /// <returns>plugin version number</returns>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static int GetPluginVersion() => Version;

        /// <summary>Called when set the Vehicle Spec</summary>
        /// <param name="vehicleSpec">Set Spec</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetVehicleSpec(VehicleSpec vehicleSpec) => VersionSelector.CoreHost.SetVehicleSpec(vehicleSpec);

        /// <summary>Called when car is put</summary>
        /// <param name="defaultBrakePosition">Default Brake Position (Refer to InitialPos class)</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void Initialize(int defaultBrakePosition) => VersionSelector.CoreHost.Initialize(defaultBrakePosition);

        /// <summary>Called in every refleshing the display</summary>
        /// <param name="vehicleState">State</param>
        /// <param name="panel">Panel (Pointer of int[256])</param>
        /// <param name="sound">Sound (Pointer of int[256])</param>
        /// <returns></returns>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static AtsHandles Elapse(VehicleState vehicleState, IntPtr panel, IntPtr sound) => VersionSelector.CoreHost.Elapse(vehicleState, panel, sound);

        /// <summary>Called when Power notch is moved</summary>
        /// <param name="notch">Notch Number</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetPower(int notch) => VersionSelector.CoreHost.SetPower(notch);

        /// <summary>Called when Brake Notch is moved</summary>
        /// <param name="notch">Brake notch Number</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetBrake(int notch) => VersionSelector.CoreHost.SetBrake(notch);

        /// <summary>Called when Reverser is moved</summary>
        /// <param name="position">Reverser Position</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetReverser(int position) => VersionSelector.CoreHost.SetReverser(position);

        /// <summary>Called when Key is Pushed</summary>
        /// <param name="atsKeyCode">Pushed Key Number</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void KeyDown(int atsKeyCode) => VersionSelector.CoreHost.KeyDown(atsKeyCode);

        /// <summary>Called when Key is Released</summary>
        /// <param name="atsKeyCode">Released Key Number</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void KeyUp(int atsKeyCode) => VersionSelector.CoreHost.KeyUp(atsKeyCode);

        /// <summary>Called when the Horn is Blown</summary>
        /// <param name="hornType">Blown Horn Number</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void HornBlow(int hornType) => VersionSelector.CoreHost.HornBlow(hornType);

        /// <summary>Called when Door is opened</summary>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void DoorOpen() => VersionSelector.CoreHost.DoorOpen();

        /// <summary>Called when Door is closed</summary>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void DoorClose() => VersionSelector.CoreHost.DoorClose();

        /// <summary>Called when the Signal Showing Number is changed</summary>
        /// <param name="signal">Signal Showing Number</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetSignal(int signal) => VersionSelector.CoreHost.SetSignal(signal);

        /// <summary>Called when passed above the Beacon</summary>
        /// <param name="beaconData">Beacon info</param>
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetBeaconData(BeaconData beaconData) => VersionSelector.CoreHost.SetBeaconData(beaconData);
    }
}
