namespace PayGram.Public.Responses
{
    public class ResponseIssueInvoice : ResponseInvoiceInfo
    {
        /// <summary>
        /// The url that can be followed to pay this invoice
        /// </summary>
        public string PayUrl { get; set; }
        public ResponseIssueInvoice(ResponseCodes code) : base(PaygramResponseTypes.ResponseIssueInvoice, code)
        {
        }
    }
}
