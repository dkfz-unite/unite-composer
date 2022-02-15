using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine.Filters;

namespace Unite.Composer.Search.Services.Filters.Base
{
    public abstract class FiltersCollection<TIndex>
        where TIndex : class
    {
        protected const string _keywordSuffix = "keyword";
        protected readonly List<IFilter<TIndex>> _filters;


        public FiltersCollection()
        {
            _filters = new List<IFilter<TIndex>>();
        }


        public virtual void AddFilter(IFilter<TIndex> filter)
        {
            _filters.Add(filter);
        }

        public virtual void AddFilters(IEnumerable<IFilter<TIndex>> filters)
        {
            _filters.AddRange(filters);
        }

        public virtual IEnumerable<IFilter<TIndex>> All()
        {
            return _filters;
        }

        public virtual IEnumerable<IFilter<TIndex>> Without(params string[] filterNames)
        {
            return _filters.Where(filter => !filterNames.Contains(filter.Name));
        }
    }
}
