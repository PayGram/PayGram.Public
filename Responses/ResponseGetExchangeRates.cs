using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>GetExchangeRates</c>.
	/// Contains all available exchange-rate quotes, organized as a nested dictionary:
	/// base currency → quote currency → quote details.
	/// Holds the fresh quotes of the rate cache and, from the PayGram bot (since 3.5), the pairs it can compute by
	/// chaining them, marked <see cref="ResponseQuote.Derived"/>.
	/// </summary>
	public class ResponseGetExchangeRates : PaygramResponse
	{
		/// <summary>
		/// Nested dictionary of exchange rates. The outer key is the base (source) currency,
		/// the inner key is the quote (destination) currency, and the value is the <see cref="ResponseQuote"/>
		/// containing the rate, quote currency, and last-updated timestamp.
		/// </summary>
		public Dictionary<Currencies, Dictionary<Currencies, ResponseQuote>> Rates { get; set; }

		public ResponseGetExchangeRates()
			: base(PaygramResponseTypes.ResponseGetExchangeRates)
		{
			Rates = new Dictionary<Currencies, Dictionary<Currencies, ResponseQuote>>();
		}

		/// <summary>
		/// Adds an exchange-rate quote. Throws if the base→quote pair already exists.
		/// </summary>
		public void AddQuote(Currencies baseCurrency, Currencies quoteCurrency, DateTime updatedUtc, decimal price, decimal spreadBuy, decimal spreadSell)
			=> Add(baseCurrency, quoteCurrency, updatedUtc, price, spreadBuy, spreadSell, null);

		/// <summary>
		/// Adds a quote computed by chaining other quotes (<see cref="ResponseQuote.Derived"/>), along <paramref name="path"/>:
		/// the currencies crossed, from <paramref name="baseCurrency"/> to <paramref name="quoteCurrency"/> included.
		/// Throws if the base→quote pair already exists.
		/// </summary>
		public void AddDerivedQuote(Currencies baseCurrency, Currencies quoteCurrency, DateTime updatedUtc, decimal price, decimal spreadBuy, decimal spreadSell, IEnumerable<Currencies> path)
		{
			ArgumentNullException.ThrowIfNull(path);
			Add(baseCurrency, quoteCurrency, updatedUtc, price, spreadBuy, spreadSell, path.ToList());
		}

		void Add(Currencies baseCurrency, Currencies quoteCurrency, DateTime updatedUtc, decimal price, decimal spreadBuy, decimal spreadSell, List<Currencies>? path)
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
				QuoteCurrency = quoteCurrency,
				SpreadBuy = spreadBuy,
				SpreadSell = spreadSell,
				Derived = path != null,
				Path = path
			});
		}
	}
}
