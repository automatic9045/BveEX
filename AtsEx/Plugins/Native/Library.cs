using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace AtsEx.Plugins.Native
{
    internal class Library : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<Library>(@"Core\Plugins\Native");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotCheckVersion { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> VersionNotSupported { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static Library()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly IntPtr Handle;
        private readonly Imports.SetPluginVersionDelegate GetPluginVersion;

        public Imports.VoidDelegate Load { get; }
        public Imports.VoidDelegate DisposeProc { get; }
        public Imports.SetVehicleSpecDelegate SetVehicleSpec { get; }
        public Imports.Int32Delegate Initialize { get; }
        public Imports.ElapseDelegate Elapse { get; }
        public Imports.Int32Delegate SetPower { get; }
        public Imports.Int32Delegate SetBrake { get; }
        public Imports.Int32Delegate SetReverser { get; }
        public Imports.Int32Delegate KeyDown { get; }
        public Imports.Int32Delegate KeyUp { get; }
        public Imports.Int32Delegate HornBlow { get; }
        public Imports.VoidDelegate DoorOpen { get; }
        public Imports.VoidDelegate DoorClose { get; }
        public Imports.Int32Delegate SetSignal { get; }
        public Imports.SetBeaconDataDelegate SetBeaconData { get; }

        public Library(string path)
        {
            Handle = Imports.LoadLibrary(path);
            if (Handle == IntPtr.Zero)
            {
                int hResult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hResult);
            }

            GetPluginVersion = LibraryProcedure.Load<Imports.SetPluginVersionDelegate>(Handle, nameof(GetPluginVersion));
            if (GetPluginVersion is null)
            {
                throw new InvalidOperationException(string.Format(Resources.Value.CannotCheckVersion.Value, nameof(GetPluginVersion)));
            }
            int version = GetPluginVersion.Invoke();
            if (version != 0x20000)
            {
                string versionText = "0x" + Convert.ToString(version, 16);
                throw new NotSupportedException(string.Format(Resources.Value.VersionNotSupported.Value, versionText));
            }

            Load = LibraryProcedure.Load<Imports.VoidDelegate>(Handle, nameof(Load));
            DisposeProc = LibraryProcedure.Load<Imports.VoidDelegate>(Handle, "Dispose");
            SetVehicleSpec = LibraryProcedure.Load<Imports.SetVehicleSpecDelegate>(Handle, nameof(SetVehicleSpec));
            Initialize = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(Initialize));
            Elapse = LibraryProcedure.Load<Imports.ElapseDelegate>(Handle, nameof(Elapse));
            SetPower = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(SetPower));
            SetBrake = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(SetBrake));
            SetReverser = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(SetReverser));
            KeyDown = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(KeyDown));
            KeyUp = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(KeyUp));
            HornBlow = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(HornBlow));
            DoorOpen = LibraryProcedure.Load<Imports.VoidDelegate>(Handle, nameof(DoorOpen));
            DoorClose = LibraryProcedure.Load<Imports.VoidDelegate>(Handle, nameof(DoorClose));
            SetSignal = LibraryProcedure.Load<Imports.Int32Delegate>(Handle, nameof(SetSignal));
            SetBeaconData = LibraryProcedure.Load<Imports.SetBeaconDataDelegate>(Handle, nameof(SetBeaconData));
        }

        public void Dispose()
        {
            Imports.FreeLibrary(Handle);
        }
    }
}
