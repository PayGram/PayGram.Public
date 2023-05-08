namespace PayGram.Public.Requests
{
	public class RequestRedeemInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}