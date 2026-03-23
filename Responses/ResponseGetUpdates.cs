using PayGram.Public.UserAPI;

namespace PayGram.Public.Responses
{
	public class ResponseGetUpdates : PaygramResponse
	{
		public List<UserCallbackInfo> Updates { get; set; }

		public ResponseGetUpdates()
			: base(PaygramResponseTypes.ResponseGetExchangeRates)
		{
			Updates = new List<UserCallbackInfo>();
		}
		public ResponseGetUpdates(string err)
			: base(err, PaygramResponseTypes.ResponseGetExchangeRates, ResponseCodes.ResponseGenericError)
		{
			Updates = new List<UserCallbackInfo>();
		}
		public ResponseGetUpdates(ResponseCodes code)
			: base(PaygramResponseTypes.ResponseGetExchangeRates, code)
		{
			Updates = new List<UserCallbackInfo>();
		}
	}
}
