using FetchData.Api;
using FetchData.Interfaces;
using FetchData.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FetchData.Services
{
    public class SymbolService : ISymbolService
    {
        private ISymbolRepository symbolRepository;

        private PolygonApiClient client;

        private const int perPage = 1000;

        public SymbolService(ISymbolRepository symbolRepository, PolygonApiClient client)
        {
            this.symbolRepository = symbolRepository;
            this.client = client;
        }

        public async Task AddSymbolsFromApi()
        {
            var pageNumber = 1;
            Page<Symbol> page;
            do
            {
                Console.SetCursorPosition(0, 1);
                page = await client.GetSymbols(pageNumber++, perPage);
                await symbolRepository.AddSymbols(page.Tickers);
                Console.WriteLine($"{ (int)(((double)(page.page * page.PerPage) / (double)page.Count) * 100)}% done.");
            }
            while (page.page <= page.Count / page.PerPage);
           
        }
    }
}
