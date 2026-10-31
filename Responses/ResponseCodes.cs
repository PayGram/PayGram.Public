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
		/// The suspension lasts until a PayGram admin releases the account. Receiving, deposits, swaps between the user's own
		/// currencies and the balance keep working
		/// </summary>
		OutgoingSuspended = 557,
		/// <summary>
		/// The price moved against the caller between the quote and the execution: a payment with an automatic swap would cost
		/// more than its MaxSourceAmount, or a swap would receive less than its MinReceivedAmount. Nothing was moved. Ask for a new
		/// quote and, if the user confirms the new amount, send it with a NEW unique key (the parameters changed)
		/// </summary>
		QuoteChanged = 558,
		/// <summary>
		/// The automatic swap from the chosen currency cannot be done now, for a reason that is not the payer's: the pair was
		/// disabled, the rate is missing, the amount is outside the limits of a swap, or the platform lacks liquidity in the
		/// requested currency. Nothing was moved. Propose the next option of the quote (QuotePayInvoice) or try again later
		/// </summary>
		SwapUnavailable = 559,
		/// <summary>
		/// The unique key (ClientUnique) is already registered for a different request: the same key was sent with other
		/// parameters, or for another operation. Final for this call: nothing was executed by it, and the request first
		/// registered with that key is untouched (it may have gone through). Before sending a new operation with a NEW key,
		/// check the original one with CheckUnique: never resend this call with the same key (it gets this answer again), and
		/// never send the same operation again with a new key without knowing whether the first ran.
		/// Until the release of 3.7 the server answered 551 (ResponseUniqueRequestViolation) with "already registered for a different request" in
		/// its message
		/// </summary>
		UniqueKeyReusedForOtherRequest = 560,
	}
}
