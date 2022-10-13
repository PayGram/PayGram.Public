using Newtonsoft.Json;

namespace PayGram.Public.UserAPI
{
    public class UserCallbackBalanceInfo : UserCallBackTransaction
    {
        /// <summary>
        /// The balance after the transaction
        /// </summary>
        public decimal Balance { get; set; }
    }
}
