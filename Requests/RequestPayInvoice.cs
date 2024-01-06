namespace PayGram.Public.Requests
{
	public class RequestPayInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}