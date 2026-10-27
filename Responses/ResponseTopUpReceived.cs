using CurrenciesLib;
using Newtonsoft.Json;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>RedeemVoucher</c> / <c>RedeemVoucherV2</c> when a voucher or red envelope is successfully redeemed.
	/// Contains the amount credited to the user's balance after fees.
	/// A resend by the user who already redeemed the code stays a refusal (500), never a second success, and carries
	/// <see cref="RefusalReason"/> <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/> with the identifiers and amounts of
	/// that original redemption (3.8.0): see <see cref="RedeemRefusalReasons"/>.
	/// </summary>
	public class ResponseTopUpReceived : PaygramResponse
	{
		/// <summary>
		/// The gross amount received from the invoice, in <see cref="CurrencyCode"/>, before fees.
		/// Set from the invoice's Amount.
		/// </summary>
		public decimal Received { get; set; }
		/// <summary>
		/// The net amount credited to the user's balance after fees (Received - Fees). Computed property.
		/// </summary>
		public decimal Credited => Received - Fees;
		/// <summary>
		/// The fees deducted from this redemption, in <see cref="CurrencyCode"/>.
		/// Set from the invoice's Fees.
		/// </summary>
		public decimal Fees { get; set; }
		/// <summary>
		/// The currency in which the voucher was denominated and the credit was applied.
		/// </summary>
		public Currencies CurrencyCode { get; set; }
		/// <summary>
		/// The user's balance in <see cref="CurrencyCode"/> after the redemption has been applied.
		/// </summary>
		public decimal NewBalance { get; set; }

		/// <summary>
		/// The code (guid) of the voucher or red envelope that was redeemed. On a success and on a refusal
		/// <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/>; absent otherwise. Added in 3.8.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? InvoiceCode { get; set; }
		/// <summary>
		/// The id of the transaction that credited the redeemer: the same <c>TransactionId</c> as the entry of that credit in the
		/// statement. On a success, and on a refusal <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/> the SAME value the
		/// original success carried. A correlation id only, not the key to deduplicate credits: the callbacks of a credit (the
		/// return of an unclaimed voucher, the remainder of a red envelope) do not carry it, so credit a code
		/// (<see cref="InvoiceCode"/>) at most once per user. Absent on every other failure, and (rarely) on a success whose
		/// transaction could not be read back. Added in 3.8.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? TransactionId { get; set; }
		/// <summary>
		/// When the redeemer was credited, UTC: on a success now, on a refusal <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/>
		/// the time of the original redemption. Absent when <see cref="TransactionId"/> is. Added in 3.8.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? RedeemedAtUtc { get; set; }

		/// <summary>
		/// Only on a refusal (Success false): why the redeem was refused, one of <see cref="RedeemRefusalReasons"/>. Absent on a
		/// success and on every other failure. A string: treat an unknown value like an absent one. Added in 3.8.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? RefusalReason { get; set; }
		/// <summary>
		/// With <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/>: the net amount credited by the original redemption (its
		/// <see cref="Credited"/>). <see cref="Received"/>, <see cref="Fees"/> and <see cref="Credited"/> stay 0 on a refusal:
		/// nothing was credited now. Added in 3.8.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public decimal? OriginalCredited { get; set; }
		/// <summary>With <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/>: the fees of the original redemption. Added in 3.8.0</summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public decimal? OriginalFees { get; set; }
		/// <summary>With <see cref="RedeemRefusalReasons.AlreadyRedeemedByYou"/>: the currency of the original redemption. Added in 3.8.0</summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Currencies? OriginalCurrency { get; set; }

		public ResponseTopUpReceived()
							: base(PaygramResponseTypes.ResponseTopUpReceived)
		{

		}
		public ResponseTopUpReceived(ResponseCodes code) : base(PaygramResponseTypes.ResponseTopUpReceived, code)
		{

		}
		public ResponseTopUpReceived(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseTopUpReceived, code)
		{

		}
	}
}
