namespace PayGram.Public.Responses
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
		public ResponseSwap(string err)
					: base(err, PaygramResponseTypes.ResponseSwap, ResponseCodes.ResponseGenericError)
		{
		}
		public ResponseSwap(ResponseCodes code)
					: base(PaygramResponseTypes.ResponseSwap, code)
		{
		}
	}
}
