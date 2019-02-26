namespace Flagger.Factories
{
    using System.Collections.Generic;
    using Flagger.Managers.Interfaces;
    using Interfaces;

    public class ManagerFactory : IManagerFactory
    {
        private readonly ISearchSuggestionManager _searchSuggestionManager;

        public ManagerFactory(ISearchSuggestionManager searchSuggestionManager)
        {
            _searchSuggestionManager = searchSuggestionManager;
        }

        public IEnumerable<IManager> Create()
        {
            return new List<IManager>
            {
                _searchSuggestionManager
            };
        }
    }
}