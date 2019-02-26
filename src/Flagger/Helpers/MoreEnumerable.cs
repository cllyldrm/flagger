namespace Flagger.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class MoreEnumerable
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
