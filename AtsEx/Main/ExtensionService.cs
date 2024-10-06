using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx
{
    internal class ExtensionService : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ExtensionService>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ExtensionTickResultTypeInvalid { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static ExtensionService()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


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
                TickResult tickResult = extension.Tick(elapsed);
                if (!(tickResult is ExtensionTickResult))
                {
                    throw new InvalidOperationException(string.Format(Resources.Value.ExtensionTickResultTypeInvalid.Value,
                       $"{nameof(PluginBase)}.{nameof(PluginBase.Tick)}", nameof(ExtensionTickResult)));
                }
            }
        }
    }
}
