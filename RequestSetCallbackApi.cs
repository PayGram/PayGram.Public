using PayGram.Public;
using System.ComponentModel.DataAnnotations;

namespace PayGram.Types
{
	public class RequestSetCallbackApi : PaygramRequest
	{
		[StringLength(Constraints.CALLBACKURL_MAX)]
		public string Url { get; set; }
	}
}
