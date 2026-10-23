using CurrenciesLib;

namespace PayGram.Public.UserAPI
{
	public class WithdrawAdminResponse
	{
		/// <summary>
		/// Internal note of the operator: always null in the callbacks sent to clients
		/// </summary>
		public string? AdminComment { get; set; }
		/// <summary>
		/// Can be set by admins, can be read by authorized user/client
		/// </summary>
		public string? AdminResponse { get; set; }
		/// <summary>
		/// The status of the withdraw this reply comes with: AcceptedPendingTransfer (8) approved, Completed (2) sent (the tx hash
		/// in AdminResponse), Cancelled (3) rejected and refunded (the reason in AdminResponse). A withdraw cannot be cancelled
		/// or changed once submitted: only an admin rejects it, and the reject always refunds.
		/// </summary>
		public int InvoiceStatus { get; set; }
		/// <summary>
		/// The amount effectively transferred
		/// When an admin replies, if the status is complete, AmountSent=0 and CurrencyCode=null, the amount will be taken from the originating invoice
		/// </summary>
		public decimal AmountSent { get; set; }
		/// <summary>
		/// The currency code of AmountSent
		/// It is the string representing one of the <see cref="Currencies"/>.
		/// </summary>
		public Currencies Currency { get; set; }
		/// <summary>Not used any more (the destination of a withdraw never changes): always null. Kept for the shape of the callback</summary>
		public WithdrawMethod UpdatedWithdrawMethod { get; set; }
		/// <summary>
		/// The network fee charged by the crypto sender, when known. Used to record the
		/// on-chain leg of the withdrawal; not shown to the user.
		/// </summary>
		public decimal? NetworkFee { get; set; }
		/// <summary>
		/// The currency of <see cref="NetworkFee"/> as reported by the sender.
		/// </summary>
		public string? NetworkFeeCurrency { get; set; }
	}
}
