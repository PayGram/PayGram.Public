namespace PayGram.Public.Responses
{
	/// <summary>
	/// The values of <see cref="ResponseTopUpReceived.RefusalReason"/> (RedeemVoucher / RedeemVoucherV2, vouchers and red
	/// envelopes): why a redeem was refused. A string, not an enum, so that a value added later never breaks the reading of an
	/// answer: treat an unknown value (and an absent one) as a generic refusal and show <see cref="PaygramResponse.Message"/>.
	/// The answer keeps its code (500 ResponseGenericError) and its English Message: the reason only adds a machine-readable tag.
	/// </summary>
	public static class RedeemRefusalReasons
	{
		/// <summary>
		/// The code was already redeemed by this same user (a resend after a lost answer or a 551; for a red envelope, the user
		/// already took its share; for the sender of a voucher or the creator of an envelope, it already took it back). Nothing
		/// was credited now. It stays a refusal, never a second success, and it describes the original redemption:
		/// <see cref="ResponseTopUpReceived.InvoiceCode"/>, <see cref="ResponseTopUpReceived.TransactionId"/> (the same value the
		/// original success carried), <see cref="ResponseTopUpReceived.RedeemedAtUtc"/>, <see cref="ResponseTopUpReceived.OriginalCredited"/>,
		/// <see cref="ResponseTopUpReceived.OriginalFees"/> and <see cref="ResponseTopUpReceived.OriginalCurrency"/>. Deduplicate by
		/// code, never by TransactionId: credit a voucher or red envelope code at most once per user, whatever told you (a
		/// success, or a callback: the automatic return of an unclaimed voucher, the remainder of a red envelope given back to
		/// its creator, which carry the code and no TransactionId). If you have not credited this InvoiceCode for this user yet
		/// (its success was lost), credit OriginalCredited once; if you have, do not credit it again. TransactionId is only a
		/// correlation id. Only the redeemer gets it: anyone else gets the plain "already redeemed" refusal, without a reason
		/// or any detail
		/// </summary>
		public const string AlreadyRedeemedByYou = nameof(AlreadyRedeemedByYou);
	}
}
