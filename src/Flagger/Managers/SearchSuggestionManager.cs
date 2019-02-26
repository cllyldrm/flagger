namespace Flagger.Managers
{
    using System.Threading.Tasks;
    using Interfaces;
    using Jobs;
    using Quartz;

    public class SearchSuggestionManager : ISearchSuggestionManager
    {
        public SearchSuggestionManager()
        {
            DefaultValue = true;
        }

        public string Endpoint => "api.com/suggestion/suggest";

        public bool DefaultValue { get; set; }

        public string FlagName => "IsSearchSuggestionEnabled";

        public int ErrorHits => 50;

        public async Task StartJob(IScheduler scheduler)
        {
            var job = JobBuilder.Create<SearchSuggestionJob>()
                                          .WithIdentity("SearchSuggestionJob")
                                          .Build();

            var trigger = TriggerBuilder.Create()
                                                  .WithIdentity("SearchSuggestionCron")
                                                  .StartNow()
                                                  .WithCronSchedule("0 0/1 * * * ?")
                                                  .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}