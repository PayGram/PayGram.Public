using CurrenciesLib;
using Newtonsoft.Json;

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

		/// <summary>
		/// Spread applied when user buys the QuoteCurrency (e.g., 0.02 = 2%)
		/// </summary>
		public decimal SpreadBuy { get; set; }
		/// <summary>
		/// Spread applied when user sells the QuoteCurrency (e.g., 0.02 = 2%)
		/// </summary>
		public decimal SpreadSell { get; set; }

		/// <summary>
		/// True when PayGram has no quote of its own for this pair and computed it by chaining fresh quotes along
		/// <see cref="Path"/> (since 3.5): the price is the product of the legs, each spread is compounded as
		/// <c>1 - Π(1 - spread)</c>, <see cref="UpdatedUTC"/> is the oldest leg. It is the rate a Convert of the pair
		/// uses at that moment. Left out of the json when false, so the direct quotes are written as before.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public bool Derived { get; set; }

		/// <summary>
		/// Only for a <see cref="Derived"/> quote: the currencies crossed, from the base currency to
		/// <see cref="QuoteCurrency"/> included (e.g. USD, USDT, PHPT). Null, and left out of the json, for a direct quote.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<Currencies>? Path { get; set; }

		public override string ToString()
		{
			return $"{QuoteCurrency}:{Price:0.####}@{UpdatedUTC:dd/MM H:m:ss}{(Derived ? " derived" : "")}";
		}
	}
}
