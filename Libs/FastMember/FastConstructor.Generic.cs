using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastCaching;

namespace FastMember
{
    public partial class FastConstructor
    {
        private class Generic : FastConstructor
        {
            private readonly Type[] SourceParameterTypes;
            private readonly FastCache<Type[], FastConstructor> MethodCache = new FastCache<Type[], FastConstructor>();

            public override ConstructorInfo Source { get; }

            public Generic(ConstructorInfo source)
            {
                Source = source;
                SourceParameterTypes = source.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
            }

            public override object Invoke(object[] args, Type[] genericTypeArguments)
            {
                if (genericTypeArguments is null || !genericTypeArguments.Any()) throw new ArgumentException(Resources.Value.GenericTypeArgumentsMismatch.Value);

                FastConstructor method = MethodCache.GetOrAdd(genericTypeArguments, _ =>
                {
                    Type constructedType = Source.DeclaringType.MakeGenericType(genericTypeArguments);

                    Type[] parameterTypes = (Type[])SourceParameterTypes.Clone();
                    for (int i = 0; i < parameterTypes.Length; i++)
                    {
                        Type type = parameterTypes[i];
                        if (type.IsGenericParameter)
                        {
                            parameterTypes[i] = genericTypeArguments[type.GenericParameterPosition];
                        }
                    }

                    ConstructorInfo constructedConstructor = constructedType.GetConstructor(parameterTypes);
                    FastConstructor result = Create(constructedConstructor);

                    return result;
                });

                return method.Invoke(args);
            }
        }
    }
}
