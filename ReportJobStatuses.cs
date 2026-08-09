namespace PayGram.Public
{
	public enum ReportJobStatuses
	{
		/// <summary>
		/// The report was booked and is waiting to be generated
		/// </summary>
		Requested = 0,
		/// <summary>
		/// A worker is currently generating the report
		/// </summary>
		Processing = 1,
		/// <summary>
		/// The report file was generated and can be downloaded
		/// </summary>
		Ready = 2,
		/// <summary>
		/// The report was downloaded at least once
		/// </summary>
		Delivered = 3,
		/// <summary>
		/// The generation failed permanently (max retries exceeded)
		/// </summary>
		Failed = 4,
		/// <summary>
		/// The report file was deleted after the retention period elapsed
		/// </summary>
		Expired = 5,
	}
}
