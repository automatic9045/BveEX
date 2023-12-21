using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastCaching;
using FastMember;

namespace TypeWrapping
{
    internal class ConstructedGenericClassMemberSet : ClassMemberSet
    {
        private readonly ClassMemberSet DefinitionMembers;

        private readonly FastCache<FastMethod, FastMethod> MethodCache = new FastCache<FastMethod, FastMethod>();
        private readonly FastCache<FastField, FastField> FieldCache = new FastCache<FastField, FastField>();
        private readonly FastCache<FastEvent, FastEvent> EventCache = new FastCache<FastEvent, FastEvent>();
        private readonly FastCache<FastConstructor, FastConstructor> ConstructorCache = new FastCache<FastConstructor, FastConstructor>();

        public ConstructedGenericClassMemberSet(Type wrapperType, Type originalType, ClassMemberSet definitionMembers) : base(wrapperType, originalType)
        {
            DefinitionMembers = definitionMembers;
        }

        public override FastMethod GetSourcePropertyGetterOf(string wrapperName)
        {
            FastMethod definition = DefinitionMembers.GetSourcePropertyGetterOf(wrapperName);
            FastMethod constructed = GetOrCreateMethod(definition);
            return constructed;
        }

        public override FastMethod GetSourcePropertySetterOf(string wrapperName)
        {
            FastMethod definition = DefinitionMembers.GetSourcePropertySetterOf(wrapperName);
            FastMethod constructed = GetOrCreateMethod(definition);
            return constructed;
        }

        public override FastField GetSourceFieldOf(string wrapperName)
        {
            FastField definition = DefinitionMembers.GetSourceFieldOf(wrapperName);

            FastField constructed = FieldCache.GetOrAdd(definition, _ =>
            {
                BindingFlags bindingFlags = BindingFlags.Default;
                bindingFlags |= definition.Source.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic | BindingFlags.InvokeMethod;
                bindingFlags |= definition.Source.IsStatic ? BindingFlags.Static : BindingFlags.Instance;

                FieldInfo field = OriginalType.GetField(wrapperName, bindingFlags);
                return FastField.Create(field);
            });

            return constructed;
        }

        public override FastEvent GetSourceEventOf(string wrapperName)
        {
            throw new NotImplementedException();
        }

        public override FastConstructor GetSourceConstructor(Type[] parameters = null)
        {
            FastConstructor definition = DefinitionMembers.GetSourceConstructor(parameters);

            FastConstructor constructed = ConstructorCache.GetOrAdd(definition, _ =>
            {
                BindingFlags bindingFlags = BindingFlags.Instance;
                bindingFlags |= definition.Source.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic | BindingFlags.InvokeMethod;

                Type[] parameterTypes = ConvertParameterTypes(definition.Source);

                ConstructorInfo constructor = OriginalType.GetConstructor(bindingFlags, null, parameterTypes, null);
                return FastConstructor.Create(constructor);
            });

            return constructed;
        }

        public override FastMethod GetSourceMethodOf(string wrapperName, Type[] parameters = null)
        {
            FastMethod definition = DefinitionMembers.GetSourceMethodOf(wrapperName, parameters);
            FastMethod constructed = GetOrCreateMethod(definition);
            return constructed;
        }

        private FastMethod GetOrCreateMethod(FastMethod original)
        {
            MethodInfo originalMethod = original.Source;
            if (originalMethod.IsGenericMethodDefinition) throw new NotSupportedException();

            FastMethod constructed = MethodCache.GetOrAdd(original, _ =>
            {
                BindingFlags bindingFlags = BindingFlags.Default;
                bindingFlags |= originalMethod.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic | BindingFlags.InvokeMethod;
                bindingFlags |= originalMethod.IsStatic ? BindingFlags.Static : BindingFlags.Instance;

                Type[] parameterTypes = ConvertParameterTypes(originalMethod);

                MethodInfo method = OriginalType.GetMethod(originalMethod.Name, bindingFlags, null, parameterTypes, null);
                return FastMethod.Create(method);
            });

            return constructed;
        }

        private Type[] ConvertParameterTypes(MethodBase originalMethod)
        {
            return originalMethod
                .GetParameters()
                .AsParallel()
                .Select(parameter =>
                {
                    Type originalParameterType = parameter.ParameterType;
                    if (originalParameterType.IsGenericParameter)
                    {
                        int genericParameterPosition = originalParameterType.GenericParameterPosition;
                        Type parameterType = OriginalType.GenericTypeArguments[genericParameterPosition];

                        return parameterType;
                    }
                    else
                    {
                        return originalParameterType;
                    }
                })
                .ToArray();
        }
    }
}
