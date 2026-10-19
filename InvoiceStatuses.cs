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
		AcceptedPendingTransfer = 8,
		/// <summary>
		/// A payment request nobody paid within its validity (30 minutes from its creation): it cannot be paid any more.
		/// Terminal; no money was involved. Its issuer gets an InvoiceExpired callback.
		/// </summary>
		Expired = 10
	}

}
