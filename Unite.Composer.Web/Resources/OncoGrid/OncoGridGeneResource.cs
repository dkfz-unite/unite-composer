using System.Collections.Generic;
using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Web.Resources.OncoGrid
{
    public class OncoGridGeneResource
    {
        /// <summary>
        /// Attention: Do not rename this attribute because this name is required within the used javascript framework for oncogrid:
        /// https://github.com/oncojs/oncogrid
        /// </summary>
        public string Id { get; set; }

        public string Symbol { get; set; }

        public OncoGridGeneResource(GeneIndex index)
        {
            Id = index.EnsemblId;
            Symbol = index.Symbol;
        }

        //TODO Check if DistinctBy could be implemented instead of this Comparer
        private sealed class EnsemblIdEqualityComparer : IEqualityComparer<OncoGridGeneResource>
        {
            public bool Equals(OncoGridGeneResource x, OncoGridGeneResource y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(OncoGridGeneResource obj)
            {
                return (obj.Id != null ? obj.Id.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<OncoGridGeneResource> EnsemblIdComparer { get; } =
            new EnsemblIdEqualityComparer();
    }
}