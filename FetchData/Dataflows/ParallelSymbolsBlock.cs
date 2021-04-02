using FetchData.Api;
using FetchData.Interfaces;
using FetchData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FetchData.Dataflows
{
    public class ParallelSymbolsBlock
    {
        private int parralelizmDegree;

        private PolygonApiClient client;

        private ISymbolRepository symbolRepository;

        public List<Symbol> Symbols { get; set; } = new List<Symbol>();

        public ParallelSymbolsBlock(int parralelizmDegree, PolygonApiClient client, ISymbolRepository symbolRepository)
        {
            this.parralelizmDegree = parralelizmDegree;
            this.symbolRepository = symbolRepository;
            this.client = client;
        }

        public async Task UpdateSymbolsAsync(int startPage, int endPage, int perPage)
        {
            var actionBlock = CreateActionBlock(perPage);
            
            for (int i = startPage; i <= endPage; i++)
            {
                actionBlock.Post(i);
            }
            actionBlock.Complete();
            await actionBlock.Completion;
        }

        private ActionBlock<int> CreateActionBlock(int perPage)
        {
            var actionBlock = new ActionBlock<int>(async (page) =>
            {
                
                var pageSymbols = await client.GetSymbols(page, perPage);
                await symbolRepository.MergeSymbolsAsync(pageSymbols.Tickers);
                Symbols.AddRange(pageSymbols.Tickers);
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"{ (int)(((double)Symbols.Count / (double)pageSymbols.Count) * 100)}% done.");
            },
            new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = parralelizmDegree });
            return actionBlock;
        }
    }
}
