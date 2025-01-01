using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mackoy.Bvets;

using BveEx.Launcher;

namespace BveEx.Caller.InputDevice
{
    public class InputDevice : IInputDevice
    {
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        private const string LauncherName = "BveEx.Launcher";
        private const string ErrorCaption = "読込エラー - BveEX Caller";

        private VersionSelector VersionSelector;

        public event InputEventHandler LeverMoved;
        public event InputEventHandler KeyDown;
        public event InputEventHandler KeyUp;

        public InputDevice()
        {
#if DEBUG
            if (!Debugger.IsAttached) Debugger.Launch();
#endif
            string callerDirectory = Path.GetDirectoryName(Assembly.Location);
            string textPath = Path.Combine(callerDirectory, "BveEx.Caller.InputDevice.txt");

            string bveExDirectory = Path.Combine(callerDirectory, "BveEx");
            if (File.Exists(textPath))
            {
                using (StreamReader sr = new StreamReader(textPath))
                {
                    string line = sr.ReadLine();
                    bveExDirectory = Path.Combine(callerDirectory, line);
                }
            }

            string launcherLocation = Path.Combine(bveExDirectory, LauncherName + ".dll");
            if (!File.Exists(launcherLocation))
            {
                MessageBox.Show($"エラーコード CI-1: BveEX Launcher が見つかりません。\n\n指定された場所:\n{launcherLocation}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Assembly AssemblyResolve(object sender, ResolveEventArgs e)
            {
                AssemblyName assemblyName = new AssemblyName(e.Name);
                if (assemblyName.Name == LauncherName)
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
                    return Assembly.LoadFrom(launcherLocation);
                }
                else
                {
                    return null;
                }
            }

            void Load()
            {
                VersionSelector = new VersionSelector(Assembly);

                VersionSelector.CoreHost.LeverMoved += (sender, e) => LeverMoved?.Invoke(this, new Mackoy.Bvets.InputEventArgs(e.Axis, e.Value));
                VersionSelector.CoreHost.KeyDown += (sender, e) => KeyDown?.Invoke(this, new Mackoy.Bvets.InputEventArgs(e.Axis, e.Value));
                VersionSelector.CoreHost.KeyUp += (sender, e) => KeyUp?.Invoke(this, new Mackoy.Bvets.InputEventArgs(e.Axis, e.Value));
            }
        }

        public void Dispose()
        {
            VersionSelector?.Dispose();
        }

        public void Configure(IWin32Window owner) => VersionSelector.CoreHost.Configure(owner);
        public void Load(string settingsPath) => VersionSelector.CoreHost.Load(settingsPath);
        public void SetAxisRanges(int[][] ranges) => VersionSelector.CoreHost.SetAxisRanges(ranges);
        public void Tick() => VersionSelector.CoreHost.Tick();
    }
}
