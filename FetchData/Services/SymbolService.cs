using FetchData.Interfaces;
using FetchData.Models;
using Microsoft.Extensions.Configuration;
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

        private int page = 1;

        IConfiguration config;

        private HttpClient client;


        public SymbolService(ISymbolRepository symbolRepository, IConfiguration config)
        {
            this.symbolRepository = symbolRepository;
            this.config = config;
            client = new HttpClient()
            {
                BaseAddress = new Uri(config.GetSection("apiUrl").Get<string>())
            };
        }

        public async Task<Page<Symbol>> AddSymbolsFromApi()
        {
            Page<Symbol> symbols = await client.GetFromJsonAsync<Page<Symbol>>($"tickers?apiKey={config.GetSection("apiKey").Get<string>()}&page={page++}&perpage=1000");
            await symbolRepository.AddSymbols(symbols.Tickers);
            return symbols;
        }
    }
}
