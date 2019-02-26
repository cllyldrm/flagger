namespace Flagger.Services.Interfaces
{
    using System.Threading.Tasks;
    using Flagger.Managers.Interfaces;

    public interface IFlagService
    {
        Task<bool> IsSwitchable(IManager manager);

        Task Switch(IManager manager);
    }
}