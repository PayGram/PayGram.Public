using CurrenciesLib;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>PayVoucherV2</c>.
	/// Pays an existing invoice/voucher. Amount and currency must match the invoice exactly.
	/// </summary>
	public class PayVoucherRequest
	{
		/// <summary>The human-readable voucher code to pay (e.g. "ABC-1234").</summary>
		public string VoucherCode { get; set; } = string.Empty;
		/// <summary>Expected invoice amount. Must match the invoice's amount exactly.</summary>
		public decimal Amt { get; set; }
		/// <summary>Expected invoice currency. Must match the invoice's currency exactly.</summary>
		public Currencies CurSym { get; set; }
	}
}
