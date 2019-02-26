namespace Flagger.Clients
{
    using Flurl.Http.Configuration;
    using Interfaces;
    using Microsoft.Extensions.Options;
    using Models;

    public class ElasticClient : RestClient, IElasticClient
    {
        public ElasticClient(IFlurlClientFactory clientFactory, IOptions<AppSettings> appSettings)
            : base(clientFactory, appSettings, appSettings.Value.ElasticSearchUrl)
        {
        }
    }
}