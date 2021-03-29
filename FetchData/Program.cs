using System;
using System.IO;
using FetchData.Interfaces;
using FetchData.Models;
using FetchData.Repositories;
using FetchData.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FetchData
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISymbolService, SymbolService>()
                .AddSingleton<ISymbolRepository, SymbolRepository>()
                .AddSingleton(config)
                .BuildServiceProvider();

            var symbolService = serviceProvider.GetService<ISymbolService>();

            Console.CursorVisible = false;
            Console.WriteLine("Starting Fetch data from api to database...");
            Page<Symbol> page;
            do
            {
                
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 1);
                page = symbolService.AddSymbolsFromApi().Result;
                Console.WriteLine($"{ (int)(((double)(page.page*page.PerPage)/(double)page.Count)*100)}% done.");

            }
            while (page.page <= page.Count / page.PerPage);
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Finished");
        }
    }
}
