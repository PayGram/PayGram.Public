using CurrenciesLib;
using CurrenciesLib.Cryptos;
using PayGram.Public;

namespace PayGram.Types
{
    public class ResponseWithdrawAccepted : PaygramResponse
    {
        /// <summary>
        /// The balance after witdrawing the funds of the selected currency-account
        /// </summary>
        public decimal NewBalance { get; set; }
        /// <summary>
        /// The currency of the account from where the funds will be withdrawn from
        /// </summary>
        public Currencies CurrencyCode { get; set; }
        /// <summary>
        /// The fee for this transfer expressed in the CurrencyCode
        /// </summary>
        public decimal ExpectedFee { get; set; }
        /// <summary>
        /// The amount that will be deducted from the source account expressed by the CurrencyCode
        /// </summary>
        public decimal WithdrawnAmount { get; set; }
        /// <summary>
        /// The amount that will be sent to the user expressed in his chosen currency, net of the PayGram and crypto transaction fees, but excluding 
        /// additional interidiary bank or beneficiary bank fees. This will likely be the received amount.
        /// </summary>
        public decimal AmountWillReceive { get; set; }
        /// <summary>
        /// The currency that the user requested to receive the funds
        /// It is the string representing one of the <see cref="Currencies"/>.
        /// If the CurrencyCode represents a crypto currency it might be followed by the <see cref="Crypto.CRYPTO_NETWORK_SEPARATOR"/> and the network name 
        /// which is one of <see cref="CryptoCurrencies"/> for example USDT_ERC20
        /// </summary>
        public string CurrencyCodeTransfer { get; set; }
        /// <summary>
        /// The unique identifier for the invoice that identifies the withdraw operation
        /// </summary>
        public Guid InvoiceCode { get; set; }

        public ResponseWithdrawAccepted()
                                    : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseWithdrawAccepted)))
        {

        }
    }
}