using CurrenciesLib;
using CurrenciesLib.Cryptos;

namespace PayGram.Types
{
	public class RequestTopUp : PaygramRequest
	{
		/// <summary>
		/// The provider used to process the topup, or zero if a provider should be chosen automatically
		/// </summary>
		public int Provider { get; set; }
		/// <summary>
		/// The amount, expressed in PG$, that should be received
		/// </summary>
		public decimal PGAmount { get; set; }
		/// <summary>
		/// The currency that will be used to pay for the topup
		/// For fiat currencies it is the symbol such EUR, USD and so on
		/// It is the string representing one of the <see cref="Currencies"/>.
		/// If the CurrencyCode represents a crypto currency it might be followed by the <see cref="Crypto.CRYPTO_NETWORK_SEPARATOR"/> and the network name 
		/// which is one of <see cref="CryptoCurrencies"/> for example USDT_ERC20
		/// </summary>
		public string CurrencyCode { get; set; }
		/// <summary>
		/// The data that will be sent back to the user/client when there are updates for the topup
		/// </summary>
		public string CallbackData { get; set; }
	}
}
