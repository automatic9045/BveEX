using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private readonly SplashForm SplashForm;

        [Obsolete]
        public CoreHost CoreHost { get; }

        public VersionSelector()
        {
            SplashForm = new SplashForm();
            if (0 < Application.OpenForms.Count)
            {
                SplashForm.Show(Application.OpenForms[0]);
            }
            else
            {
                SplashForm.Show();
            }

            SplashForm.ProgressText = "AtsEX を探しています...";

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

            if (!atsExAssemblies.Any())
            {
                ErrorDialog.Show(4, $"AtsEX 本体の読込に失敗しました。候補となる AtsEX 本体フォルダが見つかりませんでした。",
                    "AtsEX を再インストールしてください。");
                throw new NotSupportedException();
            }

            atsExAssemblyDirectory = atsExAssemblies.Last().Directory; // TODO: バージョンを選択できるようにする
#endif

            if (!Directory.Exists(atsExAssemblyDirectory))
            {
                ErrorDialog.Show(3, $"AtsEX 本体の読込に失敗しました。フォルダ '{atsExAssemblyDirectory}' が見つかりませんでした。",
                    "AtsEX を再インストールしてください。");
                throw new NotSupportedException();
            }

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

            SplashForm.ProgressText = "アップデートを確認しています...";

            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls) || ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls11))
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }

            UpdateChecker.CheckUpdates();
        }

        [Obsolete]
        public VersionSelector(Assembly callerAssembly) : this()
        {
            CoreHost = new CoreHost(callerAssembly, BveFinder);
        }
    }
}
