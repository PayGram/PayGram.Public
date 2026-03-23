using PayGram.Public.UserAPI;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>GetUpdates</c>.
	/// Contains a list of pending notifications for the authenticated user, such as incoming transfers,
	/// deposit confirmations, invoice payments, and withdrawal status changes.
	/// Updates are marked as delivered once the response is successfully sent to the caller.
	/// </summary>
	public class ResponseGetUpdates : PaygramResponse
	{
		/// <summary>
		/// List of pending notifications. Each <see cref="UserCallbackInfo"/> contains details about a specific event
		/// (balance change, money sent, invoice update, or withdrawal update) along with a unique Id, timestamp, and callback data.
		/// </summary>
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
