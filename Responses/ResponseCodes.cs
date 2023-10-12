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
	}
}
