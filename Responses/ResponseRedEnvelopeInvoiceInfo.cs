namespace PayGram.Public.Responses
{
	public record RedEnvRedeemer(decimal Amount, decimal Fee, string UserIdInClient);

	public class ResponseRedEnvelopeInvoiceInfo : ResponseInvoiceInfo
	{
		public decimal AvailableAmount => Amount - ExpectedFee - RedeemedAmount;
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
