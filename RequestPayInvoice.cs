namespace PayGram.Public
{
	public class RequestPayInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}