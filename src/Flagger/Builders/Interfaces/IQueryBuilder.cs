namespace Flagger.Builders.Interfaces
{
    using System.Collections.Generic;

    public interface IQueryBuilder
    {
        IQueryBuilder New();

        IQueryBuilder Size(int? size = null);

        IQueryBuilder Search(string endpoint, List<string> statusCodes = null, int lastMinute = 5);

        string Build();
    }
}