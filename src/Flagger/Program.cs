namespace Flagger
{
    using System;
    using System.Globalization;
    using System.IO;
    using Builders;
    using Builders.Interfaces;
    using Clients;
    using Clients.Interfaces;
    using Factories;
    using Flurl.Http;
    using Flurl.Http.Configuration;
    using Jobs;
    using Managers;
    using Managers.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Newtonsoft.Json;
    using Quartz.Impl;
    using Quartz.Spi;
    using Services;
    using Services.Interfaces;

    public static class Program
    {
        public static IConfigurationRoot Configuration;

        public static void Main()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Culture = CultureInfo.InvariantCulture
            };

            JsonConvert.DefaultSettings = () => jsonSerializerSettings;

            FlurlHttp.Configure(c => { c.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings); });

            Configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            InitializeJobs(serviceProvider);

            Console.ReadLine();
        }

        #region private functions

        private static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IConfiguration>(Configuration);

            var appSettings = new AppSettings();

            Configuration.GetSection("AppSettings").Bind(appSettings);

            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddSingleton<IElasticClient, ElasticClient>();
            services.AddSingleton<IAppmanagementClient, AppmanagementClient>();
            services.AddSingleton<ISearchSuggestionManager, SearchSuggestionManager>();
            services.AddTransient<IQueryBuilder, QueryBuilder>();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<IFlagService, FlagService>();
            services.AddScoped<SearchSuggestionJob>();
        }

        private static async void InitializeJobs(IServiceProvider serviceProvider)
        {
            var scheduler = await new StdSchedulerFactory().GetScheduler();
            var jobFactory = new JobFactory(serviceProvider);
            scheduler.JobFactory = jobFactory;

            await scheduler.Start();

            var managerFactory = new ManagerFactory(serviceProvider.GetService<ISearchSuggestionManager>());
            var managers = managerFactory.Create();

            foreach (var manager in managers)
            {
                await manager.StartJob(scheduler);
            }
        }

        #endregion
    }
}