using CurrenciesLib;

namespace PayGram.Public.UserAPI
{
    public class WithdrawInfo
    {
        public WithdrawStatuses Status { get; set; }
        /// <summary>
        /// The amount to send in the requested currency
        /// </summary>
        public decimal AmountInRequestedCurrency { get; set; }
        public decimal FeesRequestedCurrency { get; set; }
        /// <summary>
        /// The requested currency chosen to withdraw
        /// </summary>
        public Currencies RequestedCurrency { get; set; }
        public string UserId { get; set; }
        public decimal BalanceBeforeWithdraw { get; set; }
        public Guid WithdrawId { get; set; }
        public WithdrawMethod WithdrawMethod { get; set; }
    }
}