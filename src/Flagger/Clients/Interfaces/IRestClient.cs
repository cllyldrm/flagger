namespace Flagger.Clients.Interfaces
{
    using System.Threading.Tasks;

    public interface IRestClient
    {
        Task<TModel> GetAsync<TModel>(string path)
            where TModel : new();

        Task<TModel> PostAsync<TModel>(string path, string query)
            where TModel : new();

        Task<TModel> PatchAsync<TModel>(string path, string query)
            where TModel : new();
    }
}