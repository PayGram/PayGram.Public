using CurrenciesLib;

namespace PayGram.Public.Requests
{
	public class P2PGetGroupsRequest : PaygramRequest
	{
		public P2PGroupType P2PGroupType { get; set; }
		public Currencies TraderCurrency { get; set; }
		public Currencies NeededCurrency { get; set; }
		public bool OwnOnly { get; set; }
	}
}
