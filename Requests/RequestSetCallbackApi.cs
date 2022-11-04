using System.ComponentModel.DataAnnotations;

namespace PayGram.Public.Requests
{
    public class RequestSetCallbackApi : PaygramRequest
    {
        [StringLength(Constraints.CALLBACKURL_MAX)]
        public string Url { get; set; }
    }
}
