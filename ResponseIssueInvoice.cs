using PayGram.Public;
namespace PayGram.Public
{
    public class ResponseIssueInvoice : ResponseInvoiceInfo
    {
        /// <summary>
        /// The url that can be followed to pay this invoice
        /// </summary>
        public string PayUrl { get; set; }
        public ResponseIssueInvoice() : base()
        {
            Type = PaygramResponseTypes.ResponseIssueInvoice;
        }
    }
}
