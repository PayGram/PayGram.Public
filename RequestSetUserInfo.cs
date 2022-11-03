using PayGram.Public;
using System.ComponentModel.DataAnnotations;

namespace PayGram.Public
{
	public class RequestSetUserInfo : PaygramRequest
	{
		[StringLength(Constraints.CALLBACKURL_MAX)]
		public string CallbackUrl { get; set; }
	}
}
