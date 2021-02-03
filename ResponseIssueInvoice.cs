using PayGram.Public;
namespace PayGram.Types
{
	public class ResponseIssueInvoice : ResponseInvoiceInfo
	{
		public ResponseIssueInvoice() : base()
		{
			Type = PaygramResponseTypes.ResponseIssueInvoice;
		}
	}
}
