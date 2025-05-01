using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FastMember
{
    public abstract partial class FastMethod
    {
        public abstract MethodInfo Source { get; }

        public static FastMethod Create(MethodInfo source)
            => source.DeclaringType.IsGenericTypeDefinition ? new Generic(source) : new NonGeneric(source) as FastMethod;

        public abstract object Invoke(object instance, object[] args);

        public abstract object InvokeDeclaredOnly(object instance, object[] args);
    }
}
