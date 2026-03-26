namespace PayGram.Public.Client
{
	/// <summary>
	/// Returned by <c>Convert</c> / <c>ConvertV2</c>.
	/// Contains the result of a read-only currency conversion quote. No funds are moved.
	/// </summary>
	public class ResponseConvert : PaygramResponse
	{
		/// <summary>
		/// The converted amount in the destination currency at the current exchange rate.
		/// </summary>
		public decimal Result { get; set; }

		public ResponseConvert() : base(PaygramResponseTypes.ResponseConvert)
		{
		}
		public ResponseConvert(decimal result) : base(PaygramResponseTypes.ResponseConvert, ResponseCodes.ResponseOK)
		{
			Result = result;
		}
		public ResponseConvert(string err)
					: base(err, PaygramResponseTypes.ResponseConvert, ResponseCodes.ResponseGenericError)
		{
		}
		public ResponseConvert(ResponseCodes code)
					: base(PaygramResponseTypes.ResponseConvert, code)
		{
		}
	}
}
