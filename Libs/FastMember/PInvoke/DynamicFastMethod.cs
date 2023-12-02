using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace FastMember.PInvoke
{
    public class DynamicFastMethod : FastMethod
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<DynamicFastMethod>("FastMember");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotCallBeforeCompile { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static DynamicFastMethod()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private FastMethod CompiledMethod;

        public string Name { get; }
        public bool IsCompiled { get; private set; } = false;
        public override MethodInfo Source
            => IsCompiled ? CompiledMethod.Source : throw new InvalidOperationException(Resources.Value.CannotCallBeforeCompile.Value);

        internal DynamicFastMethod(string name)
        {
            Name = name;
        }

        internal void Compile(FastMethod compiledMethod)
        {
            CompiledMethod = compiledMethod;
            IsCompiled = true;
        }

        public override object Invoke(object instance, object[] args) 
            => IsCompiled ? CompiledMethod.Invoke(instance, args) : throw new InvalidOperationException(Resources.Value.CannotCallBeforeCompile.Value);
    }
}
