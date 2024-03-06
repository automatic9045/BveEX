using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace TypeWrapping
{
    public partial class MemberNotFoundException
    {
        private static class MessageFactory
        {
            private class ResourceSet
            {
                private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<MemberNotFoundException>(@"TypeWrapping");

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

            static MessageFactory()
            {
#if DEBUG
                _ = Resources.Value;
#endif
            }


            /// <summary>
            /// 例外を説明するメッセージを作成します。
            /// </summary>
            /// <remarks>
            /// 各インデックスに対応するテキストは以下の通りです:
            /// <list type="bullet">
            /// <item>
            /// <term>0</term>
            /// <description>アクセシビリティ</description>
            /// </item>
            /// <item>
            /// <term>1</term>
            /// <description>メンバー名</description>
            /// </item>
            /// <item>
            /// <term>2</term>
            /// <description>ラッパー型名</description>
            /// </item>
            /// <item>
            /// <term>3</term>
            /// <description>オリジナル型名</description>
            /// </item>
            /// <item>
            /// <term>4</term>
            /// <description>(メソッドのみ) パラメータ型名</description>
            /// </item>
            /// </list>
            /// </remarks>
            public static string CreateMessage(Type wrapperType, Type originalType, MemberKind kind, string name, bool isNonPublic, bool isStatic, Type[] parameterTypes)
            {
                Resource<string> messageResource = GetMessageResource();
                string accessibilityText = (isNonPublic ? Resources.Value.NonPublic : Resources.Value.Public).Value + (isStatic ? Resources.Value.Static : Resources.Value.Instance).Value;
                string parameterText = parameterTypes is null ? null : string.Join(", ", parameterTypes.Select(t => t.Name));

                string message = string.Format(messageResource.Value, accessibilityText, name, wrapperType.Name, originalType.Name, parameterText);
                return message;


                Resource<string> GetMessageResource()
                {
                    switch (kind)
                    {
                        case MemberKind.WrapperProperty:
                            return Resources.Value.WrapperPropertyNotFound;

                        case MemberKind.OriginalField:
                            return Resources.Value.OriginalFieldNotFound;

                        case MemberKind.FieldWrapperProperty:
                            return Resources.Value.FieldWrapperPropertyNotFound;

                        case MemberKind.WrapperEvent:
                            return Resources.Value.WrapperEventNotFound;

                        case MemberKind.EventOriginalAddAccessor:
                            return Resources.Value.OriginalMethodNotFound;

                        case MemberKind.EventOriginalRemoveAccessor:
                            return Resources.Value.OriginalMethodNotFound;

                        case MemberKind.EventOriginalDelegate:
                            return Resources.Value.OriginalFieldNotFound;

                        case MemberKind.WrapperMethod:
                            return Resources.Value.WrapperMethodNotFound;

                        case MemberKind.OriginalMethod:
                            return Resources.Value.OriginalMethodNotFound;

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }
    }
}
