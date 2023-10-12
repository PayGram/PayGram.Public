namespace PayGram.Public
{
	public class ResponseSetBusiness : PaygramResponse
	{
		public ResponseSetBusiness() : base(PaygramResponseTypes.ResponseSetBusiness)
		{

		}
		public ResponseSetBusiness(ResponseCodes code) : base(PaygramResponseTypes.ResponseSetBusiness, code)
		{

		}

		public long BusinessId { get; set; }
	}
}
