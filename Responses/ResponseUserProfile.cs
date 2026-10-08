using System;
using System.Collections.Generic;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Admin-only recap of a user of the calling client: wallets, rights, what it has outstanding.
	/// </summary>
	public class ResponseUserProfile : PaygramResponse
	{
		/// <summary>The user id at the client side (the telegram id for the bot)</summary>
		public string? UserCliId { get; set; }
		/// <summary>The PayGram id of the user</summary>
		public int UserId { get; set; }
		public DateTime JoinedOn { get; set; }
		/// <summary>The PayGram rights bits (1 Normal, 4 Admin, 8 Rooot, 32 Loader; 0 banned)</summary>
		public int UserRights { get; set; }
		public string? RightsNames { get; set; }
		/// <summary>Whether the user set a webhook for its callbacks</summary>
		public bool HasCallbackUrl { get; set; }
		public List<BalanceInfo> Balances { get; set; } = new();
		/// <summary>Vouchers sent by the user and not redeemed yet</summary>
		public int OpenVouchers { get; set; }
		/// <summary>Total of the open vouchers, per currency</summary>
		public List<BalanceInfo> OpenVouchersAmounts { get; set; } = new();
		/// <summary>Red envelopes of the user still open</summary>
		public int OpenRedEnvelopes { get; set; }
		/// <summary>What is still inside the open red envelopes, per currency</summary>
		public List<BalanceInfo> OpenRedEnvelopesAmounts { get; set; } = new();
		/// <summary>Withdrawals requested, approved or being refunded</summary>
		public int PendingWithdrawals { get; set; }

		public ResponseUserProfile() : base(PaygramResponseTypes.ResponseUserProfile) { }
		public ResponseUserProfile(ResponseCodes code) : base(PaygramResponseTypes.ResponseUserProfile, code) { }
		public ResponseUserProfile(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseUserProfile, code) { }
	}
}
