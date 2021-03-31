using System;
using System.IO;
using System.Threading.Tasks;
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
        static async Task Main(string[] args)
        {
            var serviceProvider = Startup.ConfigureServices();
            var symbolService = serviceProvider.GetService<ISymbolService>();

            Console.CursorVisible = false;
            Console.WriteLine("Starting Fetch data from api to database...");
            await symbolService.UpdateSymbolsAsync();
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Finished");
        }
    }
}
