using FetchData.Api;
using FetchData.Interfaces;
using FetchData.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FetchData.Helpers;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;
using System.Threading;
using FetchData.Dataflows;

namespace FetchData.Services
{
    public class SymbolService : ISymbolService
    {
        private ISymbolRepository symbolRepository;

        private IEventRepository eventRepository;

        private PolygonApiClient client;

        private int perPage = 2000;

        public SymbolService(ISymbolRepository symbolRepository, PolygonApiClient client, IEventRepository eventRepository)
        {
            this.symbolRepository = symbolRepository;
            this.eventRepository = eventRepository;
            this.client = client;
        }

        public async Task UpdateSymbolsAsync()
        {
            var oldSymbols = await symbolRepository.GetSymbolsAsync();
            var parallelSymbols = new ParallelSymbolsBlock(10, client, symbolRepository);
            var page = await client.GetSymbols(1, 1);
            await parallelSymbols.UpdateSymbolsAsync(1, (page.Count / perPage) + 1, perPage);
            await DelistSymbolsAsync(oldSymbols, parallelSymbols.Symbols);
        }

        static void qwe(int i)
        {
            Thread.Sleep(10000);
            Console.WriteLine(i);
            
        }

        private async Task DelistSymbolsAsync(List<Symbol> oldSymbols, List<Symbol> newSymbols)
        {
            var symbolsForDelist = oldSymbols.Except<Symbol>(newSymbols, new SymbolTickerComparer()).Where(s => s.Delisted == false).ToList();   
            await symbolRepository.DelistSymbolsAsync(symbolsForDelist);
            await eventRepository.AddEventAsync(symbolsForDelist, "delisted");
        }
    }
}
