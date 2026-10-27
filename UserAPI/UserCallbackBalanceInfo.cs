using Newtonsoft.Json;

namespace PayGram.Public.UserAPI
{
	public class UserCallbackBalanceInfo : UserCallBackTransaction
	{
		/// <summary>
		/// The balance after the transaction
		/// </summary>
		public decimal Balance { get; set; }
		/// <summary>
		/// The counterpart of the movement, as the client that owns it knows the user: who paid an invoice (on the callback to
		/// the payee), who redeemed a voucher or a red envelope (on the callback to the sender), who sent a direct transfer
		/// (on the callback to the receiver), the sender of a voucher (on the callback to the redeemer). The counterpart may be a
		/// user of another PayGram client: <see cref="FromClientId"/> says which, so the id is matched against the right user base.
		/// Null when the movement has no counterpart (a deposit, a swap, a fee).
		/// </summary>
		public string? UserIdInClientFrom { get; set; }
		/// <summary>
		/// The PayGram client of <see cref="UserIdInClientFrom"/>. Compare it with your own client id (<c>ResponseUserInfo.ClientId</c>)
		/// to know whether the id belongs to your user base. Null when <see cref="UserIdInClientFrom"/> is null.
		/// </summary>
		public int? FromClientId { get; set; }
		/// <summary>
		/// The code (guid) of the invoice this movement belongs to: the same code the statement shows for it
		/// (<c>StatementEntry.Invoice</c>). For a crypto deposit, the deposit account (deposit invoice) that received it: an
		/// account receives many deposits, so the code alone does not tell one deposit from another. For a direct transfer, its
		/// voucher (<see cref="UserCallbackMoneySent.VoucherCode"/>); for a withdrawal, the withdrawal (<see cref="WithdrawInfo.WithdrawId"/>).
		/// Always set on <see cref="UserCallbackInvoiceInfo"/>. Absent (null) in callbacks stored before 3.9.0. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? InvoiceCode { get; set; }
	}
}
