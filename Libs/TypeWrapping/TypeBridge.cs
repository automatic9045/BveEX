using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWrapping
{
    public class TypeBridge : IReadOnlyDictionary<Type, TypeMemberSetBase>
    {
        private readonly IReadOnlyDictionary<Type, TypeMemberSetBase> Source;

        public TypeMemberSetBase this[Type bridgedWrapper] => Source[bridgedWrapper];
        TypeMemberSetBase IReadOnlyDictionary<Type, TypeMemberSetBase>.this[Type key] => this[key];

        public IEnumerable<Type> BridgedWrappers => Source.Keys;
        IEnumerable<Type> IReadOnlyDictionary<Type, TypeMemberSetBase>.Keys => BridgedWrappers;

        public IEnumerable<TypeMemberSetBase> TargetWrappers => Source.Values;
        IEnumerable<TypeMemberSetBase> IReadOnlyDictionary<Type, TypeMemberSetBase>.Values => TargetWrappers;

        public int Count => Source.Count;

        public bool ContainsBridgedWrapper(Type bridgedWrapper) => Source.ContainsKey(bridgedWrapper);
        bool IReadOnlyDictionary<Type, TypeMemberSetBase>.ContainsKey(Type key) => ContainsBridgedWrapper(key);

        public bool TryGetTargetWrapper(Type bridgedWrapper, out TypeMemberSetBase targetWrapper) => Source.TryGetValue(bridgedWrapper, out targetWrapper);
        bool IReadOnlyDictionary<Type, TypeMemberSetBase>.TryGetValue(Type key, out TypeMemberSetBase value) => TryGetTargetWrapper(key, out value);

        public IEnumerator<KeyValuePair<Type, TypeMemberSetBase>> GetEnumerator() => Source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public TypeBridge(IReadOnlyDictionary<Type, TypeMemberSetBase> source)
        {
            Source = source;
        }
    }
}
