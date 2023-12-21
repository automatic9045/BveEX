using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;

namespace TypeWrapping
{
    public abstract class ClassMemberSet : TypeMemberSetBase
    {
        internal ClassMemberSet(Type wrapperType, Type originalType) : base(wrapperType, originalType)
        {
        }

        public abstract FastMethod GetSourcePropertyGetterOf(string wrapperName);
        public abstract FastMethod GetSourcePropertySetterOf(string wrapperName);
        public abstract FastField GetSourceFieldOf(string wrapperName);
        public abstract FastEvent GetSourceEventOf(string wrapperName);
        public abstract FastConstructor GetSourceConstructor(Type[] parameters = null);
        public abstract FastMethod GetSourceMethodOf(string wrapperName, Type[] parameters = null);
    }
}
