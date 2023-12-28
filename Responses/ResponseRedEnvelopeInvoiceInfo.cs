namespace PayGram.Public.Responses
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="Amount">The amount including the fees</param>
	/// <param name="Fees">The fees</param>
	/// <param name="UserIdInClient"></param>
	/// <param name="RedeemedAtUtc"></param>
	public record RedEnvRedeemer(decimal Amount, decimal Fees, string UserIdInClient, DateTime RedeemedAtUtc);

	public class ResponseRedEnvelopeInvoiceInfo : ResponseInvoiceInfo
	{
		public decimal AvailableAmount => Amount - ExpectedFee - RedeemedAmount;
		/// <summary>
		/// The redeemed amount including the fees
		/// </summary>
		public decimal RedeemedAmount { get; set; }
		public int RedeemersCount { get; set; }
		public int MaxRedeemers { get; set; }
		public List<RedEnvRedeemer> Redeemers { get; set; }

		public ResponseRedEnvelopeInvoiceInfo() : base()
		{
			Type = PaygramResponseTypes.ResponseRedEnvelopeInvoiceInfo;
		}
		public ResponseRedEnvelopeInvoiceInfo(ResponseCodes code) : base(PaygramResponseTypes.ResponseRedEnvelopeInvoiceInfo, code)
		{

		}
	}
}
