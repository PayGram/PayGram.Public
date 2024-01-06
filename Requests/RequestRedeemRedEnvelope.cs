namespace PayGram.Public
{
	public class RequestRedeemRedEnvelope : PaygramRequest
	{
		public Guid InvoiceId { get; set; }

	}
}
