using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AtsEx.Native;
using AtsEx.Native.InputDevices;

using BveEx.Launcher.Hosting;

namespace BveEx.Launcher
{
    internal class LegacyCoreHost : ICoreHost
    {
        private readonly InputDeviceMain Core = null;

        public event EventHandler<InputEventArgs> LeverMoved;
        public event EventHandler<InputEventArgs> KeyDown;
        public event EventHandler<InputEventArgs> KeyUp;

        public LegacyCoreHost(Assembly callerAssembly, TargetBveFinder bveFinder)
        {
            Assembly launcherAssembly = Assembly.GetExecutingAssembly();

            CallerInfo callerInfo = new CallerInfo(bveFinder.TargetProcess, bveFinder.TargetAppDomain, bveFinder.TargetAssembly, callerAssembly, launcherAssembly);
            Core = new InputDeviceMain(callerInfo);
        }

        public void Dispose() => Core?.Dispose();
        public void Configure(IWin32Window owner) => Core.Configure(owner);
        public void Load(string settingsPath) => Core.Load(settingsPath);
        public void SetAxisRanges(int[][] ranges) => Core.SetAxisRanges(ranges);
        public void Tick() => Core.Tick();
    }
}
