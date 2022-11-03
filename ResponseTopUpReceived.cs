using CurrenciesLib;
using PayGram.Public;

namespace PayGram.Public
{
    public class ResponseTopUpReceived : PaygramResponse
    {
        /// <summary>
        /// The amount of money received in TransactionCurrency
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// The amount of money credited
        /// </summary>
        public decimal Credited { get; set; }
        /// <summary>
        /// The fees paid for this transaction
        /// </summary>
        public decimal Fees { get; set; }
        public string CallbackData { get; set; }
        public Currencies CurrencyCode { get; set; }
        public decimal NewBalance { get; set; }

        public ResponseTopUpReceived()
                            : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseTopUpReceived)))
        {

        }

    }
}
