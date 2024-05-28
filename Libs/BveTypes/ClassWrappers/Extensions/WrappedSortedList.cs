using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using UnembeddedResources;

namespace BveTypes.ClassWrappers.Extensions
{
    /// <summary>
    /// 値がオリジナル型の <see cref="SortedList{TKey, TValue}"/> のラッパーを表します。
    /// </summary>
    /// <typeparam name="TKey">キーの型。</typeparam>
    /// <typeparam name="TValueWrapper">値のオリジナル型に対応するラッパー型。</typeparam>
    /// <seealso cref="SortedList{TKey, TValue}"/>
    [AdditionalTypeWrapper(typeof(SortedList<,>))]
    public class WrappedSortedList<TKey, TValueWrapper> : IDictionary<TKey, TValueWrapper>, IDictionary, IReadOnlyDictionary<TKey, TValueWrapper>
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(WrappedSortedList<,>), @"PluginHost\ClassWrappers");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotOmitParameter { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TypeNotSupported { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static WrappedSortedList()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private static BveTypeSet BveTypes = null;

        private readonly dynamic SrcAsDynamic;
        private readonly ITwoWayConverter<object, TValueWrapper> ValueConverter;

        private readonly Type SrcType;

        private readonly FastField VersionField;
        private readonly FastField KeysField;
        private readonly FastField ValuesField;

        private int Version => (int)VersionField.GetValue(Src);
        private TKey[] KeyArray => KeysField.GetValue(Src) as TKey[];
        private Array ValueArray => ValuesField.GetValue(Src) as Array;

        /// <summary>
        /// ラップされているオリジナル オブジェクトを取得します。
        /// </summary>
        public IDictionary Src { get; }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <param name="valueConverter">オリジナル型とラッパー型を相互に変換するための <see cref="ITwoWayConverter{T1, T2}"/>。</param>
        public WrappedSortedList(IDictionary src, ITwoWayConverter<object, TValueWrapper> valueConverter)
        {
            if (BveTypes is null)
            {
                BveTypes = ClassWrapperInitializer.LazyInitialize();
            }

            Src = src;
            SrcAsDynamic = Src;
            ValueConverter = valueConverter;

            SrcType = Src.GetType();

            VersionField = FastField.Create(SrcType.GetField("version", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod));
            KeysField = FastField.Create(SrcType.GetField("keys", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod));
            ValuesField = FastField.Create(SrcType.GetField("values", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod));
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        public WrappedSortedList(IDictionary src) : this(src, new ClassWrapperConverter<TValueWrapper>())
        {
#if DEBUG
            if (!typeof(TValueWrapper).IsSubclassOf(typeof(ClassWrapperBase)))
            {
                throw new ArgumentException(string.Format(Resources.Value.CannotOmitParameter.Value, nameof(TValueWrapper), nameof(ClassWrapperBase)));
            }
#endif
        }


        /// <inheritdoc/>
        public TValueWrapper this[TKey key]
        {
            get => ValueConverter.Convert(Src[key]);
            set => Src[key] = ValueConverter.ConvertBack(value);
        }

        object IDictionary.this[object key]
        {
            get => this[(TKey)key];
            set => this[(TKey)key] = (TValueWrapper)value;
        }

        /// <inheritdoc/>
        public int Capacity
        {
            get => KeyArray.Length;
            set => SrcAsDynamic.Capacity = value;
        }

        /// <inheritdoc/>
        public Comparer<TValueWrapper> Comparer => SrcAsDynamic.Comparer;

        /// <inheritdoc/>
        public int Count => Src.Count;

        /// <inheritdoc/>
        public bool IsFixedSize => Src.IsFixedSize;

        /// <inheritdoc/>
        public bool IsReadOnly => Src.IsReadOnly;

        /// <inheritdoc/>
        public bool IsSynchronized => Src.IsSynchronized;

        /// <inheritdoc/>
        public ICollection<TKey> Keys => Src.Keys as ICollection<TKey>;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValueWrapper>.Keys => Src.Keys as IEnumerable<TKey>;

        ICollection IDictionary.Keys => Src.Keys;

        /// <inheritdoc/>
        public object SyncRoot => Src.SyncRoot;

        private WrappedValueList __Values;
#pragma warning disable IDE1006 // 命名スタイル
        private ICollection<TValueWrapper> _Values => __Values = __Values ?? new WrappedValueList(this);
#pragma warning restore IDE1006 // 命名スタイル

        /// <inheritdoc/>
        public ICollection<TValueWrapper> Values => _Values;

        IEnumerable<TValueWrapper> IReadOnlyDictionary<TKey, TValueWrapper>.Values => _Values;

        ICollection IDictionary.Values => (ICollection)_Values;



        /// <inheritdoc/>
        public void Add(TKey key, TValueWrapper value) => Src.Add(key, ValueConverter.ConvertBack(value));

        void ICollection<KeyValuePair<TKey, TValueWrapper>>.Add(KeyValuePair<TKey, TValueWrapper> item) => Add(item.Key, item.Value);

        void IDictionary.Add(object key, object value) => Add((TKey)key, (TValueWrapper)value);

        /// <inheritdoc/>
        public void Clear() => Src.Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValueWrapper> item)
        {
            int index = IndexOfKey(item.Key);
            return index >= 0 && Count > index && ValueArray.GetValue(index) == ValueConverter.ConvertBack(item.Value);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key) => Src.Contains(key);

        bool IDictionary.Contains(object key) => ContainsKey((TKey)key);

        /// <inheritdoc/>
        public bool ContainsValue(TValueWrapper value)
        {
            int index = IndexOfValue(value);
            return 0 <= index && index < Count;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValueWrapper>[] array, int arrayIndex)
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

        void ICollection.CopyTo(Array array, int index) => CopyTo(array as KeyValuePair<TKey, TValueWrapper>[], index);

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValueWrapper>> GetEnumerator() => new Enumerator(this, EnumeratorType.Enumerator);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this, EnumeratorType.Enumerator);

        IDictionaryEnumerator IDictionary.GetEnumerator() => new Enumerator(this, EnumeratorType.DictionaryEnumerator);

        /// <inheritdoc/>
        public int IndexOfKey(TKey key) => Array.IndexOf(KeyArray, key);

        /// <inheritdoc/>
        public int IndexOfValue(TValueWrapper value) => Array.IndexOf(ValueArray, ValueConverter.ConvertBack(value));

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            try
            {
                Src.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValueWrapper> item)
        {
            if (Contains(item))
            {
                Remove(item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        void IDictionary.Remove(object key) => Remove((TKey)key);

        /// <inheritdoc/>
        public void RemoveAt(int index) => SrcAsDynamic.RemoveAt(index);

        /// <inheritdoc/>
        public void TrimExcess() => SrcAsDynamic.TrimExcess();

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValueWrapper value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        internal sealed class WrappedValueList : ICollection<TValueWrapper>, ICollection
        {
            private readonly WrappedSortedList<TKey, TValueWrapper> SortedList;

            public WrappedValueList(WrappedSortedList<TKey, TValueWrapper> sortedList)
            {
                if (sortedList is null)
                {
                    throw new ArgumentNullException();
                }

                SortedList = sortedList;
            }

            public int Count => SortedList.Count;

            public bool IsReadOnly => true;

            public object SyncRoot => SortedList.SyncRoot;

            public bool IsSynchronized => false;

            public void Add(TValueWrapper item) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public bool Contains(TValueWrapper item) => SortedList.ContainsValue(item);

            public void CopyTo(TValueWrapper[] array, int arrayIndex)
            {
                if (!(array is null) && array.Rank != 1)
                {
                    throw new ArgumentException();
                }

                try
                {
                    Array.Copy(SortedList.Values.ToArray(), 0, array, arrayIndex, SortedList.Count);
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException();
                }
            }

            public void CopyTo(Array array, int index) => CopyTo(array as TValueWrapper[], index);

            public IEnumerator<TValueWrapper> GetEnumerator() => new WrappedValueEnumerator(SortedList);

            public bool Remove(TValueWrapper item) => throw new NotSupportedException();

            IEnumerator IEnumerable.GetEnumerator() => new WrappedValueEnumerator(SortedList);
        }

        internal enum EnumeratorType
        {
            Enumerator,
            DictionaryEnumerator,
        }

        internal sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValueWrapper>>, IDisposable, IEnumerator, IDictionaryEnumerator
        {
            private readonly WrappedSortedList<TKey, TValueWrapper> SortedList;
            private readonly EnumeratorType Type;
            
            private readonly IEnumerator<TKey> KeyEnumerator;
            private readonly IEnumerator<TValueWrapper> ValueEnumerator;

            public Enumerator(WrappedSortedList<TKey, TValueWrapper> sortedList, EnumeratorType type)
            {
                SortedList = sortedList;
                Type = type;

                KeyEnumerator = SortedList.Keys.GetEnumerator();
                ValueEnumerator = SortedList.Values.GetEnumerator();
            }

            public KeyValuePair<TKey, TValueWrapper> Current => new KeyValuePair<TKey, TValueWrapper>(KeyEnumerator.Current, ValueEnumerator.Current);

            public object Key => KeyEnumerator.Current;

            public object Value => ValueEnumerator.Current;

            public DictionaryEntry Entry => new DictionaryEntry(KeyEnumerator.Current, ValueEnumerator.Current);

            object IEnumerator.Current
            {
                get
                {
                    switch (Type)
                    {
                        case EnumeratorType.Enumerator:
                            return Current;
                        case EnumeratorType.DictionaryEnumerator:
                            return Entry;
                        default:
                            throw new NotSupportedException(string.Format(Resources.Value.TypeNotSupported.Value, nameof(Type), Type));
                    }
                }
            }

            public void Dispose()
            {
                KeyEnumerator.Dispose();
                ValueEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                bool keyEnumeratorResult = KeyEnumerator.MoveNext();
                bool valueEnumeratorResult = ValueEnumerator.MoveNext();

                return keyEnumeratorResult != valueEnumeratorResult ? throw new AccessViolationException() : keyEnumeratorResult;
            }

            public void Reset()
            {
                KeyEnumerator.Reset();
                ValueEnumerator.Reset();
            }
        }

        internal sealed class WrappedValueEnumerator : IEnumerator<TValueWrapper>, IDisposable, IEnumerator
        {
            private readonly WrappedSortedList<TKey, TValueWrapper> SortedList;
            private readonly int Version;

            private int Index;

            public WrappedValueEnumerator(WrappedSortedList<TKey, TValueWrapper> sortedList)
            {
                SortedList = sortedList;
                Version = SortedList.Version;
            }

            public TValueWrapper Current { get; private set; }

            object IEnumerator.Current => Index == 0 || Index == SortedList.Count + 1 ? throw new InvalidOperationException() : Current;

            public void Dispose()
            {
                Index = 0;
                Current = default;
            }

            public bool MoveNext()
            {
                if (Version != SortedList.Version)
                {
                    throw new InvalidOperationException();
                }

                if (Index < SortedList.Count)
                {
                    object currentValueSrc = SortedList.ValueArray.GetValue(Index);
                    Current = SortedList.ValueConverter.Convert(currentValueSrc);
                    Index++;
                    return true;
                }

                Index = SortedList.Count + 1;
                Current = default;
                return false;
            }

            public void Reset()
            {
                if (Version != SortedList.Version)
                {
                    throw new InvalidOperationException();
                }

                Index = 0;
                Current = default;
            }
        }
    }
}
