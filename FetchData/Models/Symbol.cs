using System;
using System.Collections.Generic;
using System.Text;

namespace FetchData.Models
{
    public class Symbol
    {
        public string Ticker { get; set; }

        public string Name { get; set; }

        public string Market { get; set; }

        public string Locale { get; set; }

        public string Currency { get; set; }

        public bool Active { get; set; }

        public string PrimaryExch { get; set; }

        public DateTime Updated { get; set; }

        public string Url { get; set; }

        public Codes Codes { get; set; }

        public bool Delisted { get; set; }
    }
}
