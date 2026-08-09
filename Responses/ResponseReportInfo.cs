namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>BookReport</c> and <c>GetReportStatus</c>.
	/// Describes a statement-export job: its identifier, current status and date range.
	/// </summary>
	public class ResponseReportInfo : PaygramResponse
	{
		public long ReportId { get; set; }
		public ReportJobStatuses Status { get; set; }
		public ReportFormats Format { get; set; }
		/// <summary>
		/// Null when the report covers the full history of the user
		/// </summary>
		public DateTime? FromUtc { get; set; }
		public DateTime ToExcludingUtc { get; set; }
		public DateTime CreatedUtc { get; set; }
		public DateTime? CompletedUtc { get; set; }
		/// <summary>
		/// Suggested file name, set once the report is Ready
		/// </summary>
		public string? FileName { get; set; }

		public ResponseReportInfo() : base(PaygramResponseTypes.ResponseReportInfo)
		{
		}
		public ResponseReportInfo(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseReportInfo, code)
		{
		}
		public ResponseReportInfo(ResponseCodes code) : base(PaygramResponseTypes.ResponseReportInfo, code)
		{
		}
	}
}
