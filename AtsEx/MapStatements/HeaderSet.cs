using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    internal sealed class HeaderSet : IHeaderSet, IEnumerable<Header>
    {
        private readonly IReadOnlyDictionary<Identifier, IReadOnlyList<Header>> PublicHeaders;

        public IReadOnlyList<Header> PrivateHeaders { get; }

        public HeaderSet(IReadOnlyDictionary<Identifier, IReadOnlyList<Header>> publicHeaders, IReadOnlyList<Header> privateHeaders)
        {
            PublicHeaders = publicHeaders;
            PrivateHeaders = privateHeaders;
        }

        public IReadOnlyList<IHeader> GetAll(Identifier identifier) => PublicHeaders.TryGetValue(identifier, out IReadOnlyList<Header> result) ? result : new List<Header>();

        public IEnumerator<Header> GetEnumerator() => PublicHeaders.Values.SelectMany(x => x).Concat(PrivateHeaders).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
}
