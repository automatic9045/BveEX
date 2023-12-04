using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace AtsEx.Plugins.Native
{
    internal class AtsPluginVersionNotSupportedException : Exception
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<AtsPluginVersionNotSupportedException>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Message { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static AtsPluginVersionNotSupportedException()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public int Version { get; }

        public AtsPluginVersionNotSupportedException(int version) : base(string.Format(Resources.Value.Message.Value, version))
        {
            Version = version;
        }
    }
}
