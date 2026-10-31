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
		/// The friendly code (12 characters) of the withdrawal: the one its callbacks and its statement entry carry. Absent in an
		/// answer stored before 3.9.0 and replayed. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? FriendlyVoucherCode { get; set; }

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

		/// <summary>
		/// A withdraw refused before anything was debited, with its reason: 500 ResponseGenericError, Type Unknown, the English
		/// <paramref name="message"/>, as before the reason existed, plus <see cref="RefusalReason"/> and, for the amount limits,
		/// <see cref="MinAmount"/> and <see cref="MaxAmount"/>. The fields of an accepted withdraw are written as their zero values,
		/// as on every other failed withdraw answer (purely additive)
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
