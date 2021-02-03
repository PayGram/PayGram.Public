namespace PayGram.Types
{
	/// <summary>
	/// Gets the 20 most recent updates.
	/// If <see cref="UserCliId"/> is specified, the updates returned will be for the given user, 
	/// otherwise for the requesting client
	/// </summary>
	public class RequestGetUpdates : PaygramRequest
	{
		public RequestGetUpdates()
		{
		}
	}

	/// <summary>
	/// Request to mark a number of updates as sent-error
	/// </summary>
	public class RequestMarkUpdatesError : PaygramRequest
	{
		/// <summary>
		/// The ids of the updates that should be marked as unsent
		/// </summary>
		public int[] UpdateIds { get; set; }
	}
}
