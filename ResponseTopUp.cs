using PayGram.Public;

namespace PayGram.Types
{
    public class ResponseTopUp : PaygramResponse
    {
        public decimal AmountToSendInChosenCurrency { get; set; }
        public decimal CoinTotalFees { get; set; }
        public decimal CoinAmountExcFees { get { return CoinAmountWillReceive + CoinTotalFees; } }
        /// <summary>
        /// The currency used to deposit
        /// It is the string representing one of the <see cref="Currencies"/>.
        /// If the CurrencyCode represents a crypto currency it might be followed by the <see cref="Crypto.CRYPTO_NETWORK_SEPARATOR"/> and the network name 
        /// which is one of <see cref="CryptoCurrencies"/> for example USDT_ERC20
        /// </summary>
        public string PaymentCurrency { get; set; }
        public decimal CoinAmountWillReceive { get; set; }
        public decimal ChangeRateExclFees { get { return AmountToSendInChosenCurrency / CoinAmountExcFees; } }
        public decimal ChangeRateInclFees { get { return AmountToSendInChosenCurrency / CoinAmountWillReceive; } }

        public ResponseTopUp()
                    : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseTopUp)))
        {

        }
    }
}
