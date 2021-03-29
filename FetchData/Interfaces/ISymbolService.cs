using FetchData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FetchData.Interfaces
{
    public interface ISymbolService
    {
        Task<Page<Symbol>> AddSymbolsFromApi();
    }
}
