using CurrenciesLib;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>TransferCreditV2</c>.
	/// Transfers funds from the caller's account to another user.
	/// </summary>
	public class TransferCreditRequest
	{
		/// <summary>Telegram ID of the recipient. Null to transfer to the user's own linked account.</summary>
		public long? To { get; set; }
		/// <summary>Amount to transfer (must be &gt; 0).</summary>
		public decimal Amt { get; set; }
		/// <summary>Currency of the transfer (e.g. USDT, PHPT, BTC).</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Client-generated idempotency key (max 64 bytes). Prevents duplicate transfers.</summary>
		public string Unique { get; set; } = string.Empty;
		/// <summary>Optional callback data returned in notifications related to this transfer.</summary>
		public string? Cd { get; set; }
	}
}
