namespace PayGram.Public.UserAPI
{
	public class UserCallbackInvoiceInfo : UserCallbackBalanceInfo
	{
		public string? FriendlyVoucherCode { get; set; }
		public Guid InvoiceCode { get; set; }
		// the counterpart (who paid, who redeemed, who sent) is UserIdInClientFrom / FromClientId of UserCallbackBalanceInfo
	}
}
