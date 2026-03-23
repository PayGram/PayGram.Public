namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>RedeemVoucherV2</c>.
	/// Redeems a voucher or red envelope, crediting the funds to the caller's account.
	/// </summary>
	public class RedeemVoucherRequest
	{
		/// <summary>The human-readable voucher code to redeem (e.g. "ABC-1234").</summary>
		public string VoucherCode { get; set; } = string.Empty;
	}
}
