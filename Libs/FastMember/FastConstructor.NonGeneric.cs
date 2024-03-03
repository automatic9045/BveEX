using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastMember
{
    public partial class FastConstructor
    {
        private class NonGeneric : FastConstructor
        {
            private readonly Func<object[], object> Invoker;

            public override ConstructorInfo Source { get; }

            public NonGeneric(ConstructorInfo source)
            {
                Source = source;
                Invoker = ReflectionExpressionGenerator.GenerateConstructorInvoker(source);
            }

            public override object Invoke(object[] args, Type[] genericTypeArguments)
            {
                if (!(genericTypeArguments is null) && genericTypeArguments.Any()) throw new ArgumentException(Resources.Value.GenericTypeArgumentsMismatch.Value);

                return Invoker(args);
            }
        }
    }
}
