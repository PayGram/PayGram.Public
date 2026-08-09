namespace PayGram.Public.UserAPI
{
	/// <summary>
	/// Sent when a booked statement-export report changes state
	/// (typically when it becomes <see cref="ReportJobStatuses.Ready"/> or <see cref="ReportJobStatuses.Failed"/>).
	/// The report content is never embedded here: the client downloads it via the DownloadReport endpoint.
	/// </summary>
	public class UserCallbackReportInfo
	{
		public long ReportId { get; set; }
		public ReportJobStatuses Status { get; set; }
		public ReportFormats Format { get; set; }
		public DateTime FromUtc { get; set; }
		public DateTime ToExcludingUtc { get; set; }
		/// <summary>
		/// Suggested file name for the downloaded report
		/// </summary>
		public string? FileName { get; set; }
	}
}
