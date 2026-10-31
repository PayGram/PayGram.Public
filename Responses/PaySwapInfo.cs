using CurrenciesLib;
using Newtonsoft.Json;

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
		/// <summary>
		/// The payer's balance in the currency of the invoice right after the payment, read from the payment itself (in the same
		/// transaction as the swap): what the swap left over (it rounds up, and a minimum may make it buy more). The same meaning
		/// in the answer, in the replay or settle of a lost answer, and in the payer's callback
		/// </summary>
		public decimal Change { get; set; }
		/// <summary>
		/// The code (guid) of the swap itself: the invoice its two statement entries (the sale of <see cref="SourceCurrency"/> and
		/// the purchase) show, not the paid invoice (that one is the InvoiceCode of the answer or callback). Absent when it could
		/// not be read. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? InvoiceCode { get; set; }
		/// <summary>
		/// The friendly code (12 characters) of the swap itself, with <see cref="InvoiceCode"/> (not the paid invoice's). Absent
		/// when <see cref="InvoiceCode"/> is. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? FriendlyVoucherCode { get; set; }
	}
}
