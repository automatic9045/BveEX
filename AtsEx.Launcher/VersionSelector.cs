using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Launcher.Hosting;

namespace AtsEx.Launcher
{
    public partial class VersionSelector
    {
        private static readonly TargetBveFinder BveFinder = new TargetBveFinder();

        static VersionSelector()
        {
#if DEBUG
            if (!Debugger.IsAttached) Debugger.Launch();
#endif
        }

        [Obsolete]
        public CoreHost CoreHost { get; }

        public VersionSelector()
        {
            Assembly launcherAssembly = Assembly.GetExecutingAssembly();
            string rootDirectory = Path.GetDirectoryName(launcherAssembly.Location);

            string atsExAssemblyDirectory;
#if DEBUG
            atsExAssemblyDirectory = Path.Combine(rootDirectory, "Debug");
#else
            IEnumerable<string> availableDirectories = Directory.GetDirectories(rootDirectory).Where(x => x.Contains('.'));
            IEnumerable<(string Directory, AssemblyName AssemblyName)> atsExAssemblies = availableDirectories
                .Select(x => (Directory: x, Location: Path.Combine(x, "AtsEx.dll")))
                .Where(x => File.Exists(x.Location))
                .Select(x => (x.Directory, AssemblyName: AssemblyName.GetAssemblyName(x.Location)))
                .OrderBy(x => x.AssemblyName.Version);

            atsExAssemblyDirectory = atsExAssemblies.Last().Directory; // TODO: バージョンを選択できるようにする
#endif

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                AssemblyName assemblyName = new AssemblyName(e.Name);
                switch (assemblyName.Name)
                {
                    case "AtsEx.Diagnostics":
                        string diagnosticsPath = Path.Combine(rootDirectory, assemblyName.Name + ".dll");
                        return Assembly.LoadFrom(diagnosticsPath);

                    default:
                        string path = Path.Combine(atsExAssemblyDirectory, assemblyName.Name + ".dll");
                        return File.Exists(path) ? Assembly.LoadFrom(path) : null;
                }
            };

            UpdateChecker.CheckUpdates();
        }

        [Obsolete]
        public VersionSelector(Assembly callerAssembly) : this()
        {
            CoreHost = new CoreHost(callerAssembly, BveFinder);
        }
    }
}
