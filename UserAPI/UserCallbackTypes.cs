namespace PayGram.Public.UserAPI
{
	public enum UserCallbackTypes
	{
		/// <summary>
		/// The type of the callback after a transaction happened
		/// </summary>
		BalanceInfo = 0,
		/// <summary>
		/// The type of the callback after a withdraw transaction happened
		/// </summary>
		WithdrawInfo = 1,
		/// <summary>
		/// The type of the callback after an invoice was either paid to this user
		/// </summary>
		InvoiceInfoCredited = 2,
		InvoiceInfoDebited = 5,
		/// <summary>
		/// The type of the callback to represent a basic response
		/// </summary>
		CallbackInfo = 3,
		/// <summary>
		/// Received when the user sent an amount through the direct transfer
		/// </summary>
		MoneySent = 4
	}
}
