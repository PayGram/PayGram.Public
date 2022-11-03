using PayGram.Public;

namespace PayGram.Public
{
	public class RequestStatement : PaygramRequest
	{
		public DateTime FromUtc { get; set; }
		public DateTime ToExcludingUtc { get; set; }
		public long StartIdx { get; set; }
		public int ResultsPerPage { get; set; }
		public bool GoForward { get; set; }
	}
}
