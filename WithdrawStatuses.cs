namespace PayGram.Public
{
	public enum WithdrawStatuses
	{
		Requested = 1,
		Completed = 2,
		Cancelled = 3,
		Error = 5,
		// 100 was Accepted, internal to the old approval bot and never sent to clients: removed. Do not reuse the value.
		/// <summary> This the status once the first admin accepted the transfer</summary>
		AcceptedPendingTransfer = 8,
		// 7 was Denied: removed on 2026-10-19 (never used, no invoice ever had it). Do not reuse the value.
	}
}
