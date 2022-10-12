using CurrenciesLib;

namespace PayGram.Types
{
	public class BalanceInfo
	{
		public decimal Balance { get; set; }
		public Currencies Currency { get; set; }
	}
}
