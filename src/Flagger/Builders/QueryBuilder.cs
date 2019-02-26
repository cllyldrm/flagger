namespace Flagger.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Interfaces;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.ElasticSearch;
    using Newtonsoft.Json;

    public class QueryBuilder : IQueryBuilder
    {
        private readonly AppSettings _appSettings;
        private readonly List<string> _statusCodes;
        private Dictionary<string, object> _query;

        public QueryBuilder(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _statusCodes = FillStatusCodes();
        }

        public IQueryBuilder New()
        {
            _query = new Dictionary<string, object>();
            return this;
        }

        public IQueryBuilder Size(int? size = null)
        {
            var pageSize = size ?? _appSettings.ElasticQueryDefaultSize;
            _query.Add("size", pageSize);
            return this;
        }

        public IQueryBuilder Search(string endpoint, List<string> statusCodes = null, int lastMinute = 5)
        {
            _query.Add(
                       "query",
                       new
                       {
                           @bool = new
                           {
                               must = new List<object>
                               {
                                   new
                                   {
                                       query_string = new
                                       {
                                           default_field = "Outbound.Url",
                                           query = endpoint
                                       }
                                   }
                               },
                               should = statusCodes.IsAny()
                                            ? (from statusCode in statusCodes
                                               select new
                                               {
                                                   match_phrase = new Inbound
                                                   {
                                                       StatusCode = statusCode
                                                   }
                                               }).ToList()
                                            : (from statusCode in _statusCodes
                                               select new
                                               {
                                                   match_phrase = new Inbound
                                                   {
                                                       StatusCode = statusCode
                                                   }
                                               }).ToList(),
                               minimum_should_match = 1,
                               filter = new
                               {
                                   range = new Range
                                   {
                                       Timestamp = new Time
                                       {
                                           From = $"now-{lastMinute}m",
                                           To = "now"
                                       }
                                   }
                               }
                           }
                       });

            return this;
        }

        public string Build()
        {
            return JsonConvert.SerializeObject(_query, Formatting.None);
        }

        #region private functions

        private static List<string> FillStatusCodes()
        {
            return new List<string>
            {
                "500",
                "502",
                "503"
            };
        }

        #endregion
    }
}