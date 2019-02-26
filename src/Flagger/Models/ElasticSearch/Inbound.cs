namespace Flagger.Models.ElasticSearch
{
    using Newtonsoft.Json;

    public class Inbound
    {
        [JsonProperty(PropertyName = "Inbound.StatusCode")]
        public string StatusCode { get; set; }
    }
}