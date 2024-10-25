using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveEx.Launcher.Hosting;

using BveEx.Native;
using BveEx.Native.InputDevices;

namespace BveEx.Launcher
{
    public class CoreHostAsInputDevice : IDisposable
    {
        private readonly InputDeviceMain InputDeviceMain;

        public event EventHandler<InputEventArgs> LeverMoved;
        public event EventHandler<InputEventArgs> KeyDown;
        public event EventHandler<InputEventArgs> KeyUp;

        internal CoreHostAsInputDevice(Assembly callerAssembly, TargetBveFinder bveFinder)
        {
            LauncherVersionChecker.Check();

            Assembly launcherAssembly = Assembly.GetExecutingAssembly();
            CallerInfo callerInfo = new CallerInfo(bveFinder.TargetProcess, bveFinder.TargetAppDomain, bveFinder.TargetAssembly, callerAssembly, launcherAssembly);

            InputDeviceMain = new InputDeviceMain(callerInfo);
        }

        public void Dispose() => InputDeviceMain?.Dispose();
        public void Configure(IWin32Window owner) => InputDeviceMain.Configure(owner);
        public void Load(string settingsPath) => InputDeviceMain.Load(settingsPath);
        public void SetAxisRanges(int[][] ranges) => InputDeviceMain.SetAxisRanges(ranges);
        public void Tick() => InputDeviceMain.Tick();
    }
}
