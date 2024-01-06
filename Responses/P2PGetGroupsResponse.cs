using PayGram.Public.UserAPI;

namespace PayGram.Public.Responses
{
	public class P2PGetGroupsResponse : PaygramResponse
	{
		public P2PGetGroupsResponse() : base(PaygramResponseTypes.ResponseP2PGroupList)
		{
		}
		public P2PGetGroupsResponse(ResponseCodes code) : base(PaygramResponseTypes.ResponseP2PGroupList, code)
		{

		}
		public List<P2PGroup> Groups { get; set; } = new List<P2PGroup>();
	}
}