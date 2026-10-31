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
		/// <summary>
		/// The friendly code of the voucher. The same value as <see cref="UserCallbackBalanceInfo.FriendlyVoucherCode"/> (it is
		/// stored there); kept here, written even when null, so that this callback's body does not change
		/// </summary>
		public new string? FriendlyVoucherCode
		{
			get => base.FriendlyVoucherCode;
			set => base.FriendlyVoucherCode = value;
		}
		/// <summary>
		/// The link to redeem the voucher or the red envelope. Set only for the PayGram Telegram bot (a link to the bot): the
		/// PayGram API leaves it null for its own integrators on purpose (it does not depend on Telegram; where a voucher is
		/// redeemed from is the integrator's business). The PayGram bot API fills it for its integrators
		/// </summary>
		public string? RedeemUrl { get; set; }
	}
}
