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
	}
}
