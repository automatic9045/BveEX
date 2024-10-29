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
    /// オリジナル型の配列のラッパーを表します。
    /// </summary>
    /// <typeparam name="TWrapper">値のオリジナル型に対応するラッパー型。</typeparam>
    /// <seealso cref="Array"/>
    [AdditionalTypeWrapper(typeof(Array))]
    public class WrappedArray<TWrapper> : ClassWrapperBase, IList<TWrapper>, IList
    {
        private static BveTypeSet BveTypes = null;
        private static ArrayConstructorSet ArrayConstructors = null;

        private static void LoadConstructors()
        {
            Type originalType = BveTypes.GetTypeInfoOf<TWrapper>().OriginalType;
            Type arrayType = originalType.MakeArrayType();
            ArrayConstructors = new ArrayConstructorSet(arrayType);
        }


        /// <summary>
        /// ラップされているオリジナル オブジェクトです。
        /// </summary>
        protected readonly new Array Src;

        /// <summary>
        /// オリジナル型とラッパー型を相互に変換するためのコンバータです。
        /// </summary>
        protected readonly ITwoWayConverter<object, TWrapper> Converter;

        /// <summary>
        /// <see cref="WrappedArray{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップされているオリジナル オブジェクト。</param>
        /// <param name="converter">オリジナル型とラッパー型を相互に変換するためのコンバータ。</param>
        /// <exception cref="ArgumentException"><paramref name="src"/> が配列ではありません。</exception>
        protected WrappedArray(Array src, ITwoWayConverter<object, TWrapper> converter) : base(src)
        {
            if (BveTypes is null)
            {
                BveTypes = ClassWrapperInitializer.LazyInitialize();
                LoadConstructors();
            }

            Src = src;
            if (!Src.GetType().IsArray) throw new ArgumentException();

            Converter = converter;
        }

        /// <summary>
        /// 既定のコンバータを使用して、<see cref="WrappedArray{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src"></param>
        protected WrappedArray(Array src) : this(src, new ClassWrapperConverter<TWrapper>())
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <param name="converter">オリジナル型とラッパー型を相互に変換するための <see cref="ITwoWayConverter{T1, T2}"/>。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="WrappedArray{TWrapper}"/> クラスのインスタンス。</returns>
        public static WrappedArray<TWrapper> FromSource(Array src, ITwoWayConverter<object, TWrapper> converter) => src is null ? null : new WrappedArray<TWrapper>(src, converter);

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="WrappedArray{TWrapper}"/> クラスのインスタンス。</returns>
        public static WrappedArray<TWrapper> FromSource(Array src) => FromSource(src, new ClassWrapperConverter<TWrapper>());

        /// <summary>
        /// 指定した長さを備えた空の配列を表す <see cref="WrappedArray{TWrapper}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="capacity">新しい配列に格納できる要素の数。</param>
        public WrappedArray(int capacity) : base(ArrayConstructors.Create(capacity))
        {
        }

        /// <inheritdoc/>
        public TWrapper this[int index]
        {
            get => Converter.Convert(Src.GetValue(index));
            set => Src.SetValue(Converter.ConvertBack(value), index);
        }

        /// <inheritdoc/>
        public bool IsFixedSize => Src.IsFixedSize;
        /// <inheritdoc/>
        public bool IsReadOnly => Src.IsReadOnly;
        /// <inheritdoc/>
        public bool IsSynchronized => Src.IsSynchronized;
        /// <inheritdoc/>
        public int Length => Src.Length;
        /// <inheritdoc/>
        public long LongLength => Src.LongLength;
        /// <inheritdoc/>
        public int Rank => Src.Rank;
        /// <inheritdoc/>
        public object SyncRoot => Src.SyncRoot;

        /// <inheritdoc/>
        public void CopyTo(TWrapper[] array, int index) => (this as ICollection).CopyTo(array, index);
        /// <inheritdoc/>
        public IEnumerator<TWrapper> GetEnumerator() => new WrappedEnumerator<TWrapper>(Src.GetEnumerator(), Converter);

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
        public int GetLength(int dimension) => Src.GetLength(dimension);
        public long GetLongLength(int dimension) => Src.GetLongLength(dimension);
        public int GetLowerBound(int dimension) => Src.GetLowerBound(dimension);
        public int GetUpperBound(int dimension) => Src.GetUpperBound(dimension);
        public TWrapper GetValue(int index) => Converter.Convert(Src.GetValue(index));
        public TWrapper GetValue(int index1, int index2) => Converter.Convert(Src.GetValue(index1, index2));
        public TWrapper GetValue(int index1, int index2, int index3) => Converter.Convert(Src.GetValue(index1, index2, index3));
        public TWrapper GetValue(int[] indices) => Converter.Convert(Src.GetValue(indices));
        public TWrapper GetValue(long index) => Converter.Convert(Src.GetValue(index));
        public TWrapper GetValue(long index1, long index2) => Converter.Convert(Src.GetValue(index1, index2));
        public TWrapper GetValue(long index1, long index2, long index3) => Converter.Convert(Src.GetValue(index1, index2, index3));
        public TWrapper GetValue(params long[] indices) => Converter.Convert(Src.GetValue(indices));
        public void Initialize() => Src.Initialize();
        public void SetValue(TWrapper value, int index) => Src.SetValue(Converter.ConvertBack(value), index);
        public void SetValue(TWrapper value, int index1, int index2) => Src.SetValue(Converter.ConvertBack(value), index1, index2);
        public void SetValue(TWrapper value, int index1, int index2, int index3) => Src.SetValue(Converter.ConvertBack(value), index1, index2, index3);
        public void SetValue(TWrapper value, params int[] indices) => Src.SetValue(Converter.ConvertBack(value), indices);
        public void SetValue(TWrapper value, long index) => Src.SetValue(Converter.ConvertBack(value), index);
        public void SetValue(TWrapper value, long index1, long index2) => Src.SetValue(Converter.ConvertBack(value), index1, index2);
        public void SetValue(TWrapper value, long index1, long index2, long index3) => Src.SetValue(Converter.ConvertBack(value), index1, index2, index3);
        public void SetValue(TWrapper value, params long[] indices) => Src.SetValue(Converter.ConvertBack(value), indices);
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません

        int ICollection<TWrapper>.Count => (Src as ICollection).Count;
        void ICollection<TWrapper>.Add(TWrapper value) => (Src as IList).Add(Converter.ConvertBack(value));
        void ICollection<TWrapper>.Clear() => (Src as IList).Clear();
        bool ICollection<TWrapper>.Contains(TWrapper value) => (Src as IList).Contains(value);
        int IList<TWrapper>.IndexOf(TWrapper value) => (Src as IList).IndexOf(value);
        void IList<TWrapper>.Insert(int index, TWrapper value) => (Src as IList).Insert(index, value);
        TWrapper IList<TWrapper>.this[int index]
        {
            get => Converter.Convert((Src as IList)[index]);
            set => (Src as IList)[index] = Converter.ConvertBack(value);
        }
        bool ICollection<TWrapper>.Remove(TWrapper value)
        {
            (Src as IList).Remove(value);
            return true;
        }
        void IList<TWrapper>.RemoveAt(int index) => (Src as IList).RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => Src.GetEnumerator();

        int ICollection.Count => (Src as ICollection).Count;
        int IList.Add(object value) => (Src as IList).Add(value);
        void IList.Clear() => (Src as IList).Clear();
        bool IList.Contains(object value) => (Src as IList).Contains(value);
        void ICollection.CopyTo(Array array, int index)
        {
            for (int i = 0; i < Src.Length; i++)
            {
                if (array.Length <= index + i) break;

                object value = Src.GetValue(i);
                array.SetValue(value, index + i);
            }
        }
        int IList.IndexOf(object value) => (Src as IList).IndexOf(value);
        void IList.Insert(int index, object value) => (Src as IList).Insert(index, value);
        object IList.this[int index]
        {
            get => Converter.Convert((Src as IList)[index]);
            set => (Src as IList)[index] = Converter.ConvertBack((TWrapper)value);
        }
        void IList.Remove(object value) => (Src as IList).Remove(value);
        void IList.RemoveAt(int index) => (Src as IList).RemoveAt(index);


        internal sealed class ArrayConstructorSet
        {
            private readonly FastConstructor Constructor;
            private static readonly Type[] ConstructorParameters = new Type[] { typeof(int) };

            public ArrayConstructorSet(Type type)
            {
                ConstructorInfo constructor = type.GetConstructor(ConstructorParameters);
                Constructor = FastConstructor.Create(constructor);
            }

            public object Create(int capacity) => Constructor.Invoke(new object[] { capacity });
        }
    }
}
