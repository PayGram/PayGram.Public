using PayGram.Public;
using PayGram.Public.UserAPI;

namespace PayGram.Public.Responses
{
	public class P2PGroupListResponse : PaygramResponse
	{
		public P2PGroupListResponse() : base(PaygramResponseTypes.ResponseP2PGroupList)
		{
		}
		public List<P2PGroup> Groups { get; set; } = new List<P2PGroup>();
	}
}