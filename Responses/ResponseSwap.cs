namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>SwapAsync</c> / <c>SwapV2</c>.
	/// Contains the result of exchanging funds from one currency to another in the user's account.
	/// </summary>
	public class ResponseSwap : PaygramResponse
	{
		/// <summary>
		/// The amount received in the destination currency after the swap.
		/// </summary>
		public decimal ReceivedAmount { get; set; }
		/// <summary>
		/// The fees charged for the swap, expressed in the source currency.
		/// </summary>
		public decimal FeesRequestedCurrency { get; set; }
		/// <summary>
		/// The user's balance in the source currency after the swap.
		/// </summary>
		public decimal BalanceSourceAfter { get; set; }
		/// <summary>
		/// The user's balance in the destination currency after the swap.
		/// </summary>
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
