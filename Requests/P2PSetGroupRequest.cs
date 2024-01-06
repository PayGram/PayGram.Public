using PayGram.Public.UserAPI;

namespace PayGram.Public.Requests
{
	public class P2PSetGroupRequest : PaygramRequest
	{
		public P2PSetGroupRequest()
		{

		}

		public P2PGroup GroupInfo { get; set; }
	}
}
