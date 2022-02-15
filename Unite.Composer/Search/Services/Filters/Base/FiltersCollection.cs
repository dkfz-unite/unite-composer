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

        public virtual void Add(BooleanFilter<TIndex> filter)
        {
            if (filter.Value != null)
            {
                _filters.Add(filter);
            }
        }

        public virtual void Add<TProp>(EqualityFilter<TIndex, TProp> filter)
        {
            if (filter.Values?.Any() == true)
            {
                _filters.Add(filter);
            }
        }

        public virtual void Add<TProp>(MultiPropertyRangeFilter<TIndex, TProp> filter)
        {
            if (filter.ValueFrom != null || filter.ValueTo != null)
            {
                _filters.Add(filter);
            }
        }

        public virtual void Add<TProp>(NotNullFilter<TIndex, TProp> filter)
        {
            _filters.Add(filter);
        }

        public virtual void Add<TProp>(RangeFilter<TIndex, TProp> filter)
        {
            if (filter.From != null || filter.To != null)
            {
                _filters.Add(filter);
            }
        }

        public virtual void Add<TProp>(SimilarityFilter<TIndex, TProp> filter)
        {
            if (filter.Values?.Any() == true)
            {
                _filters.Add(filter);
            }
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
