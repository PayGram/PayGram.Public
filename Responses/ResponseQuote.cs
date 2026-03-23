using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// A single exchange-rate quote within <see cref="ResponseGetExchangeRates"/>.
	/// Represents the price of one unit of the base currency expressed in the quote currency.
	/// </summary>
	public class ResponseQuote
	{
		/// <summary>
		/// The destination (quote) currency for this rate.
		/// </summary>
		public Currencies QuoteCurrency { get; set; }
		/// <summary>
		/// The UTC timestamp when this rate was last updated from the rate provider.
		/// </summary>
		public DateTime UpdatedUTC { get; set; }
		/// <summary>
		/// The exchange rate: how many units of <see cref="QuoteCurrency"/> equal one unit of the base currency.
		/// </summary>
		public decimal Price { get; set; }

		public override string ToString()
		{
			return $"{QuoteCurrency}:{Price}@{UpdatedUTC:dd/MM H:m:ss}";
		}
	}
}
