using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// <see cref="double"/> 型の値を 1 つ記憶します。
    /// </summary>
    public class ValueContainer : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ValueContainer>();

            ValueGetMethod = members.GetSourcePropertyGetterOf(nameof(Value));
            ValueSetMethod = members.GetSourcePropertySetterOf(nameof(Value));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ValueContainer"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ValueContainer(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ValueContainer"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ValueContainer FromSource(object src) => src is null ? null : new ValueContainer(src);

        private static FastMethod ValueGetMethod;
        private static FastMethod ValueSetMethod;
        /// <summary>
        /// 値を取得します。
        /// </summary>
        public double Value
        {
            get => (double)ValueGetMethod.Invoke(Src, null);
            set => ValueSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
