using CurrenciesLib;

namespace PayGram.Public.Responses
{
    public class ResponseTopUp : PaygramResponse
    {
        public decimal AmountToSendInChosenCurrency { get; set; }
        public decimal CoinTotalFees { get; set; }
        public decimal CoinAmountExcFees { get { return CoinAmountWillReceive + CoinTotalFees; } }
        /// <summary>
        /// The currency used to deposit
        /// It is the string representing one of the <see cref="Currencies"/>.
        /// </summary>
        //public Currencies Currency { get; set; }
        //public CryptoNetworks Network { get; set; }
        public decimal CoinAmountWillReceive { get; set; }
        public decimal ChangeRateExclFees { get { return AmountToSendInChosenCurrency / CoinAmountExcFees; } }
        public decimal ChangeRateInclFees { get { return AmountToSendInChosenCurrency / CoinAmountWillReceive; } }

        public ResponseTopUp()
                    : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseTopUp)))
        {

        }
    }
}
