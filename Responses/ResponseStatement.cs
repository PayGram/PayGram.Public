namespace PayGram.Public.Responses
{
	public class ResponseStatement : PaygramResponse
	{
		public IList<StatementEntry> StatementEntries { get; set; } = new List<StatementEntry>();
		public int Results => StatementEntries.Count;
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
