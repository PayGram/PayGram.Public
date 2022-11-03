namespace PayGram.Public
{
	public class RequestInvoiceInfo : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}
