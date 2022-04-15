using CurrenciesLib;
using PayGram.Public;

namespace PayGram.Types
{
    public class ResponseGetExchangeRates : PaygramResponse
    {
        /// <summary>
        /// A dictionary of base-currencies and quoted-currencies
        /// </summary>
        public Dictionary<Currencies, Dictionary<Currencies, ResponseQuote>> Rates { get; set; }

        public ResponseGetExchangeRates()
            : base(PaygramResponseTypes.ResponseGetExchangeRates)
        {
            Rates = new Dictionary<Currencies, Dictionary<Currencies, ResponseQuote>>();
        }

        public void AddQuote(Currencies baseCurrency, Currencies quoteCurrency, DateTime updatedUtc, decimal price)
        {
            var bc = Rates.ContainsKey(baseCurrency) ? Rates[baseCurrency] : null;
            if (bc == null)
            {
                bc = new Dictionary<Currencies, ResponseQuote>();
                Rates.Add(baseCurrency, bc);
            }

            if (bc.ContainsKey(quoteCurrency))
                throw new Exception($"duplicated quote currency for: baseCurrency: {bc}, quoteCurrency: {quoteCurrency}");

            bc.Add(quoteCurrency, new ResponseQuote()
            {
                Price = price,
                UpdatedUTC = updatedUtc,
                QuoteCurrency = quoteCurrency
            });
        }
    }
}
