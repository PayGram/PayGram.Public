using Newtonsoft.Json;
using CurrenciesLib;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>TransferCreditV2</c>.
	/// Transfers funds from the caller's account to another user.
	/// </summary>
	public class TransferCreditRequest
	{
		/// <summary>Telegram ID of the recipient. Null to create a voucher instead: the response carries its RedeemUrl.</summary>
		public long? To { get; set; }
		/// <summary>Amount to transfer (must be &gt; 0).</summary>
		public decimal Amt { get; set; }
		/// <summary>Currency of the transfer (e.g. USDT, PHPT, BTC).</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Client-generated idempotency key (max 64 bytes). Prevents duplicate transfers.</summary>
		public string Unique { get; set; } = string.Empty;
		/// <summary>Optional callback data returned in notifications related to this transfer.</summary>
		public string? Cd { get; set; }
		/// <summary>
		/// Optional: your own id of the end user on whose behalf you send this transfer (the payer on your side), at most
		/// <see cref="Constraints.PAYER_REF_MAX"/> characters. An opaque reference, never a name, an e-mail or a phone number: PayGram
		/// stores it with the transfer, for its compliance only: it is write-only, never returned in any response or callback (to you
		/// included), seen only by PayGram's admins, and used to detect structuring (many of your payers sending to the same PayGram user). It is never an identity nor an authorization.
		/// Leave it null if you have none.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // absent when null: the wire format of a call without it is unchanged
		public string? PayerRef { get; set; }
	}
}
