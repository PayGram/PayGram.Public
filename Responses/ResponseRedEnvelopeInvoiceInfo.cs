namespace PayGram.Public.Responses
{
	public class ResponseRedEnvelopeInvoiceInfo : ResponseInvoiceInfo
	{
		public decimal AvailableAmount => Amount - ExpectedFee - RedeemedAmount;
        public decimal RedeemedAmount { get; set; }
        public int RedeemersCount { get; set; }
        public int MaxRedeemers { get; set; }
        public ResponseRedEnvelopeInvoiceInfo() : base()
		{
			Type = PaygramResponseTypes.ResponseRedEnvelopeInvoiceInfo;
		}
	}
}
