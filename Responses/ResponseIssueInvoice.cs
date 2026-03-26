namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>IssueInvoice</c> / <c>IssueInvoiceV2</c>.
	/// Extends <see cref="ResponseInvoiceInfo"/> with the payment URL that can be shared with payers.
	/// On success, contains the newly created invoice's code, amount, currency, and fees.
	/// </summary>
	public class ResponseIssueInvoice : ResponseInvoiceInfo
	{
		/// <summary>
		/// The URL that can be shared with other users so they can pay this invoice.
		/// For Telegram clients, this is a t.me deep link.
		/// </summary>
		public string? PayUrl { get; set; }
		public ResponseIssueInvoice() : base(PaygramResponseTypes.ResponseIssueInvoice, ResponseCodes.ResponseOK)
		{
		}
		public ResponseIssueInvoice(ResponseCodes code) : base(PaygramResponseTypes.ResponseIssueInvoice, code)
		{
		}
		public ResponseIssueInvoice(string err) : base(PaygramResponseTypes.ResponseIssueInvoice, ResponseCodes.ResponseGenericError)
		{
			Message = err;
		}
	}
}
