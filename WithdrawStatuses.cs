namespace PayGram.Public
{
	public enum WithdrawStatuses
	{
		Requested = 1,
		Completed = 2,
		Cancelled = 3,
		Error = 5,
		/// <summary> This is the status immediately after an admin accepts the transfer. it is never returned to the end user </summary>
		Accepted = 100,
		/// <summary> This the status once the first admin accepted the transfer</summary>
		AcceptedPendingTransfer = 8,
		Denied = 7,
	}
}
