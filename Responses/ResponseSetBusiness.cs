namespace PayGram.Public
{
	public class ResponseSetBusiness : PaygramResponse
	{
		public ResponseSetBusiness() : base(PaygramResponseTypes.ResponseSetBusiness)
		{

		}
		public ResponseSetBusiness(ResponseCodes code) : base(code)
		{

		}

		public long BusinessId { get; set; }
	}
}
