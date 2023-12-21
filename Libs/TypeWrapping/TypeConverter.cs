using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace TypeWrapping
{
    public class TypeConverter
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<TypeConverter>("TypeWrapping");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ElementTypesButArrayNotSupported { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static TypeConverter()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly Dictionary<Type, Type> ConvertibleTypes;
        private readonly Dictionary<Type, Type> TypeBridges;

        public TypeConverter(Dictionary<Type, Type> convertibleTypes, Dictionary<Type, Type> typeBridges = null)
        {
            ConvertibleTypes = convertibleTypes;
            TypeBridges = typeBridges ?? new Dictionary<Type, Type>();
        }

        public Type Convert(Type source)
        {
            if (source.IsGenericParameter)
            {
                Type declaringSource = source.DeclaringType;
                Type declaringOriginal = Convert(declaringSource);

                return (declaringOriginal as TypeInfo).GenericTypeParameters[source.GenericParameterPosition];
            }
            else if (source.IsConstructedGenericType)
            {
                Type sourceParent = source.GetGenericTypeDefinition();
                Type originalParent = ParseSimpleType(sourceParent);

                Type[] sourceChildren = source.GetGenericArguments();
                Type[] originalChildren = new Type[sourceChildren.Length];
                for (int i = 0; i < sourceChildren.Length; i++)
                {
                    originalChildren[i] = ParseSimpleType(sourceChildren[i]);
                }

                Type result = originalParent.MakeGenericType(originalChildren);
                return result;
            }
            else if (source.HasElementType)
            {
                if (!source.IsArray || source.Name.Contains("*")) throw new NotSupportedException(Resources.Value.ElementTypesButArrayNotSupported.Value);

                Type sourceElement = source.GetElementType();
                Type originalElement = ParseSimpleType(sourceElement);

                int rank = source.GetArrayRank();
                Type result = rank == 1 ? originalElement.MakeArrayType() : originalElement.MakeArrayType(rank);
                return result;
            }
            else
            {
                return ParseSimpleType(source);
            }


            Type ParseSimpleType(Type simpleSource, bool allowBridgedTypes = true)
            {
                if (!ConvertibleTypes.TryGetValue(simpleSource, out Type result))
                {
                    if (allowBridgedTypes && TypeBridges.TryGetValue(simpleSource, out Type bridgedTo))
                    {
                        return ParseSimpleType(bridgedTo, false);
                    }
                }

                return result ?? simpleSource;
            }
        }
    }
}
