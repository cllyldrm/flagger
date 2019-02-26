namespace Flagger.Models.ElasticSearch
{
    using Newtonsoft.Json;

    public class Time
    {
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
    }
}
