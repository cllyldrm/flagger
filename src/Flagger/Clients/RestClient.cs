namespace Flagger.Clients
{
    using System;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Flurl.Http.Configuration;
    using Flurl.Http.Content;
    using Interfaces;
    using Microsoft.Extensions.Options;
    using Models;
    using Polly;
    using Polly.CircuitBreaker;

    public class RestClient : IRestClient
    {
        private readonly IFlurlClient _client;
        private readonly CircuitBreakerPolicy _circuitBreaker;

        public RestClient(IFlurlClientFactory clientFactory, IOptions<AppSettings> appSettings, string url)
        {
            _client = clientFactory.Get(url);

            _circuitBreaker = Policy
                              .Handle<FlurlHttpException>()
                              .Or<FlurlHttpTimeoutException>()
                              .CircuitBreakerAsync(
                                                   exceptionsAllowedBeforeBreaking: appSettings.Value.PollyCircuitBreakExceptionCount,
                                                   durationOfBreak: TimeSpan.FromSeconds(appSettings.Value.PollyCircuitBreakDurationInSeconds));
        }

        public async Task<TModel> GetAsync<TModel>(string path)
            where TModel : new()
        {
            return await _circuitBreaker.ExecuteAsync(() => _client.Request(path).GetAsync().ReceiveJson<TModel>());
        }

        public async Task<TModel> PostAsync<TModel>(string path, string query)
            where TModel : new()
        {
            return await _circuitBreaker.ExecuteAsync(() =>
            {
                var jsonContent = new CapturedJsonContent(query);
                return _client.Request(path)
                              .PostAsync(jsonContent)
                              .ReceiveJson<TModel>();
            });
        }

        public async Task<TModel> PatchAsync<TModel>(string path, string query)
            where TModel : new()
        {
            return await _circuitBreaker.ExecuteAsync(() =>
            {
                var jsonContent = new CapturedJsonContent(query);
                return _client.Request(path)
                              .PatchAsync(jsonContent)
                              .ReceiveJson<TModel>();
            });
        }
    }
}