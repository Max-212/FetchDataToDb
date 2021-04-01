using Dapper;
using FetchData.Interfaces;
using FetchData.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FetchData.Repositories
{
    public class EventRepository : IEventRepository
    {
        private ApplicationSettings settings;

        private string sqlInsert = $@"
            Insert into Events(Ticker, Type, Date)
                Values(@Ticker, @Type, @Date)"; 

        public EventRepository(IOptions<ApplicationSettings> settings)
        {
            this.settings = settings.Value;
        }



        public async Task AddEventAsync(List<Symbol> symbols, string type)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                List<object> rows = new List<object>();
                foreach(var symbol in symbols)
                {
                    rows.Add(new
                    {
                        Ticker = symbol.Ticker,
                        Type = type,
                        Date = DateTime.Now
                    });
                }
                await connection.ExecuteAsync(sqlInsert, rows);
            }
        }
    }
}
