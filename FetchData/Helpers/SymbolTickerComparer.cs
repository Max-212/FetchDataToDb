using FetchData.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FetchData.Helpers
{
    public class SymbolTickerComparer : IEqualityComparer<Symbol>
    {
        public bool Equals(Symbol x, Symbol y)
        {
            return x.Ticker == y.Ticker;
        }

        public int GetHashCode([DisallowNull] Symbol obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return 0;
            return obj.Ticker.GetHashCode();
        }
    }
}
