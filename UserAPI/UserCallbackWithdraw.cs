namespace PayGram.Public.UserAPI
{
    public class UserCallbackWithdraw : UserCallbackBalanceInfo
    {
        public WithdrawInfo Info { get; set; }
        public WithdrawAdminResponse AdminResponse { get; set; }
    }

}