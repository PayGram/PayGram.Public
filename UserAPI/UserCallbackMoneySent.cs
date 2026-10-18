using System.ComponentModel;

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
		/// True when <see cref="UserCliToId"/> is a user of the client receiving this callback. Defaults to true for callbacks produced
		/// before this field existed.
		/// </summary>
		// the default is true: IgnoreAndPopulate must drop true (not false) when storing, and a missing field reads back as true
		[DefaultValue(true)]
		public bool UserCliToBelongsToClient { get; set; } = true;
		public Guid VoucherCode { get; set; }
		public string? FriendlyVoucherCode { get; set; }
		public string? RedeemUrl { get; set; }
	}
}
