using CurrenciesLib;

namespace PayGram.Public.Responses
{
    public class ResponseQuote
    {
        public Currencies QuoteCurrency { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{QuoteCurrency}:{Price}@{UpdatedUTC:dd/MM H:m:ss}";
        }
    }
}
