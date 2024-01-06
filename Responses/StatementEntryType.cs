namespace PayGram.Public.Responses
{
	public enum StatementEntryType
	{
		None = 0,
		Deposit = 1,
		Withdrawal = 2,
		WithdrawalRefund = 3,
		OutgoingTransfer = 4,
		OutgoingDirectTransfer = 5,
		IncomingTransfer = 6,
		IncomingDirectTransfer = 7,
		ChangeSell = 8,
		ChangeBuy = 9,
		Fee = 10,
		Other = 11
	}
}
