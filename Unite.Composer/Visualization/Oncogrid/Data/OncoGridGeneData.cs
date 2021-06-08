using System.Collections.Generic;

namespace Unite.Composer.Visualization.Oncogrid.Data
{
    public class OncoGridGeneData
    {
        /// <summary>
        /// Attention: Do not rename this attribute because this name is required within the used javascript framework for oncogrid:
        /// https://github.com/oncojs/oncogrid
        /// </summary>
        public string Id { get; set; }

        public string Symbol { get; set; }

        //TODO Check if DistinctBy could be implemented instead of this Comparer
        private sealed class EnsemblIdEqualityComparer : IEqualityComparer<OncoGridGeneData>
        {
            public bool Equals(OncoGridGeneData x, OncoGridGeneData y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(OncoGridGeneData obj)
            {
                return (obj.Id != null ? obj.Id.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<OncoGridGeneData> EnsemblIdComparer { get; } =
            new EnsemblIdEqualityComparer();
    }
}