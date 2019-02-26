namespace Flagger.Managers.Interfaces
{
    using System.Threading.Tasks;
    using Quartz;

    public interface IManager
    {
        string Endpoint { get; }

        bool DefaultValue { get; set; }

        string FlagName { get; }

        int ErrorHits { get; }

        Task StartJob(IScheduler scheduler);
    }
}