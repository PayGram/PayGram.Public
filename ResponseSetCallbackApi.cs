using PayGram.Public;

namespace PayGram.Public
{
    public class ResponseSetCallbackApi : PaygramResponse
    {
        public string CallbackApi { get; set; }

        public ResponseSetCallbackApi(string callbackApi)
                : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseSetCallbackApi)))
        {
            this.CallbackApi = callbackApi;
        }
    }
}
