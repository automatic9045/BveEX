using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;
using BveEx.PluginHost.Troubleshooting;

namespace BveEx.Troubleshooting
{
    internal class TroubleshooterSet : IDisposable
    {
        private readonly List<ITroubleshooter> Troubleshooters;

        private TroubleshooterSet(List<ITroubleshooter> troubleshooters)
        {
            Troubleshooters = troubleshooters;

            AppDomain.CurrentDomain.FirstChanceException += OnExceptionThrown;
        }

        public static TroubleshooterSet Load()
        {
            string troubleshootersDirectory = Path.Combine(Path.GetDirectoryName(App.Instance.BveExAssembly.Location), "Troubleshooters");
            Directory.CreateDirectory(troubleshootersDirectory);

            List<ITroubleshooter> troubleshooters = new List<ITroubleshooter>();
            string[] filePaths = Directory.GetFiles(troubleshootersDirectory, "*.dll");
            foreach (string filePath in filePaths)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(filePath);
                    IEnumerable<Type> types = assembly.GetTypes()
                        .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterfaces().Contains(typeof(ITroubleshooter)));
                    foreach (Type type in types)
                    {
                        ITroubleshooter instance = (ITroubleshooter)Activator.CreateInstance(type);
                        troubleshooters.Add(instance);
                    }
                }
                catch { }
            }

            return new TroubleshooterSet(troubleshooters);
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.FirstChanceException -= OnExceptionThrown;
        }

        private void OnExceptionThrown(object sender, FirstChanceExceptionEventArgs e)
        {
            Resolve(e.Exception);
        }

        private bool Resolve(Exception exception)
        {
            foreach (ITroubleshooter troubleshooter in Troubleshooters)
            {
                bool result = troubleshooter.Resolve(exception);
                if (result) return true;
            }

            return false;
        }
    }
}
