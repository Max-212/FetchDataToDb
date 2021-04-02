using FetchData.Interfaces;
using FetchData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Linq;

namespace FetchData.Repositories
{
    public class SymbolRepository : ISymbolRepository
    {
        private ApplicationSettings settings;

        private string sqlInsert = $@"
            Insert into Symbols (
               Ticker, Name, Market, Locale, Currency, Active, PrimaryExch, Updated, cik, Figiuid, Scfigi, Cfigi, Figi, Url)
            Values (@Ticker, @Name, @Market, @Locale, @Currency, @Active, @PrimaryExch, @Updated, @cik,
               @Figiuid, @Scfigi, @Cfigi, @Figi, @Url)
            On Conflict (Ticker) Do
               Update Set Name = @Name, Market = @Market, Locale = @Locale, Currency = @Currency, Active = @Active,
                   PrimaryExch = @PrimaryExch, Updated = @Updated, cik = @cik, Figiuid = @Figiuid, Scfigi = @Scfigi,
                   Cfigi = @Cfigi, Figi = @Figi, Url = @Url, Delisted = false";

        private string sqlDelist = $@"
            Update Symbols set Delisted = true where Ticker = @Ticker";

        private string sqlSelect = "Select * from Symbols";


        public SymbolRepository(IOptions<ApplicationSettings> settings)
        {
            this.settings = settings.Value;
            
        }


        public async Task MergeSymbolsAsync(List<Symbol> symbols)
        {
            using(var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                List<object> rows = new List<object>();
                foreach (var symbol in symbols)
                {
                    rows.Add(new
                    {
                        Ticker = symbol.Ticker,
                        Name = symbol.Name,
                        Market = symbol.Market,
                        Locale = symbol.Locale,
                        Currency = symbol.Currency,
                        Active = symbol.Active,
                        PrimaryExch = symbol.PrimaryExch,
                        Updated = symbol.Updated,
                        cik = symbol.Codes?.Cik,
                        Figiuid = symbol.Codes?.Figiuid,
                        Scfigi = symbol.Codes?.Scfigi,
                        Cfigi = symbol.Codes?.Cfigi,
                        Figi = symbol.Codes?.Figi,
                        Url = symbol.Url
                    });
                }
                await connection.ExecuteAsync(sqlInsert, rows);
            }
        }

        public async Task DelistSymbolsAsync(List<Symbol> symbols)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                List<object> rows = new List<object>();
                foreach(var symbol in symbols)
                {
                    rows.Add(new { Ticker = symbol.Ticker });
                }
                await connection.ExecuteAsync(sqlDelist, rows);
            }
        }

        public async Task<List<Symbol>> GetSymbolsAsync()
        {
            using(var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                var result = await connection.QueryMultipleAsync(sqlSelect);
                var symbols = await result.ReadAsync<Symbol>();
                return symbols.ToList();
            }
        }
    }
}
