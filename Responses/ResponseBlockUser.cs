namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>BlockUserV2</c> and <c>UnblockUserV2</c>.
	/// </summary>
	public class ResponseBlockUser : PaygramResponse
	{
		public long TelegramId { get; set; }
		/// <summary>
		/// Human readable list of the permissions the user has after the operation
		/// </summary>
		public string? Permissions { get; set; }

		public ResponseBlockUser() : base(PaygramResponseTypes.ResponseBlockUser)
		{
		}
		public ResponseBlockUser(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseBlockUser, code)
		{
		}
		public ResponseBlockUser(ResponseCodes code) : base(PaygramResponseTypes.ResponseBlockUser, code)
		{
		}
	}
}
