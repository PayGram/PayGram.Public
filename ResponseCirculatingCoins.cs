using PayGram.Public;

namespace PayGram.Types
{
	public class ResponseCirculatingCoins : PaygramResponse
	{
		public IList<BalanceInfo> Coins { get; set; } = new List<BalanceInfo>();
		public ResponseCirculatingCoins() : base(PaygramResponseTypes.ResponseCirculatingCoins)
		{

		}
	}
}
