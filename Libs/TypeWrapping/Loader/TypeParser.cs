using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using FastCaching;
using UnembeddedResources;

namespace TypeWrapping
{
    internal class TypeParser
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<TypeParser>("TypeWrapping");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> IllegalCharacterDetectedInAlias { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> IllegalTypeDetected { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TypeNotFound { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TypeNameFormatNotSupported { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> BracketPairMissing { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        private const char Separator = ';';

        static TypeParser()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly FastCache<string, Type> Cache = new FastCache<string, Type>();
        private readonly ConcurrentDictionary<string, Type> SpecializedTypes;

        private readonly string SpecializedTypeAlias;

        /// <summary>
        /// <see cref="TypeParser"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="specializedTypeAlias">名前空間の指定をせずに型を参照する場合に使用するエイリアス。</param>
        /// <param name="specializeTypes">名前空間の指定をせずに参照可能な型の一覧。</param>
        public TypeParser(string specializedTypeAlias, IEnumerable<Type> specializeTypes)
        {
#if DEBUG
            if (!Regex.IsMatch(specializedTypeAlias, @"^[0-9a-zA-Z]+$"))
            {
                throw new ArgumentException(Resources.Value.IllegalCharacterDetectedInAlias.Value, specializedTypeAlias);
            }
            else if (specializeTypes.Any(t => t.IsArray || t.IsConstructedGenericType))
            {
                throw new ArgumentException(Resources.Value.IllegalTypeDetected.Value, nameof(specializeTypes));
            }
#endif

            SpecializedTypeAlias = specializedTypeAlias + ":";
            SpecializedTypes = new ConcurrentDictionary<string, Type>(specializeTypes.ToDictionary(t => t.FullName.Split('.').Last(), t => t));
        }

        public Type Parse(string text, Converter<Type, Type> specializedTypeConverter = null)
        {
            text = text.Trim();

            Type type = Cache.GetOrAdd(text, key =>
            {
                int specializedTypeAliasIndex = text.IndexOf(SpecializedTypeAlias);
                if (specializedTypeAliasIndex != -1)
                {
                    int startIndex = specializedTypeAliasIndex + SpecializedTypeAlias.Length;
                    (int nodeEndIndex, string specializedTypeName, string childText) = AnalyzeNode(text, startIndex);

                    Type parsedType = ParseSingleSpecializedTypeName(specializedTypeName);
                    Type convertedType = specializedTypeConverter is null ? parsedType : specializedTypeConverter(parsedType);

                    if (!convertedType.AssemblyQualifiedName.StartsWith(convertedType.FullName)) throw new FormatException(string.Format(Resources.Value.TypeNameFormatNotSupported.Value, text));
                    string replaceTo = $"[{convertedType.FullName}{childText ?? ""}{convertedType.AssemblyQualifiedName.Substring(convertedType.FullName.Length)}]";

                    string replacedText = text.Replace(SpecializedTypeAlias + specializedTypeName + childText, replaceTo);
                    return Parse(replacedText, specializedTypeConverter);
                }

                if (text.StartsWith("["))
                {
                    text = text.Substring(1);
                    if (text.EndsWith("]"))
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                    else
                    {
                        throw new FormatException(Resources.Value.BracketPairMissing.Value);
                    }
                }

                Type result = Type.GetType(text);
                return result ?? throw new ArgumentException(string.Format(Resources.Value.TypeNotFound.Value, text));
            });
            return type;
        }

        private (int NodeEndIndex, string TypeName, string NestedText) AnalyzeNode(string text, int startIndex)
        {
            string typeName = null;
            string nestedText = null;

            int nestCount = 0;
            int nestStartIndex = -1;
            int nestEndIndex = -2;
            for (int i = startIndex; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case ',':
                        if (nestCount == 0)
                        {
                            if (typeName is null) typeName = text.Substring(startIndex, i - startIndex);
                            return (i, typeName, nestedText);
                        }
                        break;

                    case '[':
                        if (nestCount == 0)
                        {
                            typeName = text.Substring(startIndex, i - startIndex);
                            nestStartIndex = i;
                        }
                        nestCount++;
                        break;

                    case ']':
                        if (nestCount == 0)
                        {
                            if (typeName is null) typeName = text.Substring(startIndex, i - startIndex);
                            return (i, typeName, nestedText);
                        }
                        else if (nestCount == 1)
                        {
                            nestedText = text.Substring(nestStartIndex, i - nestStartIndex + 1);
                            nestEndIndex = i;
                        }
                        nestCount--;
                        break;
                }

                if (i == nestEndIndex + 1)
                {
                    throw new FormatException(string.Format(Resources.Value.TypeNameFormatNotSupported.Value, text));
                }
            }

            if (nestCount != 0) throw new FormatException(Resources.Value.BracketPairMissing.Value);

            if (typeName is null) typeName = text.Substring(startIndex);
            return (text.Length, typeName, nestedText);
        }

        public Type ParseSingleSpecializedTypeName(string typeName)
        {
            if (!SpecializedTypes.TryGetValue(typeName, out Type type))
            {
                string message = string.Format(Resources.Value.TypeNotFound.Value, typeName);
                throw new ArgumentException(message);
            }

            return type;
        }

        public IEnumerable<Type> ParseArray(string text, Converter<Type, Type> specializedTypeConverter = null)
        {
            switch (text)
            {
                case null:
                case "":
                case "void":
                case "System.Void":
                    return Enumerable.Empty<Type>();

                default:
                    string[] textArray = text.Split(Separator);
                    return textArray.Select(x => Parse(x, specializedTypeConverter));
            }
        }
    }
}
