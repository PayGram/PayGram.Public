using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>QuotePayInvoice</c>: how the payer can pay a payment request in a currency it does not hold (enough of),
	/// with an automatic swap from another currency of its balance. A snapshot: nothing is reserved or locked, the payment is
	/// executed at the price of its moment and refused (<see cref="ResponseCodes.QuoteChanged"/>, nothing moved) when it costs
	/// more than the MaxSourceAmount the payer confirmed.
	/// </summary>
	public class ResponsePayQuote : PaygramResponse
	{
		/// <summary>The payment request quoted</summary>
		public Guid InvoiceCode { get; set; }
		/// <summary>The currency of the payment request, the one the merchant receives</summary>
		public Currencies Currency { get; set; }
		/// <summary>The amount of the payment request, in <see cref="Currency"/></summary>
		public decimal Amount { get; set; }
		/// <summary>The payer's balance in <see cref="Currency"/>: it is used first, all of it</summary>
		public decimal BalanceInCurrency { get; set; }
		/// <summary>
		/// What is missing in <see cref="Currency"/> (Amount - BalanceInCurrency, at least 0): the part a swap converts. Zero: the
		/// payer can pay without a swap
		/// </summary>
		public decimal Shortfall { get; set; }
		public DateTime QuotedAtUtc { get; set; }
		/// <summary>
		/// Until when the quote is indicative (60 seconds, never past the expiry of the payment request). Past it the payment can
		/// still be sent: the server prices it again and refuses it only if it costs more than the confirmed MaxSourceAmount
		/// </summary>
		public DateTime ValidUntilUtc { get; set; }
		/// <summary>
		/// The currencies that cover the payment, the cheapest first (fees and spread included, the priority of the currency
		/// breaks a tie). Empty when no currency of the payer can cover it, or when <see cref="Shortfall"/> is 0
		/// </summary>
		public List<PayQuoteOption> Options { get; set; } = new();

		public ResponsePayQuote() : base(PaygramResponseTypes.ResponsePayQuote)
		{
		}
		public ResponsePayQuote(ResponseCodes code) : base(PaygramResponseTypes.ResponsePayQuote, code)
		{
		}
		public ResponsePayQuote(string message, ResponseCodes code) : base(message, PaygramResponseTypes.ResponsePayQuote, code)
		{
		}
	}

	/// <summary>One currency a payment request can be paid with, through a swap (<see cref="ResponsePayQuote"/>)</summary>
	public class PayQuoteOption
	{
		/// <summary>The currency taken from the payer's balance and swapped</summary>
		public Currencies SourceCurrency { get; set; }
		/// <summary>
		/// The total taken from the payer in <see cref="SourceCurrency"/>, fees included: show it as the price, and send it as
		/// MaxSourceAmount (with PayWithCurrency = SourceCurrency) to pay
		/// </summary>
		public decimal SourceAmount { get; set; }
		/// <summary>The swap fee, in <see cref="SourceCurrency"/>, already inside <see cref="SourceAmount"/></summary>
		public decimal SwapFees { get; set; }
		/// <summary>
		/// What the swap gives in the currency of the payment request (at least the Shortfall): the excess over the Shortfall stays
		/// on the payer's balance as change
		/// </summary>
		public decimal ReceivedAmount { get; set; }
		/// <summary>The rate of the swap, spread included: units of the payment request's currency for 1 <see cref="SourceCurrency"/></summary>
		public decimal Rate { get; set; }
		/// <summary>
		/// What the swap costs against the market rate, fees and spread together, as a fraction (0.013 = 1.3%): the options are
		/// sorted by it
		/// </summary>
		public decimal CostFraction { get; set; }
		/// <summary>The payer's balance in <see cref="SourceCurrency"/> now</summary>
		public decimal SourceBalance { get; set; }
		/// <summary>The position of the option, 1 the one to propose first</summary>
		public int Rank { get; set; }
	}
}
