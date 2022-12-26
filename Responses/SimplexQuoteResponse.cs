using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayGram.Public
{
    public class SimplexQuoteResponse
    {
        public string user_id { get; set; }
        public string quote_id { get; set; }
        public DigitalMoney digital_money { get; set; }
        public FiatMoney fiat_money { get; set; }
        public DateTime valid_until { get; set; }
        public string error { get; set; }
    }

    public class DigitalMoney
    {
        public string currency { get; set; }
        public decimal amount { get; set; }
    }

    public class FiatMoney
    {
        public string currency { get; set; }
        public decimal base_amount { get; set; }
        public decimal total_amount { get; set; }
    }
}
