namespace PayGram.Public.Requests
{
	/// <summary>
	/// Used by BlockUserV2 / UnblockUserV2. The target user is identified by his telegram id.
	/// </summary>
	public class BlockUserRequest
	{
		public long TelegramId { get; set; }
	}
}
