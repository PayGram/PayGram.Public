using CurrenciesLib;
using CurrenciesLib.Cryptos;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>DepositCreditV2</c>.
	/// Generates a crypto deposit address; funds sent there are credited to the user's balance.
	/// </summary>
	public class DepositCreditRequest
	{
		/// <summary>Amount expected to be deposited (must be &gt; 0).</summary>
		public decimal Amt { get; set; }
		/// <summary>Currency to deposit (e.g. USDT, BTC).</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Blockchain network for the deposit address (e.g. TRC20, ERC20, BEP20).</summary>
		public CryptoNetworks Network { get; set; }
		/// <summary>Optional callback data returned when the deposit is confirmed.</summary>
		public string? Cd { get; set; }
	}
}
