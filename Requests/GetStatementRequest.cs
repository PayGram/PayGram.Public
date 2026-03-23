namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>GetStatementV2</c>.
	/// Retrieves the user's transaction history for a given date range with pagination.
	/// </summary>
	public class GetStatementRequest
	{
		/// <summary>Start of the date range (inclusive, UTC).</summary>
		public DateTime FromUtc { get; set; }
		/// <summary>End of the date range (exclusive, UTC).</summary>
		public DateTime ExcludingToUtc { get; set; }
		/// <summary>Starting index for pagination (0-based).</summary>
		public int StIdx { get; set; }
		/// <summary>Number of results to return per page (max 1000).</summary>
		public int Results { get; set; }
		/// <summary>True to sort oldest-first, false for newest-first.</summary>
		public bool GoForward { get; set; }
	}
}
