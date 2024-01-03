namespace PayGram.Public.Responses
{
	public class ResponseCreateRedEnvelope : PaygramResponse
	{
		public Guid InvoiceCode { get; set; }
        public string? FriendlyVoucherCode { get; set; }
        public decimal Fees { get; set; }
		public string RedeemUrl { get; set; }
		public decimal Amount { get; set; }

		public ResponseCreateRedEnvelope() : base(PaygramResponseTypes.ResponseCreateRedEnvelope)
		{

		}
		public ResponseCreateRedEnvelope(string error) : base(PaygramResponseTypes.ResponseCreateRedEnvelope)
		{
			Message = error;
		}
	}
}
