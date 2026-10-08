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
		/// this property is also included if the payer belongs to another client.
		/// On the "voucher redeemed" callback sent to the voucher's sender it is set only when the sender itself
		/// redeemed the voucher (a recall by hand, or the automatic return of an unclaimed voucher): the sender can
		/// then tell its own recalls apart; who else redeemed it is never disclosed.
		/// </summary>
		public string? UserIdInClientFrom { get; set; }
	}
}
