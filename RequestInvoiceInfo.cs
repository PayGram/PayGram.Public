namespace PayGram.Types
{
	public class RequestInvoiceInfo : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}
