namespace Flagger.Models.ElasticSearch
{
    using Newtonsoft.Json;

    public class Range
    {
        [JsonProperty(PropertyName = "@timestamp")]
        public Time Timestamp { get; set; }
    }
}