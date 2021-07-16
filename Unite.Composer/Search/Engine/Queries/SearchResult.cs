using System.Collections.Generic;
using System.Linq;

namespace Unite.Composer.Search.Engine.Queries
{
    public class SearchResult<TIndex> where TIndex : class
    {
        public long Total { get; set; }
        public IEnumerable<TIndex> Rows { get; set; }

        public SearchResult()
        {
            Total = 0;
            Rows = Enumerable.Empty<TIndex>();
        }
    }
}
