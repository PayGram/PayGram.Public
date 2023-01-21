namespace PayGram.Public.Responses
{
	public class ResponseCreateRedEnvelope : PaygramResponse
	{
		public Guid InvoiceCode { get; set; }
		public decimal Fees { get; set; }
		public ResponseCreateRedEnvelope() : base(PaygramResponseTypes.ResponseCreateRedEnvelope)
		{

		}
	}
}
