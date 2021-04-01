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

namespace FetchData.Services
{
    public class SymbolService : ISymbolService
    {
        private ISymbolRepository symbolRepository;

        private IEventRepository eventRepository;

        private PolygonApiClient client;

        private const int perPage = 1000;

        public SymbolService(ISymbolRepository symbolRepository, PolygonApiClient client, IEventRepository eventRepository)
        {
            this.symbolRepository = symbolRepository;
            this.eventRepository = eventRepository;
            this.client = client;
        }

        public async Task UpdateSymbolsAsync()
        {
            var oldSymbols = await symbolRepository.GetSymbolsAsync();
            var newSymbols = new List<Symbol>();
            var pageNumber = 1;
            Page<Symbol> page;
            do
            {
                Console.SetCursorPosition(0, 1);
                page = await client.GetSymbols(pageNumber++, perPage);
                await symbolRepository.MergeSymbolsAsync(page.Tickers);
                newSymbols.AddRange(page.Tickers);
                Console.WriteLine($"{ (int)(((double)(page.page * page.PerPage) / (double)page.Count) * 100)}% done.");
            }
            while (page.page <= page.Count / page.PerPage);
            await DelistSymbolsAsync(oldSymbols, newSymbols);
        }

        private async Task DelistSymbolsAsync(List<Symbol> oldSymbols, List<Symbol> newSymbols)
        {
            var symbolsForDelist = oldSymbols.Except<Symbol>(newSymbols, new SymbolTickerComparer()).ToList();   
            await symbolRepository.DelistSymbolsAsync(symbolsForDelist);
            await eventRepository.AddEventAsync(symbolsForDelist, "delisted");
        }
    }
}
