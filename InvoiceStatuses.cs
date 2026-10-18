namespace PayGram.Public
{
	public enum InvoiceStatuses
	{
		Requested = 1,
		Completed = 2,
		Cancelled = 3,
		OnGoing = 4,
		Error = 5,
		// 7 was Denied: removed on 2026-10-19 (never used, no invoice ever had it). Do not reuse the value.
		AcceptedPendingTransfer = 8
	}

}
