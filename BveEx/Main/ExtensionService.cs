using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx
{
    internal class ExtensionService : IDisposable
    {
        private readonly IExtensionSet Extensions;

        public ExtensionService(IExtensionSet extensions)
        {
            Extensions = extensions;
        }

        public void Dispose()
        {
            foreach (PluginBase extension in Extensions)
            {
                extension.Dispose();
            }
        }

        public void Tick(TimeSpan elapsed)
        {
            foreach (PluginBase extension in Extensions)
            {
                extension.Tick(elapsed);
            }
        }
    }
}
