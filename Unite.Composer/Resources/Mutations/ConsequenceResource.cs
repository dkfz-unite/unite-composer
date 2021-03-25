using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Resources.Mutations
{
    public class ConsequenceResource
    {
        public string Type { get; set; }
        public string Impact { get; set; }
        public int Severity { get; set; }

        public ConsequenceResource(ConsequenceIndex index)
        {
            Type = index.Type;
            Impact = index.Impact;
            Severity = index.Severity;
        }
    }
}
