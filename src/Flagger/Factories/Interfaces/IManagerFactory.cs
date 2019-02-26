namespace Flagger.Factories.Interfaces
{
    using System.Collections.Generic;
    using Flagger.Managers.Interfaces;

    public interface IManagerFactory
    {
        IEnumerable<IManager> Create();
    }
}