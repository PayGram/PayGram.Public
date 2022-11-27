namespace PayGram.Public
{
	public enum PaygramResponseTypes
	{
		Unknown = 0,
		ResponseClientInfo = 3,
		ResponseSetCallbackApi = 4,
		ResponseCreateClient = 5,
		ResponseInvoiceInfo = 6,
		ResponseTopUp = 7,
		ResponseTopUpReceived = 8,
		ResponseTransferCredit = 9,
		ResponseWithdrawUpdated = 10,
		ResponseUserInfo = 11,
		ResponseWithdrawAccepted = 12,
		ResponseInvoiceWithdrawInfo = 13,
		ResponseIssueInvoice = 14,
		ResponseTopUpCryptapi = 15,
		ResponseCirculatingCoins = 16,
		ResponseGetExchangeRates = 17,
		ResponseGetUpdates = 18,
		ResponseConvert = 19,
		ResponseSwap = 20,
		ResponseStatement = 21,
		ResponseCreateRedEnvelope = 22,
        ResponseSimplex = 23
    }
}
