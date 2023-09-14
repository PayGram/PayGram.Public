using PayGram.Public.UserAPI;

namespace PayGram.Public
{
	public class RequestSetBusiness : PaygramRequest
	{
		public Business Business { get; set; }
	}
}
