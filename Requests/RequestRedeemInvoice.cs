namespace PayGram.Public.Requests
{
	public class RequestRedeemInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
	public class RequestRedeemRedEnvelope : RequestRedeemInvoice
	{
		/// <summary>
		/// this flag can only be set if the requesting user is the owner
		/// </summary>
		public bool Terminate { get; set; }
	}
}