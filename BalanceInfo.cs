using CurrenciesLib;

namespace PayGram.Public
{
	/// <summary>
	/// Represents a single wallet balance for a user. Used in <see cref="Responses.ResponseUserInfo.Balances"/>
	/// and callback notifications. Each wallet corresponds to one currency.
	/// </summary>
	public class BalanceInfo
	{
		/// <summary>
		/// The current balance in this wallet's <see cref="Currency"/>.
		/// </summary>
		public decimal Balance { get; set; }
		/// <summary>
		/// The currency of this wallet.
		/// </summary>
		public Currencies Currency { get; set; }
	}
}
