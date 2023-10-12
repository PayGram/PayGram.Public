namespace PayGram.Public
{
	public class P2PSetGroupResponse : PaygramResponse
	{
		public P2PSetGroupResponse() : base(PaygramResponseTypes.ResponseSetP2PGroup)
		{
		}
		public P2PSetGroupResponse(ResponseCodes code) : base(PaygramResponseTypes.ResponseSetP2PGroup, code)
		{

		}

		public int P2PGroupId { get; set; }
	}
}
