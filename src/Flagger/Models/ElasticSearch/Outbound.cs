namespace Flagger.Models.ElasticSearch
{
    using Newtonsoft.Json;

    public class Outbound
    {
        [JsonProperty(PropertyName = "Outbound.Host")]
        public string Host { get; set; }
    }
}