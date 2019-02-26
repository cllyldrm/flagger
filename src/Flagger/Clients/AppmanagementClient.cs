namespace Flagger.Clients
{
    using Flurl.Http.Configuration;
    using Interfaces;
    using Microsoft.Extensions.Options;
    using Models;

    public class AppmanagementClient : RestClient, IAppmanagementClient
    {
        public AppmanagementClient(IFlurlClientFactory clientFactory, IOptions<AppSettings> appSettings)
            : base(clientFactory, appSettings, appSettings.Value.AppmanagementUrl)
        {
        }
    }
}