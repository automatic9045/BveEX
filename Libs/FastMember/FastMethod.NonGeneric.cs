using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastMember
{
    public partial class FastMethod
    {
        private class NonGeneric : FastMethod
        {
            private readonly Func<object, object[], object> Invoker;
            private readonly Lazy<DynamicMethod> DynamicMethod;

            public override MethodInfo Source { get; }

            public NonGeneric(MethodInfo source)
            {
                Source = source;
                Invoker = ReflectionExpressionGenerator.GenerateMethodInvoker(source);
                DynamicMethod = new Lazy<DynamicMethod>(Source.ToDeclaredOnly);
            }

            public override object Invoke(object instance, object[] args) => Invoker(instance, args);

            public override object InvokeDeclaredOnly(object instance, object[] args)
            {
                return DynamicMethod.Value.Invoke(null, new object[] { instance, args });
            }
        }
    }
}
