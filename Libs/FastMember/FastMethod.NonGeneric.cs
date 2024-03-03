using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastMember
{
    public partial class FastMethod
    {
        private class NonGeneric : FastMethod
        {
            private readonly Func<object, object[], object> Invoker;

            public override MethodInfo Source { get; }

            public NonGeneric(MethodInfo source)
            {
                Source = source;
                Invoker = ReflectionExpressionGenerator.GenerateMethodInvoker(source);
            }

            public override object Invoke(object instance, object[] args) => Invoker(instance, args);
        }
    }
}
