using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using FastMember.PInvoke;

using AtsEx.PluginHost.Plugins;

namespace AtsEx.Plugins.Native
{
    [DoNotUseBveHacker]
    internal class NativePlugin : PluginBase
    {
        private readonly FunctionSet Functions;

        public override string Location { get; }
        public override string Name { get; }
        public override string Title { get; } = "";
        public override string Version { get; } = "";
        public override string Description { get; } = "";
        public override string Copyright { get; } = "";

        private NativePlugin(DynamicLibraryCaller library, FunctionSet functions, PluginBuilder builder, PluginType pluginType) : base(builder, pluginType, false)
        {
            Functions = functions;
            Location = library.FilePath;
            Name = Path.GetFileName(Location);

            Functions.Load();

            ATS_VEHICLESPEC vehicleSpec = new ATS_VEHICLESPEC()
            {
                BrakeNotches = Native.VehicleSpec.BrakeNotches,
                PowerNotches = Native.VehicleSpec.PowerNotches,
                AtsNotch = Native.VehicleSpec.AtsNotch,
                B67Notch = Native.VehicleSpec.B67Notch,
                Cars = Native.VehicleSpec.Cars,
            };
            Functions.SetVehicleSpec(vehicleSpec);
        }

        public static NativePlugin FromPackage(PluginBuilder builder, PluginType pluginType, string libraryPath)
        {
            DynamicLibraryCaller source = new DynamicLibraryCaller(libraryPath);
            FunctionSet functions = FunctionSet.RegisterFunctions(source);
            source.Compile();

            int nativeVersion = functions.GetPluginVersion();
            if (nativeVersion != 0x00020000)
            {
                throw new AtsPluginVersionNotSupportedException(nativeVersion);
            }

            NativePlugin result = new NativePlugin(source, functions, builder, pluginType);
            return result;
        }

        public override void Dispose()
        {
            Functions.Dispose();
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            return new VehiclePluginTickResult();
        }
    }
}
