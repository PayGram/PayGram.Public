using System.ComponentModel;
using Newtonsoft.Json;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>WithdrawAsync</c> / <c>WithdrawV2</c>.
	/// Indicates that a cryptocurrency withdrawal has been accepted for processing.
	/// The withdrawal still goes through an approval process before being executed on-chain.
	/// A withdraw PayGram refuses before debiting anything (500) may carry <see cref="RefusalReason"/>.
	/// </summary>
	public class ResponseWithdrawAccepted : PaygramResponse
	{
		/// <summary>
		/// The user's balance in the withdrawal currency after the funds have been reserved for withdrawal.
		/// </summary>
		public decimal NewBalance { get; set; }
		/// <summary>
		/// The fee for this withdrawal, expressed in the withdrawal currency.
		/// Includes PayGram fees and estimated crypto transaction fees.
		/// </summary>
		public decimal ExpectedFee { get; set; }
		/// <summary>
		/// The total amount deducted from the user's account, expressed in the withdrawal currency.
		/// This includes both the amount to be sent and the fees.
		/// </summary>
		public decimal WithdrawnAmount { get; set; }
		/// <summary>
		/// The net amount the user will receive at the destination address (WithdrawnAmount - ExpectedFee).
		/// Excludes any intermediary or beneficiary bank fees which may apply additionally.
		/// </summary>
		public decimal AmountWillReceive => WithdrawnAmount - ExpectedFee;
		/// <summary>
		/// The GUID of the invoice tracking this withdrawal. Can be used with <c>InvoiceInfo</c> to check withdrawal status.
		/// </summary>
		public Guid InvoiceCode { get; set; }

		/// <summary>
		/// Only on a refusal (Success false, 500): why the withdraw was refused before anything was debited, one of
		/// <see cref="WithdrawRefusalReasons"/>. Absent on a success and on every other failure. A string: treat an unknown
		/// value like an absent one (a generic refusal, <see cref="PaygramResponse.Message"/> says why). Added in 3.6.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? RefusalReason { get; set; }
		/// <summary>
		/// With <see cref="WithdrawRefusalReasons.AmountBelowMinimum"/> and <see cref="WithdrawRefusalReasons.AmountAboveMaximum"/>:
		/// the smallest amount that can be withdrawn in this currency and network. Absent otherwise. Added in 3.6.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public decimal? MinAmount { get; set; }
		/// <summary>
		/// With <see cref="WithdrawRefusalReasons.AmountBelowMinimum"/> and <see cref="WithdrawRefusalReasons.AmountAboveMaximum"/>:
		/// the largest amount that can be withdrawn in this currency and network. Absent otherwise. Added in 3.6.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public decimal? MaxAmount { get; set; }

		public ResponseWithdrawAccepted()
									: base(PaygramResponseTypes.ResponseWithdrawAccepted)
		{

		}

		// a refusal is written as it was before RefusalReason (a plain answer: message, type, code, success) plus the reason and the
		// limits: none of the zero fields of an accepted withdraw (Newtonsoft's ShouldSerialize convention). Any other answer unchanged
		[EditorBrowsable(EditorBrowsableState.Never)] public bool ShouldSerializeNewBalance() => RefusalReason == null;
		[EditorBrowsable(EditorBrowsableState.Never)] public bool ShouldSerializeExpectedFee() => RefusalReason == null;
		[EditorBrowsable(EditorBrowsableState.Never)] public bool ShouldSerializeWithdrawnAmount() => RefusalReason == null;
		[EditorBrowsable(EditorBrowsableState.Never)] public bool ShouldSerializeAmountWillReceive() => RefusalReason == null;
		[EditorBrowsable(EditorBrowsableState.Never)] public bool ShouldSerializeInvoiceCode() => RefusalReason == null;

		/// <summary>
		/// A withdraw refused before anything was debited, with its reason: the same answer as before the reason existed
		/// (500 ResponseGenericError, Type Unknown, the English <paramref name="message"/>), plus <see cref="RefusalReason"/> and,
		/// for the amount limits, <see cref="MinAmount"/> and <see cref="MaxAmount"/>
		/// </summary>
		/// <param name="reason">one of <see cref="WithdrawRefusalReasons"/></param>
		public static ResponseWithdrawAccepted Refused(string reason, string message, decimal? minAmount = null, decimal? maxAmount = null) => new()
		{
			Type = PaygramResponseTypes.Unknown,
			ResponseCode = ResponseCodes.ResponseGenericError,
			Message = message,
			RefusalReason = reason,
			MinAmount = minAmount,
			MaxAmount = maxAmount,
		};
	}
}
