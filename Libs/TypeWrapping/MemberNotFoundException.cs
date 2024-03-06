using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWrapping
{
    public partial class MemberNotFoundException : KeyNotFoundException
    {
        public Type WrapperType { get; }
        public Type OriginalType { get; }

        public MemberKind Kind { get; }
        public MemberCategory Category { get; }

        public string Name { get; }
        public bool IsNonPublic { get; }
        public bool IsStatic { get; }
        public Type[] ParameterTypes { get; }

        public MemberNotFoundException(Type wrapperType, Type originalType, MemberKind kind, string name, bool isNonPublic, bool isStatic, Type[] parameterTypes = null)
            : base(MessageFactory.CreateMessage(wrapperType, originalType, kind, name, isNonPublic, isStatic, parameterTypes))
        {
            WrapperType = wrapperType;
            OriginalType = originalType;

            Kind = kind;
            Category = kind.GetCategory();

            Name = name;
            IsNonPublic = isNonPublic;
            IsStatic = isStatic;
            ParameterTypes = parameterTypes;
        }
    }
}
