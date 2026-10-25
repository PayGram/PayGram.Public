namespace PayGram.Public
{
	public enum ResponseCodes
	{
		ResponseOK = 200,
		ResponseGenericError = 500,
		ResponseServerUpdating = 550,
		ResponseUniqueRequestViolation = 551,
		AuthError = 552,
		NotFound = 553,
		NotEnoughFunds = 554,
		/// <summary>The payment request expired (see InvoiceStatuses.Expired): it cannot be paid, ask the merchant for a new one</summary>
		InvoiceExpired = 555,
		/// <summary>The object is not in a state that allows the action (e.g. a withdrawal already being sent): the answer carries the current state</summary>
		Conflict = 556,
		/// <summary>
		/// The account is under review: its outgoing operations (withdrawals, transfers and vouchers, payments, red envelopes) are
		/// suspended. Final: nothing was moved, do not resend (neither with the same key nor a new one); the user contacts support.
		/// Receiving, deposits, swaps between the user's own currencies and the balance keep working
		/// </summary>
		OutgoingSuspended = 557,
	}
}
