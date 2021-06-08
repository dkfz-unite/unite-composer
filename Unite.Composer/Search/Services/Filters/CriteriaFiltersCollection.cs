using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine.Filters;

namespace Unite.Composer.Search.Services.Filters
{
    public abstract class CriteriaFiltersCollection<TIndex, TCriteria>
        where TIndex : class
        where TCriteria : class
    {
        protected IEnumerable<IFilter<TIndex>> filters;


        public CriteriaFiltersCollection(TCriteria criteria)
        {
            filters = MapCriteria(criteria);
        }


        public virtual IEnumerable<IFilter<TIndex>> All()
        {
            return filters;
        }

        public virtual IEnumerable<IFilter<TIndex>> Without(params string[] filterNames)
        {
            return filters.Where(filter => !filterNames.Contains(filter.Name));
        }


        protected abstract IEnumerable<IFilter<TIndex>> MapCriteria(TCriteria criteria);
    }
}
