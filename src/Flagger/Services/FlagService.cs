namespace Flagger.Services
{
    using System.Threading.Tasks;
    using Flagger.Builders.Interfaces;
    using Flagger.Clients.Interfaces;
    using Flagger.Managers.Interfaces;
    using Interfaces;
    using Models;
    using Models.ElasticSearch;

    public class FlagService : IFlagService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IAppmanagementClient _appmanagementClient;
        private readonly IQueryBuilder _queryBuilder;

        public FlagService(IElasticClient elasticClient, IQueryBuilder queryBuilder, IAppmanagementClient appmanagementClient)
        {
            _elasticClient = elasticClient;
            _queryBuilder = queryBuilder;
            _appmanagementClient = appmanagementClient;
        }

        public async Task<bool> IsSwitchable(IManager manager)
        {
            var query = _queryBuilder.New().Size().Search(manager.Endpoint).Build();
            var result = await _elasticClient.PostAsync<SearchResult>("_search", query);

            if (result.Hits.Total > manager.ErrorHits)
            {
                return manager.DefaultValue;
            }

            return !manager.DefaultValue;
        }

        public async Task Switch(IManager manager)
        {
            var isSwitched = await _appmanagementClient.PatchAsync<AppmanagementResult>($"featureToggles/key/{manager.FlagName}/switch/{!manager.DefaultValue}", string.Empty);

            if (isSwitched.Success)
            {
                manager.DefaultValue = !manager.DefaultValue;
            }
        }
    }
}