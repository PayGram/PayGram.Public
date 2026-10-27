namespace PayGram.Public.Responses
{
	/// <summary>
	/// The values of <see cref="ResponseWithdrawAccepted.RefusalReason"/>: why PayGram refused a withdraw before anything was
	/// debited (TelegramBotServer#50). A string, not an enum, so that a value added later never breaks the reading of an answer:
	/// treat an unknown value (and an absent one) as a generic refusal and show <see cref="PaygramResponse.Message"/>.
	/// The answer keeps its code (500 ResponseGenericError) and its English Message: the reason only adds a machine-readable
	/// tag to it. Nothing was debited; the same request does not succeed until what the reason says is fixed (a new amount or
	/// destination is a new request, with a new unique key)
	/// </summary>
	public static class WithdrawRefusalReasons
	{
		/// <summary>The amount is below the minimum of the currency and network: <see cref="ResponseWithdrawAccepted.MinAmount"/> and <see cref="ResponseWithdrawAccepted.MaxAmount"/> carry the limits</summary>
		public const string AmountBelowMinimum = nameof(AmountBelowMinimum);
		/// <summary>The amount is above the maximum of the currency and network: <see cref="ResponseWithdrawAccepted.MinAmount"/> and <see cref="ResponseWithdrawAccepted.MaxAmount"/> carry the limits</summary>
		public const string AmountAboveMaximum = nameof(AmountAboveMaximum);
		/// <summary>The user has an address whitelist and the destination (address, currency and network) is not in it</summary>
		public const string AddressNotWhitelisted = nameof(AddressNotWhitelisted);
		/// <summary>The destination is the user's own PayGram deposit address: the money is already in the user's balance</summary>
		public const string OwnDepositAddress = nameof(OwnDepositAddress);
		/// <summary>PayGram cannot send this currency on this network (only USDT and PHPT on TRC20 or BEP20 are sent)</summary>
		public const string NetworkNotSendable = nameof(NetworkNotSendable);
		/// <summary>The request names no currency PayGram knows (a fault in the request, not an outage)</summary>
		public const string CurrencyNotAccepted = nameof(CurrencyNotAccepted);
		/// <summary>No withdraw provider handles this currency and network now: try again later</summary>
		public const string NoProvider = nameof(NoProvider);
	}
}
