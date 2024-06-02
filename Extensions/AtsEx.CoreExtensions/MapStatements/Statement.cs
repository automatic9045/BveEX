using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost.MapStatements;

namespace AtsEx.Extensions.MapStatements
{
    /// <summary>
    /// AtsEX 由来のステートメント (マップ構文) を表します。
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// このステートメントのソースを取得します。
        /// </summary>
        public MapStatement Source { get; }

        internal Statement(MapStatement source)
        {
            Source = source;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Source.Clauses.Count == 0) return "<Empty>";

            string position = $"{{{Source.Clauses[0].LineIndex},{Source.Clauses[0].CharIndex};{Source.Location}m}}";
            return $"{position} {string.Join(".", Source.Clauses)};";
        }

        internal bool FilterMatches(IEnumerable<ClauseFilter> filters)
        {
            int i = 1;
            foreach (ClauseFilter filter in filters)
            {
                if (Source.Clauses.Count <= i) return false;
                if (!filter.Matches(Source.Clauses[i])) return false;

                i++;
            }

            return true;
        }

        internal bool FilterMatches(params ClauseFilter[] filters) => FilterMatches((IEnumerable<ClauseFilter>)filters);
    }
}
