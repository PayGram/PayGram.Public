namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>GetStatement</c> / <c>GetStatementV2</c>.
	/// Contains a paginated list of ledger entries (deposits, withdrawals, transfers, swaps, fees)
	/// for a single user, ordered by entry ID.
	/// </summary>
	public class ResponseStatement : PaygramResponse
	{
		/// <summary>
		/// The ledger entries in this page. Each entry represents one debit or credit event
		/// on the user's account, mapped from <c>DbStatement</c> rows.
		/// </summary>
		public IList<StatementEntry> StatementEntries { get; set; } = new List<StatementEntry>();
		/// <summary>
		/// The number of entries in <see cref="StatementEntries"/>. Computed from the list count.
		/// </summary>
		public int Results => StatementEntries.Count;
		/// <summary>
		/// True if there are more entries beyond this page (i.e. the DB returned more rows than <c>ResultsPerPage</c>).
		/// When paging backward (<c>GoForward = false</c>), this is always true.
		/// </summary>
		public bool NextPageAvailable { get; set; }
		//public long NextStartIdx { get; set; }
		public ResponseStatement() : base(PaygramResponseTypes.ResponseStatement)
		{

		}
		public ResponseStatement(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseStatement, code)
		{

		}
		public ResponseStatement(ResponseCodes code) : base(PaygramResponseTypes.ResponseStatement, code)
		{

		}
	}
}
