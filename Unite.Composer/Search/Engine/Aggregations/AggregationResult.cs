namespace Unite.Composer.Search.Engine.Aggregations
{
    public class AggregationResult
    {
        public string Key { get; }
        public long Value { get; }


        public AggregationResult(string key, long value)
        {
            Key = key;
            Value = value;
        }
    }
}
