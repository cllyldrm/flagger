namespace Flagger.Models
{
    public class AppSettings
    {
        public string ElasticSearchUrl { get; set; }

        public string AppmanagementUrl { get; set; }

        public int ElasticQueryDefaultSize { get; set; }

        public int PollyCircuitBreakExceptionCount { get; set; }

        public int PollyCircuitBreakDurationInSeconds { get; set; }
    }
}