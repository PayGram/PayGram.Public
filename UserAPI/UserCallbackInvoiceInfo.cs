namespace PayGram.Public.UserAPI
{
	public class UserCallbackInvoiceInfo : UserCallbackBalanceInfo
	{
		public string? FriendlyVoucherCode { get; set; }
		public Guid InvoiceCode { get; set; }
		/// <summary>
		/// If the invoice is issued by a business merchant, when this notifification is sent to a merchant,
		/// this value will be populated
		/// with the user-clientid that paid the invoice. in all other cases, it will be blank.
		/// this property is also included if the payer belongs to another client
		/// </summary>
		public string? UserIdInClientFrom { get; set; }
	}
}
