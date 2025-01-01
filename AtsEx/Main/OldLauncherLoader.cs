using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;

namespace AtsEx
{
    internal class OldLauncherLoader : IDisposable
    {
        private static readonly string FileName = "AtsEx.Launcher";


        private readonly string OldLauncherPath;

        public OldLauncherLoader()
        {
            string launcherPath = App.Instance.AtsExLauncherAssembly.Location;
            OldLauncherPath = Path.Combine(Path.GetDirectoryName(launcherPath), FileName + ".dll");

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs e)
        {
            AssemblyName assemblyName = new AssemblyName(e.Name);
            if (assemblyName.Name != FileName) return null;

            Assembly assembly = Assembly.LoadFrom(OldLauncherPath);
            return assembly;
        }
    }
}
