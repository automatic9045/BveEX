using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

using UnembeddedResources;

namespace FastMember
{
    public abstract partial class FastConstructor
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<FastConstructor>(@"FastMember");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> GenericTypeArgumentsMismatch { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static FastConstructor()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public abstract ConstructorInfo Source { get; }

        public static FastConstructor Create(ConstructorInfo source)
            => source.DeclaringType.IsGenericTypeDefinition ? new Generic(source) : new NonGeneric(source) as FastConstructor;

        public abstract object Invoke(object[] args, Type[] genericTypeArguments = null);
    }
}
