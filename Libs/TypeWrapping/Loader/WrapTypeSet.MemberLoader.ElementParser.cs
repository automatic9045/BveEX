using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using UnembeddedResources;

namespace TypeWrapping
{
    public partial class WrapTypeSet
    {
        private partial class MemberLoader
        {
            private class ElementParser
            {
                private class ResourceSet
                {
                    private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ElementParser>(@"TypeWrapping\WrapTypeSet");

                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> OriginalFieldNotFound { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> WrapperPropertyNotFound { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> FieldWrapperPropertyNotFound { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> WrapperEventNotFound { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> WrapperMethodNotFound { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> OriginalMethodNotFound { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> NonPublic { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> Public { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> Static { get; private set; }
                    [ResourceStringHolder(nameof(Localizer))] public Resource<string> Instance { get; private set; }

                    public ResourceSet()
                    {
                        ResourceLoader.LoadAndSetAll(this);
                    }
                }

                private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

                static ElementParser()
                {
#if DEBUG
                    _ = Resources.Value;
#endif
                }

                private readonly Type WrapperType;
                private readonly Type OriginalType;

                private readonly MemberParser WrapperMemberParser;
                private readonly MemberParser OriginalMemberParser;

                public ElementParser(Type wrapperType, Type originalType)
                {
                    WrapperType = wrapperType;
                    OriginalType = originalType;

                    WrapperMemberParser = new MemberParser(WrapperType);
                    OriginalMemberParser = new MemberParser(OriginalType);
                }

                public FieldInfo GetOriginalField(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetOriginalInfo(source);
                    FieldInfo field = OriginalMemberParser.GetField(name, isNonPublic, isStatic);

                    return field ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.OriginalFieldNotFound.Value,
                        GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name, OriginalType.Name));
                }

                public PropertyInfo GetWrapperProperty(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetWrapperInfo(source);
                    PropertyInfo property = WrapperMemberParser.GetProperty(name, isNonPublic, isStatic);

                    return property ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.WrapperPropertyNotFound.Value,
                        GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name));
                }

                public PropertyInfo GetFieldWrapperProperty(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetInfo(source, "WrapperProperty", "IsWrapperNonPublic", "IsWrapperStatic");
                    PropertyInfo property = WrapperMemberParser.GetProperty(name, isNonPublic, isStatic);

                    return property ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.FieldWrapperPropertyNotFound.Value,
                        GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name));
                }

                public EventInfo GetWrapperEvent(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetWrapperInfo(source);
                    EventInfo @event = WrapperMemberParser.GetEvent(name, isNonPublic, isStatic);

                    return @event ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.WrapperEventNotFound.Value,
                        GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name));
                }

                public MethodInfo GetEventOriginalAddAccessor(XElement source, Type type)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetInfo(source, "OriginalAddAccessor", "IsOriginalAccessorNonPublic", "IsOriginalStatic");
                    MethodInfo method = OriginalMemberParser.GetMethod(name, new Type[] { type }, isNonPublic, isStatic);

                    return method ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.OriginalMethodNotFound.Value,
                        type, GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name, OriginalType.Name));
                }

                public MethodInfo GetEventOriginalRemoveAccessor(XElement source, Type type)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetInfo(source, "OriginalRemoveAccessor", "IsOriginalAccessorNonPublic", "IsOriginalStatic");
                    MethodInfo method = OriginalMemberParser.GetMethod(name, new Type[] { type }, isNonPublic, isStatic);

                    return method ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.OriginalMethodNotFound.Value,
                        type, GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name, OriginalType.Name));
                }

                public FieldInfo GetEventOriginalDelegate(XElement source)
                {
                    (string name, _, bool isStatic) = GetInfo(source, "OriginalDelegateField", "_", "IsOriginalStatic");
                    FieldInfo field = OriginalMemberParser.GetField(name, true, isStatic);

                    return field ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.OriginalFieldNotFound.Value,
                        GetAccessibilityDescription(true, isStatic), name, WrapperType.Name, OriginalType.Name));
                }

                public MethodInfo GetWrapperMethod(XElement source, Type[] types)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetWrapperInfo(source);
                    MethodInfo method = WrapperMemberParser.GetMethod(name, types, isNonPublic, isStatic);

                    return method ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.WrapperMethodNotFound.Value,
                        string.Join(", ", types.Select(t => t.Name)), GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name));
                }

                public MethodInfo GetOriginalMethod(XElement source, Type[] types)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetOriginalInfo(source);
                    MethodInfo method = OriginalMemberParser.GetMethod(name, types, isNonPublic, isStatic);

                    return method ?? throw new KeyNotFoundException(
                        string.Format(Resources.Value.OriginalMethodNotFound.Value,
                        string.Join(", ", types.Select(t => t.Name)), GetAccessibilityDescription(isNonPublic, isStatic), name, WrapperType.Name, OriginalType.Name));
                }

                private string GetAccessibilityDescription(bool isNonPublic, bool isStatic)
                        => (isNonPublic ? Resources.Value.NonPublic : Resources.Value.Public).Value + (isStatic ? Resources.Value.Static : Resources.Value.Instance).Value;


                private (string Name, bool IsNonPublic, bool IsStatic) GetWrapperInfo(XElement element) => GetInfo(element, "Wrapper", "IsWrapperNonPublic", "IsWrapperStatic");
                private (string Name, bool IsNonPublic, bool IsStatic) GetOriginalInfo(XElement element) => GetInfo(element, "Original", "IsOriginalNonPublic", "IsOriginalStatic");

                private (string Name, bool IsNonPublic, bool IsStatic) GetInfo(XElement element, string nameAttributeName, string isNonPublicAttributeName, string isStaticAttributeName)
                {
                    string name = (string)element.Attribute(nameAttributeName);

                    bool isNonPublic = (bool?)element.Attribute(isNonPublicAttributeName) ?? false;
                    bool isStatic = (bool?)element.Attribute(isStaticAttributeName) ?? false;

                    return (name, isNonPublic, isStatic);
                }
            }
        }
    }
}
