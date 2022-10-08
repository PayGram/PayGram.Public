using CurrenciesLib;

namespace PayGram.Public.UserAPI
{
    public class UserCallBackTransaction
    {
        /// <summary>
        /// The effective amount of money, expressed in the Currency, that were credited or debited
        /// </summary>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// When this callback is sent to a merchant, the fees that were deducted from the TransactionAmount.
        /// When this callback is sent to a user, it is zero.
        /// </summary>
        public decimal Fees { get; set; }
        public Currencies Currency { get; set; }
    }
}
