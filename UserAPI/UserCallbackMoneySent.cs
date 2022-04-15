namespace PayGram.Public.UserAPI
{
    /// <summary>
    /// This is received only when direct transfer is used
    /// <see cref="Telegram.PayGramBotClient.TransferMoneyAsync(long, double, string)"/>
    /// </summary>
    public class UserCallbackMoneySent : UserCallbackBalanceInfo
    {
        /// <summary>
        /// The client side user id of the receiver
        /// </summary>
        public string UserCliToId { get; set; }
    }
}
