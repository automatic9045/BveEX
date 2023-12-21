using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeWrapping
{
    public partial class WrapTypeSet
    {
        private partial class MemberLoader
        {
            private class WrappedTypeResolver : TypeLoaderBase
            {
                private readonly Dictionary<Type, Type> WrapperToOriginal = new Dictionary<Type, Type>();
                private readonly Dictionary<Type, Type> BridgedWrapperToWrapper = new Dictionary<Type, Type>();
                private readonly ConcurrentDictionary<Type, List<Type>> WrapperToBridgedWrapper = new ConcurrentDictionary<Type, List<Type>>();

                private readonly TypeParser WrapperTypeParser;
                private readonly TypeParser OriginalTypeParser;

                public int WrappedTypeCount => IsLoaded ? WrapperToOriginal.Count : throw new InvalidOperationException();
                public IReadOnlyDictionary<Type, List<Type>> BridgingTypes => WrapperToBridgedWrapper;

                public WrappedTypeResolver(XElement root, string targetNamespace,
                    TypeParser wrapperTypeParser, TypeParser originalTypeParser, IDictionary<Type, Type> additionalWrapperToOriginal) : base(root, targetNamespace)
                {
                    WrapperTypeParser = wrapperTypeParser;
                    OriginalTypeParser = originalTypeParser;

                    WrapperToOriginal = new Dictionary<Type, Type>(additionalWrapperToOriginal);
                }

                protected override void LoadDelegates(IEnumerable<XElement> delegateElements, IEnumerable<XElement> parentClassElements) => Load(delegateElements, parentClassElements);

                protected override void LoadEnums(IEnumerable<XElement> enumElements, IEnumerable<XElement> parentClassElements) => Load(enumElements, parentClassElements);

                protected override void LoadClasses(IEnumerable<XElement> classElements, IEnumerable<XElement> parentClassElements) => Load(classElements, parentClassElements);

                private void Load(IEnumerable<XElement> elements, IEnumerable<XElement> parentClassElements)
                {
                    string parentWrapperClassName = GetParentWrapperClassName(parentClassElements);
                    string parentOriginalClassName = GetParentOriginalClassName(parentClassElements);


                    elements.AsParallel().ForAll(element =>
                    {
                        string wrapperTypeName = GetWrapperTypeName(element);
                        string originalTypeName = GetOriginalTypeName(element);

                        Type wrapperType = WrapperTypeParser.ParseSingleSpecializedTypeName(parentWrapperClassName + wrapperTypeName);
                        Type originalType = OriginalTypeParser.ParseSingleSpecializedTypeName(parentOriginalClassName + originalTypeName);

                        lock (WrapperToOriginal) WrapperToOriginal.Add(wrapperType, originalType);

                        XElement bridgesElement = element.Element(TargetNamespace + "Bridges");
                        if (!(bridgesElement is null))
                        {
                            IEnumerable<XElement> bridgeElements = bridgesElement.Elements(TargetNamespace + "Bridge");
                            foreach (XElement bridgeElement in bridgeElements)
                            {
                                string bridgedWrapperTypeName = GetWrapperTypeName(bridgeElement);
                                Type bridgedWrapperType = WrapperTypeParser.ParseSingleSpecializedTypeName(parentWrapperClassName + bridgedWrapperTypeName);

                                List<Type> bridgedTypes = WrapperToBridgedWrapper.GetOrAdd(wrapperType, _ => new List<Type>());
                                lock (bridgedTypes) bridgedTypes.Add(bridgedWrapperType);
                                lock (BridgedWrapperToWrapper) BridgedWrapperToWrapper.Add(bridgedWrapperType, wrapperType);
                            }
                        }
                    });
                }

                public Type GetOriginal(Type wrapper)
                {
                    TypeConverter converter = new TypeConverter(WrapperToOriginal, BridgedWrapperToWrapper);
                    return converter.Convert(wrapper);
                }

                public (Type Wrapper, Type Original) Resolve(string wrapperName)
                {
                    Type wrapper = WrapperTypeParser.ParseSingleSpecializedTypeName(wrapperName);
                    Type original = GetOriginal(wrapper);

                    return (wrapper, original);
                }

                public (Type Wrapper, Type Original) Resolve(XElement typeElement, IEnumerable<XElement> parentClassElements)
                {
                    string wrapperName = GetWrapperTypeName(typeElement);
                    string parentWrapperClassName = GetParentWrapperClassName(parentClassElements);

                    (Type Wrapper, Type Original) typePair = Resolve(parentWrapperClassName + wrapperName);
                    return typePair;
                }


                private static string GetWrapperTypeName(XElement element) => GetAttributeValue(element, "Wrapper");
                private static string GetOriginalTypeName(XElement element) => GetAttributeValue(element, "Original");

                private static string GetAttributeValue(XElement element, string attributeName)
                {
                    string attributeValue = (string)element.Attribute(attributeName);
                    return attributeValue;
                }


                private static string GetParentWrapperClassName(IEnumerable<XElement> parentClassElements) => GetParentClassName(parentClassElements, "Wrapper");
                private static string GetParentOriginalClassName(IEnumerable<XElement> parentClassElements) => GetParentClassName(parentClassElements, "Original");

                private static string GetParentClassName(IEnumerable<XElement> parentClassElements, string attributeName)
                {
                    StringBuilder parentClassNameBuilder = new StringBuilder();

                    foreach (XElement parentClassElement in parentClassElements)
                    {
                        string wrapperTypeName = GetAttributeValue(parentClassElement, attributeName);

                        parentClassNameBuilder.Append(wrapperTypeName);
                        parentClassNameBuilder.Append("+");
                    }

                    string parentClassName = parentClassNameBuilder.ToString();
                    return parentClassName;
                }
            }
        }
    }
}
