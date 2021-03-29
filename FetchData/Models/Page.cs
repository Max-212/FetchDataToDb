using System;
using System.Collections.Generic;
using System.Text;

namespace FetchData.Models
{
    public class Page<T>
    {
        public int page { get; set; }

        public int Count { get; set; }

        public int PerPage { get; set; }

        public List<T> Tickers {get; set;}
    }
}
