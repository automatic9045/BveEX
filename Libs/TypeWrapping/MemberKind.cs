using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWrapping
{
    public enum MemberKind
    {
        WrapperProperty,
        OriginalField,
        FieldWrapperProperty,
        WrapperEvent,
        EventOriginalAddAccessor,
        EventOriginalRemoveAccessor,
        EventOriginalDelegate,
        WrapperMethod,
        OriginalMethod,
    }

    internal static class MemberKindExtensions
    {
        public static MemberCategory GetCategory(this MemberKind kind)
        {
            switch (kind)
            {
                case MemberKind.WrapperProperty:
                case MemberKind.FieldWrapperProperty:
                case MemberKind.WrapperEvent:
                case MemberKind.WrapperMethod:
                    return MemberCategory.Wrapper;

                case MemberKind.OriginalField:
                case MemberKind.EventOriginalAddAccessor:
                case MemberKind.EventOriginalRemoveAccessor:
                case MemberKind.EventOriginalDelegate:
                case MemberKind.OriginalMethod:
                    return MemberCategory.Original;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
