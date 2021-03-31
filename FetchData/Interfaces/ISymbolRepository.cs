using FetchData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FetchData.Interfaces
{
    public interface ISymbolRepository
    {
        Task MergeSymbolsAsync(List<Symbol> symbols);

        Task CommitAsync();

        Task DelistSymbolsAsync();
    }
}
