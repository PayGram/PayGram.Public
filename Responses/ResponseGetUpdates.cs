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
	}
}
