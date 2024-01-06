namespace PayGram.Public
{
	public class ResponseSimplex : PaygramResponse
	{
		public string PaymentUrl { get; set; }
		public string PaymentID { get; set; }
		public SimplexQuoteResponse SimplexQuoteResponse { get; set; }
		public SimplexPaymentResponse SimplexPaymentResponse { get; set; }
		public ResponseSimplex() : base(PaygramResponseTypes.ResponseSimplex)
		{

		}
	}
}
