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
    /// <typeparamref name="T"/> 型の値を伴う、イベントのデータを提供します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueEventArgs<T> : ClassWrapperBase
    {
        private static BveTypeSet BveTypes = null;

        private static void LoadMembers()
        {
            if (!(BveTypes is null)) return;
            BveTypes = ClassWrapperInitializer.LazyInitialize();

            ClassMemberSet members = BveTypes.GetClassInfoOf<ValueEventArgs<T>>();

            Constructor = members.GetSourceConstructor();

            ValueGetMethod = members.GetSourcePropertyGetterOf(nameof(Value));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ValueEventArgs{T}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ValueEventArgs(object src) : base(src)
        {
            LoadMembers();
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ValueEventArgs{T}"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ValueEventArgs<T> FromSource(object src) => src is null ? null : new ValueEventArgs<T>(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="ValueEventArgs{T}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="value"></param>
        public ValueEventArgs(T value) : this(ConstructOriginal(value))
        {
        }
        private static object ConstructOriginal(T value)
        {
            LoadMembers();
            return Constructor.Invoke(new object[] { value });
        }

        private static FastMethod ValueGetMethod;
        /// <summary>
        /// 格納されている値を取得します。
        /// </summary>
        public T Value => (T)ValueGetMethod.Invoke(Src, null);
    }
}
