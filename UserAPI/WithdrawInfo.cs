namespace PayGram.Public.UserAPI
{
	public class WithdrawInfo
	{
		public long InternalId { get; set; }
		public WithdrawStatuses Status { get; set; }
		/// <summary>
		/// The amount to send in the requested currency
		/// </summary>
		public decimal Amount { get; set; }
		public decimal Fees { get; set; }
		public string UserId { get; set; }
		public decimal BalanceBeforeWithdraw { get; set; }
		public Guid WithdrawId { get; set; }
		public WithdrawMethod WithdrawMethod { get; set; }
	}
}