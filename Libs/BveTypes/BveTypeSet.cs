using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastCaching;
using FastMember;
using TypeWrapping;
using UnembeddedResources;

using BveTypes.ClassWrappers;

namespace BveTypes
{
    /// <summary>
    /// クラスラッパーに対応する BVE の型とメンバーの情報を提供します。
    /// </summary>
    public sealed partial class BveTypeSet
    {
        private partial class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<BveTypeSet>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TypeNotClassWrapper { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static BveTypeSet()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly WrapTypeSet Types;
        private readonly FastCache<Type, FastMethod> FromSourceMethodCache = new FastCache<Type, FastMethod>();

        private BveTypeSet(WrapTypeSet types, Version profileVersion)
        {
#if DEBUG
            TypeMemberSetBase illegalType = types.Types.Values.FirstOrDefault(type =>
            {
                if (type.WrapperType.IsInterface || type.WrapperType.IsSubclassOf(typeof(Delegate)) || type.WrapperType.IsEnum) return false;

                bool isClassWrapperBaseSubclass = type.WrapperType.IsClass && type.WrapperType.IsSubclassOf(typeof(ClassWrapperBase));
                return !isClassWrapperBaseSubclass;
            });
            if (!(illegalType is null))
            {
                throw new ArgumentException(
                    string.Format(Resources.Value.TypeNotClassWrapper.Value,
                    illegalType.WrapperType.FullName, typeof(ClassWrapperBase).FullName));
            }
#endif

            Types = types;
            ProfileVersion = profileVersion;
        }


        /// <summary>
        /// 読み込まれたプロファイルが対応している BVE のバージョンを取得します。
        /// </summary>
        public Version ProfileVersion { get; }


        /// <summary>
        /// <typeparamref name="TWrapper"/> に指定したラッパー型の情報を取得します。
        /// </summary>
        /// <typeparam name="TWrapper">ラッパー型。</typeparam>
        /// <returns><typeparamref name="TWrapper"/> に指定したラッパー型の情報を表す <see cref="TypeMemberSetBase"/>。</returns>
        /// <seealso cref="GetClassInfoOf{TWrapper}"/>
        /// <seealso cref="GetClassInfoOf(Type)"/>
        /// <seealso cref="GetEnumInfoOf{TWrapper}"/>
        /// <seealso cref="GetEnumInfoOf(Type)"/>
        public TypeMemberSetBase GetTypeInfoOf<TWrapper>() => GetTypeInfoOf(typeof(TWrapper));

        /// <summary>
        /// <paramref name="wrapperType"/> に指定したラッパー型の情報を取得します。
        /// </summary>
        /// <param name="wrapperType">ラッパー型。</param>
        /// <returns><paramref name="wrapperType"/> に指定したラッパー型の情報を表す <see cref="TypeMemberSetBase"/>。</returns>
        /// <seealso cref="GetClassInfoOf{TWrapper}"/>
        /// <seealso cref="GetClassInfoOf(Type)"/>
        /// <seealso cref="GetEnumInfoOf{TWrapper}"/>
        /// <seealso cref="GetEnumInfoOf(Type)"/>
        public TypeMemberSetBase GetTypeInfoOf(Type wrapperType)
        {
            if (wrapperType.IsConstructedGenericType)
            {
                TypeMemberSetBase members = Types.MakeGenericType(wrapperType);
                return members;
            }
            else
            {
                return Types.Types.TryGetValue(wrapperType, out TypeMemberSetBase typeInfo) ? typeInfo : Types.Bridge[wrapperType];
            }
        }


        /// <summary>
        /// <typeparamref name="TWrapper"/> に指定したラッパー列挙型の情報を取得します。
        /// </summary>
        /// <typeparam name="TWrapper">ラッパー列挙型。</typeparam>
        /// <returns><typeparamref name="TWrapper"/> に指定したラッパー列挙型の情報を表す <see cref="EnumMemberSet"/>。</returns>
        public EnumMemberSet GetEnumInfoOf<TWrapper>() => (EnumMemberSet)GetTypeInfoOf<TWrapper>();

        /// <summary>
        /// <paramref name="wrapperType"/> に指定したラッパー列挙型の情報を取得します。
        /// </summary>
        /// <param name="wrapperType">ラッパー列挙型。</param>
        /// <returns><paramref name="wrapperType"/> に指定したラッパー列挙型の情報を表す <see cref="EnumMemberSet"/>。</returns>
        public EnumMemberSet GetEnumInfoOf(Type wrapperType) => (EnumMemberSet)GetTypeInfoOf(wrapperType);


        /// <summary>
        /// <typeparamref name="TWrapper"/> に指定したラッパークラスの情報を取得します。
        /// </summary>
        /// <typeparam name="TWrapper">ラッパークラス。</typeparam>
        /// <returns><typeparamref name="TWrapper"/> に指定したラッパークラスの情報を表す <see cref="ClassMemberSet"/>。</returns>
        public ClassMemberSet GetClassInfoOf<TWrapper>() => (ClassMemberSet)GetTypeInfoOf<TWrapper>();

        /// <summary>
        /// <paramref name="wrapperType"/> に指定したラッパークラスの情報を取得します。
        /// </summary>
        /// <param name="wrapperType">ラッパークラス。</param>
        /// <returns><paramref name="wrapperType"/> に指定したラッパークラスの情報を表す <see cref="ClassMemberSet"/>。</returns>
        public ClassMemberSet GetClassInfoOf(Type wrapperType) => (ClassMemberSet)GetTypeInfoOf(wrapperType);


        /// <summary>
        /// <paramref name="originalType"/> に指定したオリジナル型のラッパー型を取得します。
        /// </summary>
        /// <param name="originalType">オリジナル型。</param>
        /// <returns><paramref name="originalType"/> のラッパー型。</returns>
        public Type GetWrapperTypeOf(Type originalType)
            => TryGetWrapperTypeOf(originalType, out Type wrapperType) ? wrapperType : throw new KeyNotFoundException();

        /// <summary>
        /// <paramref name="originalType"/> に指定したオリジナル型のラッパー型を取得します。
        /// </summary>
        /// <param name="originalType">オリジナル型。</param>
        /// <param name="wrapperType"><paramref name="originalType"/> のラッパー型が見つかった場合はラッパー型を表す <see cref="Type"/>、見つからなかった場合は <see langword="null"/> が格納されます。</param>
        /// <returns><paramref name="originalType"/> のラッパー型が見つかった場合は <see langword="true"/>、見つからなかった場合は <see langword="false"/>。</returns>
        public bool TryGetWrapperTypeOf(Type originalType, out Type wrapperType)
        {
            Type result = Types.OriginalToWrapperConverter.Convert(originalType);
            if (result == originalType)
            {
                wrapperType = null;
                return false;
            }
            else
            {
                wrapperType = result;
                return true;
            }
        }


        internal FastMethod GetCreateFromSourceMethod(Type wrapperType)
        {
            if (wrapperType is null) throw new ArgumentNullException(nameof(wrapperType));

            FastMethod result = FromSourceMethodCache.GetOrAdd(wrapperType, type =>
            {
                MethodInfo fromSourceMethod = wrapperType.
                    GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod).
                    FirstOrDefault(method =>
                    {
                        if (method.GetCustomAttribute<CreateClassWrapperFromSourceAttribute>() is null) return false;

                        ParameterInfo[] parameters = method.GetParameters();
                        return parameters.Length == 1 && parameters[0].ParameterType == typeof(object) && method.ReturnType == wrapperType;
                    });

                return fromSourceMethod is null ? null : FastMethod.Create(fromSourceMethod);
            });
            return result;
        }
    }
}
