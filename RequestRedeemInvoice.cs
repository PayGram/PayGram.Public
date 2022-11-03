namespace PayGram.Public
{
	public class RequestRedeemInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}