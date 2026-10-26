using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// The swap that funded a payment (PayInvoice with PayWithCurrency): part of the answer of the payment and of the payer's
	/// callback. The merchant never sees it: it receives the currency it asked for, as with any payment.
	/// </summary>
	public class PaySwapInfo
	{
		/// <summary>The currency taken from the payer and swapped</summary>
		public Currencies SourceCurrency { get; set; }
		/// <summary>The total taken in <see cref="SourceCurrency"/>, fees included</summary>
		public decimal SourceAmount { get; set; }
		/// <summary>The swap fee in <see cref="SourceCurrency"/>, inside <see cref="SourceAmount"/></summary>
		public decimal Fees { get; set; }
		/// <summary>What the swap gave in the currency of the invoice</summary>
		public decimal ReceivedAmount { get; set; }
		/// <summary>The payer's balance in <see cref="SourceCurrency"/> after the swap</summary>
		public decimal BalanceSourceAfter { get; set; }
		/// <summary>What was left over in the currency of the invoice after the payment (the swap rounds up): on the payer's balance</summary>
		public decimal Change { get; set; }
	}
}
