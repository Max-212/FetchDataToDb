using FetchData.Api;
using FetchData.Interfaces;
using FetchData.Repositories;
using FetchData.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FetchData
{
    public class Startup
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            var configuration = BuildConfiguration();
            services.Configure<ApplicationSettings>(configuration.Bind);
            services.AddSingleton(configuration);
            services.AddSingleton<ISymbolService, SymbolService>();
            services.AddSingleton<ISymbolRepository, SymbolRepository>();
            services.AddSingleton<PolygonApiClient>();
            services.AddSingleton<IEventRepository, EventRepository>();
            return services.BuildServiceProvider();
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
                
            return builder.Build();
        }

    }
}
