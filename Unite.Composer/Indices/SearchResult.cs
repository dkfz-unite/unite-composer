using System.Collections.Generic;
using System.Linq;

namespace Unite.Composer.Indices
{
    public class SearchResult<T>
    {
        public long Total { get; set; }
        public IEnumerable<T> Rows { get; set; }

        public SearchResult()
        {
            Total = 0;
            Rows = Enumerable.Empty<T>();
        }
    }
}
