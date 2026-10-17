namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>CheckUnique</c>.
	/// Finds the invoices behind an invoice GUID, a human-readable voucher code or the client's own unique reference
	/// (the <c>unique</c> / callback data passed on TransferCredit, IssueInvoice, Withdraw...).
	/// </summary>
	public class CheckUniqueRequest
	{
		/// <summary>Invoice GUID, voucher code (e.g. "ABC 123 45 67 89") or the unique reference the client passed when creating the operation.</summary>
		public string Query { get; set; } = string.Empty;
	}
}
