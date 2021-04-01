using FetchData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FetchData.Interfaces
{
    public interface IEventRepository
    {
        Task AddEventAsync(List<Symbol> symbols, string type);
        
    }
}
