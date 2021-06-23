using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine.Filters;

namespace Unite.Composer.Search.Services.Filters
{
    public abstract class CriteriaFiltersCollection<TIndex>
        where TIndex : class
    {
        protected ICollection<IFilter<TIndex>> _filters;


        public CriteriaFiltersCollection()
        {
            _filters = new List<IFilter<TIndex>>();
        }


        public virtual void AddFilter(IFilter<TIndex> filter)
        {
            _filters.Add(filter);
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
