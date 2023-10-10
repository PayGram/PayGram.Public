namespace PayGram.Public.Responses
{
	public class ResponseSetCallbackApi : PaygramResponse
	{
		public string CallbackApi { get; set; }

		public ResponseSetCallbackApi(string callbackApi)
				: base(PaygramResponseTypes.ResponseSetCallbackApi)
		{
			CallbackApi = callbackApi;
		}
	}
}
