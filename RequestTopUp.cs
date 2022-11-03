using CurrenciesLib;
using CurrenciesLib.Cryptos;

namespace PayGram.Public
{
    public class RequestTopUp : PaygramRequest
    {
        /// <summary>
        /// The provider used to process the topup, or zero if a provider should be chosen automatically
        /// </summary>
        public int Provider { get; set; }
        /// <summary>
        /// The amount, expressed in CurrencyCode, that should be received
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The currency that will be used to pay for the topup
        /// For fiat currencies it is the symbol such EUR, USD and so on
        /// It is the string representing one of the <see cref="Currencies"/>.
        /// </summary>
        public Currencies CurrencyCode { get; set; }
        public CryptoNetworks Network { get; set; }
        /// <summary>
        /// The data that will be sent back to the user/client when there are updates for the topup
        /// </summary>
        public string CallbackData { get; set; }
    }
}
