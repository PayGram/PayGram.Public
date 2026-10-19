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
	}
}
