using CurrenciesLib;
using Newtonsoft.Json;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>IssueInvoiceV2</c>.
	/// Creates a new payment invoice that can be shared with and paid by other users.
	/// </summary>
	public class IssueInvoiceRequest
	{
		/// <summary>Invoice amount (must be &gt; 0).</summary>
		public decimal Amt { get; set; }
		/// <summary>Currency of the invoice (e.g. USDT, PHPT, BTC).</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Optional callback data returned in notifications when the invoice is paid.</summary>
		public string? Cd { get; set; }
		/// <summary>
		/// Optional: your own id of the end user this invoice is for (the customer on your side whose order or account the payment
		/// goes to), at most <see cref="Constraints.PAYER_REF_MAX"/> characters. An opaque reference, never a name, an e-mail or a
		/// phone number. As for <see cref="TransferCreditRequest.PayerRef"/>, PayGram stores it with the invoice for its compliance
		/// only: it is write-only, never returned in any response or callback (to you included), seen only by PayGram's admins. It
		/// is never an identity nor an authorization. Leave it null if you have none.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // absent when null: the wire format of a call without it is unchanged
		public string? PayerRef { get; set; }
	}
}
