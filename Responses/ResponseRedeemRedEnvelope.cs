namespace PayGram.Public.Responses
{
	public class ResponseRedeemRedEnvelope : ResponseTopUpReceived
	{
		public int RedeemersCount { get; set; }
		public int MaxRedeemers { get; set; }
		public decimal RedeemedAmount { get; set; }
		public decimal TotalAmount { get; set; }
		public ResponseRedeemRedEnvelope()
		{
			Type = PaygramResponseTypes.ResponseRedeemRedEnvelope;
		}
	}
}
