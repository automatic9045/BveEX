using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;

namespace BveTypes.ClassWrappers.Extensions
{
    /// <summary>
    /// オリジナル型の <see cref="List{T}"/> のラッパーを表します。
    /// </summary>
    /// <typeparam name="TWrapper">値のオリジナル型に対応するラッパー型。</typeparam>
    /// <seealso cref="SortedList{TKey, TValue}"/>
    [AdditionalTypeWrapper(typeof(List<>))]
    public class WrappedList<TWrapper> : ClassWrapperBase, IList<TWrapper>, IReadOnlyList<TWrapper>, IList
    {
        private static BveTypeSet BveTypes = null;
        private static ListConstructorSet ListConstructors = null;

        private static void LoadConstructors()
        {
            Type originalType = BveTypes.GetTypeInfoOf<TWrapper>().OriginalType;
            Type listType = typeof(List<>).MakeGenericType(originalType);
            ListConstructors = new ListConstructorSet(listType);
        }


        /// <summary>
        /// ラップされているオリジナル オブジェクトです。
        /// </summary>
        protected readonly new IList Src;

        /// <summary>
        /// オリジナル型とラッパー型を相互に変換するためのコンバータです。
        /// </summary>
        protected readonly ITwoWayConverter<object, TWrapper> Converter;

        /// <summary>
        /// <see cref="WrappedList{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップされているオリジナル オブジェクト。</param>
        /// <param name="converter">オリジナル型とラッパー型を相互に変換するためのコンバータ。</param>
        /// <exception cref="ArgumentException"><paramref name="src"/> が <see cref="IList{T}"/> の派生ではありません。</exception>
        protected WrappedList(IList src, ITwoWayConverter<object, TWrapper> converter) : base(src)
        {
            if (BveTypes is null)
            {
                BveTypes = ClassWrapperInitializer.LazyInitialize();
                LoadConstructors();
            }

            Src = src;
            if (!IsSubclassOfGeneric(Src.GetType(), typeof(List<>))) throw new ArgumentException();

            Converter = converter;


            bool IsSubclassOfGeneric(Type current, Type genericBase)
            {
                do
                {
                    if (current.IsGenericType && current.GetGenericTypeDefinition() == genericBase) return true;
                }
                while ((current = current.BaseType) != null);
                return false;
            }
        }

        /// <summary>
        /// 既定のコンバータを使用して、<see cref="WrappedList{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src"></param>
        protected WrappedList(IList src) : this(src, new ClassWrapperConverter<TWrapper>())
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <param name="converter">オリジナル型とラッパー型を相互に変換するための <see cref="ITwoWayConverter{T1, T2}"/>。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="WrappedList{TWrapper}"/> クラスのインスタンス。</returns>
        public static WrappedList<TWrapper> FromSource(IList src, ITwoWayConverter<object, TWrapper> converter) => src is null ? null : new WrappedList<TWrapper>(src, converter);

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="WrappedList{TWrapper}"/> クラスのインスタンス。</returns>
        public static WrappedList<TWrapper> FromSource(IList src) => FromSource(src, new ClassWrapperConverter<TWrapper>());

        /// <summary>
        /// 空の <see cref="WrappedList{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public WrappedList() : base(ListConstructors.Create())
        {
        }

        /// <summary>
        /// 指定したコレクションからコピーした要素を格納し、コピーされる要素の数を格納できるだけの容量を備えた <see cref="WrappedList{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="collection">要素のコピー元となるコレクション。</param>
        public WrappedList(IEnumerable collection) : base(ListConstructors.Create(collection))
        {
        }

        /// <summary>
        /// 空で、指定した初期量を備えた <see cref="WrappedList{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="capacity">新しいリストに格納できる要素の数。</param>
        public WrappedList(int capacity) : base(ListConstructors.Create(capacity))
        {
        }

        /// <inheritdoc/>
        public TWrapper this[int index]
        {
            get => Converter.Convert(Src[index]);
            set => Src[index] = Converter.ConvertBack(value);
        }

        /// <inheritdoc/>
        public int Count => Src.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => Src.IsReadOnly;

        bool IList.IsFixedSize => Src.IsFixedSize;
        object ICollection.SyncRoot => Src.SyncRoot;
        bool ICollection.IsSynchronized => Src.IsSynchronized;

        /// <inheritdoc/>
        public virtual void Add(TWrapper item) => Src.Add(Converter.ConvertBack(item));

        /// <inheritdoc/>
        public void Clear() => Src.Clear();

        /// <inheritdoc/>
        public bool Contains(TWrapper item) => Src.Contains(Converter.ConvertBack(item));

        /// <inheritdoc/>
        public void CopyTo(TWrapper[] array, int arrayIndex)
        {
            if (!(array is null) && array.Rank != 1)
            {
                throw new ArgumentException();
            }

            try
            {
                Array.Copy(this.ToArray(), 0, array, arrayIndex, Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException();
            }
        }

        /// <inheritdoc/>
        public IEnumerator<TWrapper> GetEnumerator() => new WrappedEnumerator<TWrapper>(Src.GetEnumerator(), Converter);

        /// <inheritdoc/>
        public int IndexOf(TWrapper item) => Src.IndexOf(Converter.ConvertBack(item));

        /// <inheritdoc/>
        public void Insert(int index, TWrapper item) => Src.Insert(index, Converter.ConvertBack(item));

        /// <inheritdoc/>
        public bool Remove(TWrapper item)
        {
            object original = Converter.ConvertBack(item);
            if (Src.Contains(original))
            {
                Src.Remove(original);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void RemoveAt(int index) => Src.RemoveAt(index);

        object IList.this[int index] { get => this[index]; set => this[index] = (TWrapper)value; }
        int IList.Add(object value)
        {
            Add((TWrapper)value);
            return Count - 1;
        }
        bool IList.Contains(object value) => Contains((TWrapper)value);
        void ICollection.CopyTo(Array array, int index) => CopyTo((TWrapper[])array, index);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        int IList.IndexOf(object value) => IndexOf((TWrapper)value);
        void IList.Insert(int index, object value) => Insert(index, (TWrapper)value);
        void IList.Remove(object value) => Remove((TWrapper)value);

        internal sealed class ListConstructorSet
        {
            private readonly FastConstructor Default;

            private readonly FastConstructor WithItems;

            private readonly FastConstructor WithCapacitySpecified;
            private static readonly Type[] WithCapacitySpecifiedParameters = new Type[] { typeof(int) };

            public ListConstructorSet(Type type)
            {
                Default = FastConstructor.Create(type.GetConstructor(Type.EmptyTypes));
                WithItems = FastConstructor.Create(type.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { type.GenericTypeArguments[0] }) }));
                WithCapacitySpecified = FastConstructor.Create(type.GetConstructor(WithCapacitySpecifiedParameters));
            }

            public object Create() => Default.Invoke(null);
            public object Create(IEnumerable collection) => WithItems.Invoke(new object[] { collection });
            public object Create(int capacity) => WithCapacitySpecified.Invoke(new object[] { capacity });
        }
    }
}
