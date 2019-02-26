namespace Flagger.Jobs
{
    using System.Threading.Tasks;
    using Managers.Interfaces;
    using Quartz;
    using Services.Interfaces;

    public class SearchSuggestionJob : IJob
    {
        private readonly ISearchSuggestionManager _searchSuggestionManager;
        private readonly IFlagService _flagService;

        public SearchSuggestionJob(ISearchSuggestionManager searchSuggestionManager, IFlagService flagService)
        {
            _searchSuggestionManager = searchSuggestionManager;
            _flagService = flagService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var isSwitchable = await _flagService.IsSwitchable(_searchSuggestionManager);

            if (isSwitchable)
            {
                await _flagService.Switch(_searchSuggestionManager);
            }
        }
    }
}