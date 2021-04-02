using FetchData.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FetchData.Api
{
    public class PolygonApiClient
    {
        private HttpClient client;

        private ApplicationSettings settings;

        public PolygonApiClient(IOptions<ApplicationSettings> settings)
        {
            this.settings = settings.Value;
            client = new HttpClient()
            {
                BaseAddress = new Uri(this.settings.ApiUrl)
            };
        }

        public async Task<Page<Symbol>> GetSymbols(int pageNumber, int perPage)
        {
            var page = await client.GetFromJsonAsync<Page<Symbol>>($"tickers?apiKey={settings.ApiKey}&page={pageNumber}&perpage={perPage}");
            return page;
        }
    }
}
