namespace PayGram.Public.UserAPI
{
	/// <summary>
	/// This is received only when direct transfer is used
	/// <see cref="PayGram.Public.Client.PayGramBotClient.TransferMoneyAsync"/>
	/// </summary>
	public class UserCallbackMoneySent : UserCallbackBalanceInfo
	{
		/// <summary>
		/// The client side user id of the receiver
		/// </summary>
		public string? UserCliToId { get; set; }
		/// <summary>
		/// The PayGram client of <see cref="UserCliToId"/>: compare it with your own client id (<c>ResponseUserInfo.ClientId</c>) to
		/// know whether the id belongs to your user base. Null for a voucher (no receiver yet).
		/// </summary>
		public int? ToClientId { get; set; }
		public Guid VoucherCode { get; set; }
		public string? FriendlyVoucherCode { get; set; }
		public string? RedeemUrl { get; set; }
	}
}
