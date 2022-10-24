namespace PayGram.Public
{
	public class ResponseSwap : PaygramResponse
	{
		public decimal ReceivedAmount { get; set; }
		public decimal FeesRequestedCurrency { get; set; }
		public decimal BalanceSourceAfter { get; set; }
		public decimal BalanceDestAfter { get; set; }
		public ResponseSwap() : base(PaygramResponseTypes.ResponseSwap)
		{

		}
	}
}
