using PayGram.Public;

namespace PayGram.Types
{
    public class ResponseTopUpReceived : PaygramResponse
    {
        /// <summary>
        /// The amount of money received in TransactionCurrency
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// The amount of money credited in PGD
        /// </summary>
        public decimal Credited { get; set; }
        /// <summary>
        /// The fees paid for this transaction in PGD
        /// </summary>
        public decimal Fees { get; set; }
        public string CallbackData { get; set; }
        public string CurrencyCode { get; set; }
        public decimal NewBalance { get; set; }

        public ResponseTopUpReceived()
                            : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseTopUpReceived)))
        {

        }

    }
}
