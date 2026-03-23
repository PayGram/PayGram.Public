namespace PayGram.Public.Client
{
	public class ResponseConvert : PaygramResponse
	{
		public decimal Result { get; set; }

		public ResponseConvert(decimal result) : base(PaygramResponseTypes.ResponseConvert)
		{
			Result = result;
			Success = true;
		}
		public ResponseConvert(string err)
					: base(err, PaygramResponseTypes.ResponseConvert, ResponseCodes.ResponseGenericError)
		{
		}
		public ResponseConvert(ResponseCodes code)
					: base(PaygramResponseTypes.ResponseConvert, code)
		{
		}
	}
}
