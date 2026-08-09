namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>CancelPendingCallbacks</c>.
	/// </summary>
	public class ResponseCancelPendingCallbacks : PaygramResponse
	{
		/// <summary>
		/// The number of pending callbacks that were cancelled
		/// </summary>
		public int CancelledCount { get; set; }

		public ResponseCancelPendingCallbacks() : base(PaygramResponseTypes.ResponseCancelPendingCallbacks)
		{
		}
		public ResponseCancelPendingCallbacks(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseCancelPendingCallbacks, code)
		{
		}
		public ResponseCancelPendingCallbacks(ResponseCodes code) : base(PaygramResponseTypes.ResponseCancelPendingCallbacks, code)
		{
		}
	}
}
