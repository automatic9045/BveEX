using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeWrapping
{
    public partial class WrapTypeSet
    {
        private partial class MemberLoader
        {
            private class ElementParser
            {
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

                public PropertyInfo GetWrapperProperty(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetWrapperInfo(source);
                    PropertyInfo property = WrapperMemberParser.GetProperty(name, isNonPublic, isStatic);

                    return property ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.WrapperProperty, name, isNonPublic, isStatic);
                }

                public FieldInfo GetOriginalField(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetOriginalInfo(source);
                    FieldInfo field = OriginalMemberParser.GetField(name, isNonPublic, isStatic);

                    return field ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.OriginalField, name, isNonPublic, isStatic);
                }

                public PropertyInfo GetFieldWrapperProperty(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetInfo(source, "WrapperProperty", "IsWrapperNonPublic", "IsWrapperStatic");
                    PropertyInfo property = WrapperMemberParser.GetProperty(name, isNonPublic, isStatic);

                    return property ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.FieldWrapperProperty, name, isNonPublic, isStatic);
                }

                public EventInfo GetWrapperEvent(XElement source)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetWrapperInfo(source);
                    EventInfo @event = WrapperMemberParser.GetEvent(name, isNonPublic, isStatic);

                    return @event ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.WrapperEvent, name, isNonPublic, isStatic);
                }

                public MethodInfo GetEventOriginalAddAccessor(XElement source, Type type)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetInfo(source, "OriginalAddAccessor", "IsOriginalAccessorNonPublic", "IsOriginalStatic");
                    MethodInfo method = OriginalMemberParser.GetMethod(name, new Type[] { type }, isNonPublic, isStatic);

                    return method ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.EventOriginalAddAccessor, name, isNonPublic, isStatic);
                }

                public MethodInfo GetEventOriginalRemoveAccessor(XElement source, Type type)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetInfo(source, "OriginalRemoveAccessor", "IsOriginalAccessorNonPublic", "IsOriginalStatic");
                    MethodInfo method = OriginalMemberParser.GetMethod(name, new Type[] { type }, isNonPublic, isStatic);

                    return method ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.EventOriginalRemoveAccessor, name, isNonPublic, isStatic);
                }

                public FieldInfo GetEventOriginalDelegate(XElement source)
                {
                    (string name, _, bool isStatic) = GetInfo(source, "OriginalDelegateField", "_", "IsOriginalStatic");
                    FieldInfo field = OriginalMemberParser.GetField(name, true, isStatic);

                    return field ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.EventOriginalDelegate, name, true, isStatic);
                }

                public MethodInfo GetWrapperMethod(XElement source, Type[] types)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetWrapperInfo(source);
                    MethodInfo method = WrapperMemberParser.GetMethod(name, types, isNonPublic, isStatic);

                    return method ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.EventOriginalDelegate, name, isNonPublic, isStatic, types);
                }

                public MethodInfo GetOriginalMethod(XElement source, Type[] types)
                {
                    (string name, bool isNonPublic, bool isStatic) = GetOriginalInfo(source);
                    MethodInfo method = OriginalMemberParser.GetMethod(name, types, isNonPublic, isStatic);

                    return method ?? throw new MemberNotFoundException(WrapperType, OriginalType, MemberKind.EventOriginalDelegate, name, isNonPublic, isStatic, types);
                }


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
