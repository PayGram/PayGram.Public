namespace PayGram.Public.Responses
{
	public class ResponseIssueInvoice : ResponseInvoiceInfo
	{
		/// <summary>
		/// The url that can be followed to pay this invoice
		/// </summary>
		public string? PayUrl { get; set; }
		public ResponseIssueInvoice(ResponseCodes code) : base(PaygramResponseTypes.ResponseIssueInvoice, code)
		{
		}
		public ResponseIssueInvoice(string err) : base(PaygramResponseTypes.ResponseIssueInvoice, ResponseCodes.ResponseGenericError)
		{
			Message = err;
		}
	}
}
