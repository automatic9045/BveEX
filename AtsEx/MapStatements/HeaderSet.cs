using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
    internal sealed partial class HeaderSet : IHeaderSet, IEnumerable<Header>
    {
        private readonly IDictionary<Identifier, IReadOnlyList<Header>> Headers;

        public IReadOnlyList<Header> PrivateHeaders { get; }

        public HeaderSet(IDictionary<Identifier, IReadOnlyList<Header>> headers, IReadOnlyList<Header> privateHeaders)
        {
            Headers = headers;
            PrivateHeaders = privateHeaders;
        }

        public IReadOnlyList<IHeader> GetAll(Identifier identifier) => Headers.TryGetValue(identifier, out IReadOnlyList<Header> result) ? result : new List<Header>();

        public IEnumerator<Header> GetEnumerator() => Headers.Values.SelectMany(x => x).Concat(PrivateHeaders).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
