using FetchData.Interfaces;
using FetchData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FetchData.Repositories
{
    public class SymbolRepository : ISymbolRepository
    {
        private IConfiguration config;

        private string sqlInsert = "" +
            "Insert into Symbols (" +
            "   Ticker, Name, Market, Locale, Currency, Active, PrimaryExch, Updated, cik, Figiuid, Scfigi, Cfigi, Figi, Url)" +
            "Values (@Ticker, @Name, @Market, @Locale, @Currency, @Active, @PrimaryExch, @Updated, @cik," +
            "   @Figiuid, @Scfigi, @Cfigi, @Figi, @Url)" +
            "On Conflict (Ticker) Do" +
            "   Update Set Name = @Name, Market = @Market, Locale = @Locale, Currency = @Currency, Active = @Active," +
            "       PrimaryExch = @PrimaryExch, Updated = @Updated, cik = @cik, Figiuid = @Figiuid, Scfigi = @Scfigi," +
            "       Cfigi = @Cfigi, Figi = @Figi, Url = @Url";

        public SymbolRepository(IConfiguration config)
        {
            this.config = config;
        }


        public async Task AddSymbols(List<Symbol> symbols)
        {
            using (var connection = new NpgsqlConnection(config.GetSection("connectionString").Get<string>())) 
            {
                foreach(var symbol in symbols)
                {
                    await connection.ExecuteAsync(sqlInsert, new
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
                    }) ;
                }
            }
        }
    }
}
