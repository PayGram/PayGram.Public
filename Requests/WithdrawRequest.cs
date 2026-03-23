using CurrenciesLib;
using CurrenciesLib.Cryptos;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>WithdrawV2</c>.
	/// Initiates a cryptocurrency withdrawal to an external wallet address. Subject to approval.
	/// </summary>
	public class WithdrawRequest
	{
		/// <summary>Amount to withdraw (must be &gt; 0).</summary>
		public decimal Amt { get; set; }
		/// <summary>Currency to withdraw (e.g. USDT, BTC).</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Blockchain network for the withdrawal (e.g. TRC20, ERC20, BEP20).</summary>
		public CryptoNetworks Network { get; set; }
		/// <summary>Destination wallet address.</summary>
		public string Address { get; set; } = string.Empty;
		/// <summary>Client-generated idempotency key (max 64 bytes). Prevents duplicate withdrawals.</summary>
		public string Unique { get; set; } = string.Empty;
		/// <summary>Optional callback data returned in notifications related to this withdrawal.</summary>
		public string? Cd { get; set; }
	}
}
