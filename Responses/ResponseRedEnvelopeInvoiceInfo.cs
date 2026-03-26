namespace PayGram.Public.Responses
{
	/// <summary>
	/// Represents a single redemption of a red envelope, recording who redeemed it, how much, and when.
	/// </summary>
	/// <param name="Amount">The amount redeemed including the fees, in the red envelope's currency.</param>
	/// <param name="Fees">The fees deducted from this redemption.</param>
	/// <param name="UserIdInClient">The client-side user ID of the redeemer.</param>
	/// <param name="RedeemedAtUtc">The UTC timestamp when the redemption occurred.</param>
	public record InvoiceRedeemer(decimal Amount, decimal Fees, string UserIdInClient, DateTime RedeemedAtUtc);

	/// <summary>
	/// Returned by <c>InvoiceInfo</c> / <c>InvoiceInfoV2</c> when the invoice is a red envelope.
	/// Extends <see cref="ResponseInvoiceInfo"/> with red envelope-specific fields such as the list of redeemers
	/// and remaining available amount.
	/// </summary>
	public class ResponseRedEnvelopeInvoiceInfo : ResponseInvoiceInfo
	{
		/// <summary>
		/// The amount still available for redemption (Amount - ExpectedFee - RedeemedAmount). Computed property.
		/// </summary>
		public decimal AvailableAmount => Amount - ExpectedFee - RedeemedAmount;
		/// <summary>
		/// The total amount already redeemed by all redeemers, including their fees.
		/// </summary>
		public decimal RedeemedAmount { get; set; }
		/// <summary>
		/// The number of users who have already redeemed this red envelope.
		/// </summary>
		public int RedeemersCount { get; set; }
		/// <summary>
		/// The maximum number of users allowed to redeem this red envelope.
		/// </summary>
		public int MaxRedeemers { get; set; }
		/// <summary>
		/// The list of all individual redemptions, with details on who redeemed, how much, and when.
		/// </summary>
		public List<InvoiceRedeemer> Redeemers { get; set; }

		public ResponseRedEnvelopeInvoiceInfo() : base(PaygramResponseTypes.ResponseRedEnvelopeInvoiceInfo)
		{
		}
		public ResponseRedEnvelopeInvoiceInfo(ResponseCodes code) : base(PaygramResponseTypes.ResponseRedEnvelopeInvoiceInfo, code)
		{

		}
	}
}
