using CurrenciesLib;

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
	}
}
